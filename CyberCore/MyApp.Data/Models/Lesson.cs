namespace MyApp.Data.Models;

public class Lesson
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CoachId { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public int Price { get; set; }
    public bool IsPaid { get; set; }

    
    public Student? Student { get; set; }
    public Coach? Coach { get; set; }

    public override string ToString()
    {
        string studentName = Student != null ? $"{Student.FullName} ({Student.Nickname})" : $"ID:{StudentId}";
        string coachName = Coach != null ? $"{Coach.FullName} [{Coach.Discipline}]" : $"ID:{CoachId}";
        string paidStatus = IsPaid ? "Оплачено" : "Не оплачено";

        return $"[#{Id}] {StartTime:dd.MM.yyyy HH:mm} | Ученик: {studentName} -> Тренер: {coachName} | {DurationMinutes} мин. | {Price} руб. ({paidStatus})";
    }
}
