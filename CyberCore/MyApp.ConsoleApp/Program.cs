using System.Text;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;


Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine($"Путь к файлу базы данных: {AppDbContext.DbPath}");
Console.WriteLine(new string('-', 60));

using var db = new AppDbContext();


db.Database.EnsureCreated();
DbSeed.Seed(db);

Console.WriteLine("\n=== УЧЕНИКИ (Students) ===");
foreach (var student in db.Students.ToList())
{
    Console.WriteLine(student);
}

Console.WriteLine("\n=== ТРЕНЕРЫ (Coaches) ===");
foreach (var coach in db.Coaches.ToList())
{
    Console.WriteLine(coach);
}

Console.WriteLine("\n=== ЗАНЯТИЯ (Lessons) ===");

var lessons = db.Lessons
    .Include(l => l.Student)
    .Include(l => l.Coach)
    .ToList();

foreach (var lesson in lessons)
{
    Console.WriteLine(lesson);
}
