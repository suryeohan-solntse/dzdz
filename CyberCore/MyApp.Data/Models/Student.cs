namespace MyApp.Data.Models;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public int? CurrentMmr { get; set; } 
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    
    public List<Lesson> Lessons { get; set; } = new();

    public override string ToString()
    {
        string mmrText = CurrentMmr.HasValue ? $"{CurrentMmr} MMR" : "без рейтинга";
        string status = IsActive ? "Активен" : "В архиве";
        return $"[#{Id}] {FullName} (Ник: {Nickname}, {mmrText}) | Тел: {Phone} | {status}";
    }
}
