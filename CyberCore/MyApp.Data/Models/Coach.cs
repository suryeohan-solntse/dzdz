namespace MyApp.Data.Models;

public class Coach
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Discipline { get; set; } = string.Empty;
    public int HourlyRate { get; set; }
    public int? ExperienceYears { get; set; } 

    
    public List<Lesson> Lessons { get; set; } = new();

    public override string ToString()
    {
        string exp = ExperienceYears.HasValue ? $"опыт {ExperienceYears} г." : "без опыта";
        return $"[#{Id}] {FullName} — {Discipline} ({HourlyRate} руб/час, {exp})";
    }
}
