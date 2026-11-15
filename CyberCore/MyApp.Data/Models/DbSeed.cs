using MyApp.Data.Models;

namespace MyApp.Data;

public static class DbSeed
{
    public static void Seed(AppDbContext db)
    {
        
        if (db.Students.Any() || db.Coaches.Any())
        {
            return;
        }

        var students = new List<Student>
        {
            new() { FullName = "Алексей Смирнов", Nickname = "ShadowStep", CurrentMmr = 4500, Phone = "+79991112233", IsActive = true },
            new() { FullName = "Мария Волкова", Nickname = "Valkyrie", CurrentMmr = 6200, Phone = "+79992223344", IsActive = true },
            new() { FullName = "Дмитрий Козлов", Nickname = "NoobMaster", CurrentMmr = null, Phone = "+79993334455", IsActive = true },
            new() { FullName = "Егор Соколов", Nickname = "FrostByte", CurrentMmr = 3100, Phone = "+79994445566", IsActive = false }
        };

        var coaches = new List<Coach>
        {
            new() { FullName = "Артем Морозов", Discipline = "CS2", HourlyRate = 2500, ExperienceYears = 5 },
            new() { FullName = "Кирилл Новиков", Discipline = "Dota 2", HourlyRate = 3000, ExperienceYears = 7 },
            new() { FullName = "Елена Белова", Discipline = "Valorant", HourlyRate = 2200, ExperienceYears = 3 }
        };

        db.Students.AddRange(students);
        db.Coaches.AddRange(coaches);
        db.SaveChanges();

        var lessons = new List<Lesson>
        {
            new()
            {
                StudentId = students[0].Id,
                CoachId = coaches[0].Id,
                StartTime = DateTime.Now.AddDays(1).Date.AddHours(16),
                DurationMinutes = 60,
                Price = 2500,
                IsPaid = true
            },
            new()
            {
                StudentId = students[1].Id,
                CoachId = coaches[1].Id,
                StartTime = DateTime.Now.AddDays(1).Date.AddHours(18),
                DurationMinutes = 90,
                Price = 4500,
                IsPaid = true
            },
            new()
            {
                StudentId = students[2].Id,
                CoachId = coaches[2].Id,
                StartTime = DateTime.Now.AddDays(2).Date.AddHours(15),
                DurationMinutes = 60,
                Price = 2200,
                IsPaid = false
            },
            new()
            {
                StudentId = students[0].Id,
                CoachId = coaches[2].Id,
                StartTime = DateTime.Now.AddDays(3).Date.AddHours(19),
                DurationMinutes = 120,
                Price = 4400,
                IsPaid = false
            }
        };

        db.Lessons.AddRange(lessons);
        db.SaveChanges();
    }
}
