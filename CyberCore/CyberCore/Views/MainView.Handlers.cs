using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MyApp.Data;
using MyApp.Data.Models;

namespace CyberCore.Views;

public partial class MainView
{
    private void StudentsListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (this.FindControl<ListBox>("StudentsListBox")?.SelectedItem is not Student s) return;
        if (this.FindControl<TextBox>("StudentFullNameBox") is { } fn) fn.Text = s.FullName;
        if (this.FindControl<TextBox>("StudentNicknameBox") is { } nn) nn.Text = s.Nickname;
        if (this.FindControl<TextBox>("StudentMmrBox") is { } mmr) mmr.Text = s.CurrentMmr?.ToString() ?? "";
        if (this.FindControl<TextBox>("StudentPhoneBox") is { } ph) ph.Text = s.Phone;
        if (this.FindControl<CheckBox>("StudentIsActiveCheck") is { } act) act.IsChecked = s.IsActive;
    }

    private bool TryGetStudent(out Student data, out string err)
    {
        data = new Student();
        err = "";
        string fn = this.FindControl<TextBox>("StudentFullNameBox")?.Text?.Trim() ?? "";
        string nn = this.FindControl<TextBox>("StudentNicknameBox")?.Text?.Trim() ?? "";
        string mmrStr = this.FindControl<TextBox>("StudentMmrBox")?.Text?.Trim() ?? "";
        string ph = this.FindControl<TextBox>("StudentPhoneBox")?.Text?.Trim() ?? "";
        bool act = this.FindControl<CheckBox>("StudentIsActiveCheck")?.IsChecked ?? false;

        if (string.IsNullOrWhiteSpace(fn)) { err = "Введите ФИО ученика."; return false; }
        if (string.IsNullOrWhiteSpace(nn)) { err = "Введите никнейм."; return false; }
        if (string.IsNullOrWhiteSpace(ph)) { err = "Введите телефон."; return false; }

        int? mmr = null;
        if (!string.IsNullOrEmpty(mmrStr))
        {
            if (!int.TryParse(mmrStr, out int val) || val < 0) { err = "MMR должен быть числом >= 0."; return false; }
            mmr = val;
        }
        data = new Student { FullName = fn, Nickname = nn, CurrentMmr = mmr, Phone = ph, IsActive = act };
        return true;
    }

    private void ClearStudentForm()
    {
        if (this.FindControl<TextBox>("StudentFullNameBox") is { } fn) fn.Text = "";
        if (this.FindControl<TextBox>("StudentNicknameBox") is { } nn) nn.Text = "";
        if (this.FindControl<TextBox>("StudentMmrBox") is { } mmr) mmr.Text = "";
        if (this.FindControl<TextBox>("StudentPhoneBox") is { } ph) ph.Text = "";
        if (this.FindControl<CheckBox>("StudentIsActiveCheck") is { } act) act.IsChecked = true;
    }

    private void AddStudent_Click(object? sender, RoutedEventArgs e)
    {
        if (!TryGetStudent(out var st, out var err)) { ShowStatus("StudentStatusText", err, true); return; }
        try
        {
            using var db = new AppDbContext();
            db.Students.Add(st);
            db.SaveChanges();
            ClearStudentForm();
            RefreshAllLists();
            ShowStatus("StudentStatusText", "Ученик добавлен!", false);
        }
        catch (Exception ex) { ShowStatus("StudentStatusText", ex.InnerException?.Message ?? ex.Message, true); }
    }

    private void SaveStudent_Click(object? sender, RoutedEventArgs e)
    {
        if (this.FindControl<ListBox>("StudentsListBox")?.SelectedItem is not Student sel)
        {
            ShowStatus("StudentStatusText", "Выберите ученика в списке.", true);
            return;
        }
        if (!TryGetStudent(out var data, out var err)) { ShowStatus("StudentStatusText", err, true); return; }
        try
        {
            using var db = new AppDbContext();
            if (db.Students.Find(sel.Id) is not { } entity) { ShowStatus("StudentStatusText", "Не найден в БД.", true); return; }
            entity.FullName = data.FullName;
            entity.Nickname = data.Nickname;
            entity.CurrentMmr = data.CurrentMmr;
            entity.Phone = data.Phone;
            entity.IsActive = data.IsActive;
            db.SaveChanges();
            ClearStudentForm();
            RefreshAllLists();
            ShowStatus("StudentStatusText", "Изменения сохранены!", false);
        }
        catch (Exception ex) { ShowStatus("StudentStatusText", ex.InnerException?.Message ?? ex.Message, true); }
    }

    private void DeleteStudent_Click(object? sender, RoutedEventArgs e)
    {
        if (this.FindControl<ListBox>("StudentsListBox")?.SelectedItem is not Student sel)
        {
            ShowStatus("StudentStatusText", "Выберите ученика для удаления.", true);
            return;
        }
        try
        {
            using var db = new AppDbContext();
            if (db.Students.Find(sel.Id) is { } entity) { db.Students.Remove(entity); db.SaveChanges(); }
            ClearStudentForm();
            RefreshAllLists();
            ShowStatus("StudentStatusText", "Ученик удален.", false);
        }
        catch (Exception ex) { ShowStatus("StudentStatusText", ex.InnerException?.Message ?? ex.Message, true); }
    }

    private void AddCoach_Click(object? sender, RoutedEventArgs e)
    {
        string fn = this.FindControl<TextBox>("CoachFullNameBox")?.Text?.Trim() ?? "";
        string disc = this.FindControl<TextBox>("CoachDisciplineBox")?.Text?.Trim() ?? "";
        string rateStr = this.FindControl<TextBox>("CoachRateBox")?.Text?.Trim() ?? "";
        string expStr = this.FindControl<TextBox>("CoachExpBox")?.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(fn)) { ShowStatus("CoachStatusText", "Введите ФИО тренера.", true); return; }
        if (string.IsNullOrWhiteSpace(disc)) { ShowStatus("CoachStatusText", "Введите дисциплину.", true); return; }
        if (!int.TryParse(rateStr, out int rate) || rate < 0) { ShowStatus("CoachStatusText", "Неверная ставка.", true); return; }

        int? exp = null;
        if (!string.IsNullOrEmpty(expStr))
        {
            if (!int.TryParse(expStr, out int val) || val < 0) { ShowStatus("CoachStatusText", "Опыт должен быть числом >= 0.", true); return; }
            exp = val;
        }

        try
        {
            using var db = new AppDbContext();
            db.Coaches.Add(new Coach { FullName = fn, Discipline = disc, HourlyRate = rate, ExperienceYears = exp });
            db.SaveChanges();
            if (this.FindControl<TextBox>("CoachFullNameBox") is { } f) f.Text = "";
            if (this.FindControl<TextBox>("CoachDisciplineBox") is { } d) d.Text = "";
            if (this.FindControl<TextBox>("CoachRateBox") is { } r) r.Text = "";
            if (this.FindControl<TextBox>("CoachExpBox") is { } exB) exB.Text = "";
            RefreshAllLists();
            ShowStatus("CoachStatusText", "Тренер добавлен!", false);
        }
        catch (Exception ex) { ShowStatus("CoachStatusText", ex.InnerException?.Message ?? ex.Message, true); }
    }

    private void DeleteCoach_Click(object? sender, RoutedEventArgs e)
    {
        if (this.FindControl<ListBox>("CoachesListBox")?.SelectedItem is not Coach sel)
        {
            ShowStatus("CoachStatusText", "Выберите тренера для удаления.", true);
            return;
        }
        try
        {
            using var db = new AppDbContext();
            if (db.Coaches.Find(sel.Id) is { } entity) { db.Coaches.Remove(entity); db.SaveChanges(); }
            RefreshAllLists();
            ShowStatus("CoachStatusText", "Тренер удален.", false);
        }
        catch (Exception ex) { ShowStatus("CoachStatusText", ex.InnerException?.Message ?? ex.Message, true); }
    }
}
