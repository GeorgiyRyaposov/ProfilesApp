namespace ProfilesApp.Models;

public class ProfileModel
{
    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public string ProgrammingLanguage { get; set; }
    public int ExperienceYears { get; set; }
    public string Phone { get; set; }
    
    public DateTime CreatedAt { get; set; }
}