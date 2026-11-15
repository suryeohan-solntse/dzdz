using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Data.Models;

namespace CyberCore.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        AvaloniaXamlLoader.Load(this);
        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();
            DbSeed.Seed(db);
        }
        RefreshAllLists();
    }

    private void RefreshAllLists()
    {
        using var db = new AppDbContext();
        var lessons = db.Lessons.Include(l => l.Student).Include(l => l.Coach).ToList();
        var students = db.Students.ToList();
        var coaches = db.Coaches.ToList();

        if (this.FindControl<ListBox>("LessonsListBox") is { } lb) lb.ItemsSource = lessons;
        if (this.FindControl<ListBox>("StudentsListBox") is { } sb) sb.ItemsSource = students;
        if (this.FindControl<ListBox>("CoachesListBox") is { } cb) cb.ItemsSource = coaches;
        if (this.FindControl<ComboBox>("LessonStudentCombo") is { } sc) sc.ItemsSource = students;
        if (this.FindControl<ComboBox>("LessonCoachCombo") is { } cc) cc.ItemsSource = coaches;
    }

    private void ShowStatus(string name, string msg, bool isError)
    {
        if (this.FindControl<TextBlock>(name) is { } tb)
        {
            tb.Text = msg;
            tb.Foreground = isError ? Brushes.Crimson : Brushes.ForestGreen;
        }
    }

    private void AddLesson_Click(object? sender, RoutedEventArgs e)
    {
        var sc = this.FindControl<ComboBox>("LessonStudentCombo");
        var cc = this.FindControl<ComboBox>("LessonCoachCombo");
        var dp = this.FindControl<CalendarDatePicker>("LessonDatePicker");
        var tp = this.FindControl<TimePicker>("LessonTimePicker");
        var dbx = this.FindControl<TextBox>("LessonDurationBox");
        var pbx = this.FindControl<TextBox>("LessonPriceBox");
        var chk = this.FindControl<CheckBox>("LessonIsPaidCheck");

        if (sc?.SelectedItem is not Student st) { ShowStatus("LessonStatusText", "Выберите ученика.", true); return; }
        if (cc?.SelectedItem is not Coach ch) { ShowStatus("LessonStatusText", "Выберите тренера.", true); return; }
        if (dp?.SelectedDate == null) { ShowStatus("LessonStatusText", "Выберите дату.", true); return; }
        if (!int.TryParse(dbx?.Text, out int dur) || dur <= 0) { ShowStatus("LessonStatusText", "Неверная длительность.", true); return; }
        if (!int.TryParse(pbx?.Text, out int price) || price < 0) { ShowStatus("LessonStatusText", "Неверная цена.", true); return; }

        try
        {
            using var db = new AppDbContext();
            db.Lessons.Add(new Lesson
            {
                StudentId = st.Id,
                CoachId = ch.Id,
                StartTime = dp.SelectedDate.Value.Date + (tp?.SelectedTime ?? TimeSpan.Zero),
                DurationMinutes = dur,
                Price = price,
                IsPaid = chk?.IsChecked ?? false
            });
            db.SaveChanges();
            if (dbx != null) dbx.Text = "";
            if (pbx != null) pbx.Text = "";
            if (chk != null) chk.IsChecked = false;
            RefreshAllLists();
            ShowStatus("LessonStatusText", "Занятие добавлено!", false);
        }
        catch (Exception ex) { ShowStatus("LessonStatusText", ex.InnerException?.Message ?? ex.Message, true); }
    }

    private void DeleteLesson_Click(object? sender, RoutedEventArgs e)
    {
        if (this.FindControl<ListBox>("LessonsListBox")?.SelectedItem is not Lesson sel)
        {
            ShowStatus("LessonStatusText", "Выберите занятие для удаления.", true);
            return;
        }
        try
        {
            using var db = new AppDbContext();
            if (db.Lessons.Find(sel.Id) is { } entity) { db.Lessons.Remove(entity); db.SaveChanges(); }
            RefreshAllLists();
            ShowStatus("LessonStatusText", "Занятие удалено.", false);
        }
        catch (Exception ex) { ShowStatus("LessonStatusText", ex.InnerException?.Message ?? ex.Message, true); }
    }
}
