using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;

namespace CyberCore.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        AvaloniaXamlLoader.Load(this);
        LoadData();
    }

    private void LoadData()
    {
        using var db = new AppDbContext();
        db.Database.EnsureCreated();
        DbSeed.Seed(db);

        var lessonsBox = this.FindControl<ListBox>("LessonsListBox");
        var studentsBox = this.FindControl<ListBox>("StudentsListBox");
        var coachesBox = this.FindControl<ListBox>("CoachesListBox");

        if (lessonsBox != null)
        {
            lessonsBox.ItemsSource = db.Lessons
                .Include(l => l.Student)
                .Include(l => l.Coach)
                .ToList();
        }

        if (studentsBox != null)
            studentsBox.ItemsSource = db.Students.ToList();

        if (coachesBox != null)
            coachesBox.ItemsSource = db.Coaches.ToList();
    }
}
