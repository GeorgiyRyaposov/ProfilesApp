using System.Text;
using ProfilesApp.Models;

namespace ProfilesApp.Services;

public interface IStatisticsService
{
    string GetOverallStatistics(ProfileModel[] models);
}

public class StatisticsService : IStatisticsService
{
    private readonly ILocalizationService _localizationService;

    public StatisticsService(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public string GetOverallStatistics(ProfileModel[] models)
    {
        if (models == null || !models.Any())
        {
            return string.Empty;
        }
        
        var age = CalculateAverageAge(models);
        var ageForm = _localizationService.Get(GetAgeForm(age));

        var statistics = new StringBuilder();
        
        var ageStats = _localizationService.Get("Средний возраст всех опрошенных: {0} {1}", age, ageForm);
        var languageStats = _localizationService.Get("Самый популярный язык программирования: {0}", GetPopularLanguage(models));
        var experienceStats = _localizationService.Get("Самый опытный программист: {0}", GetMostExperienced(models));

        statistics.AppendLine(ageStats);
        statistics.AppendLine(languageStats);
        statistics.AppendLine(experienceStats);
        
        return statistics.ToString();
    }

    private int CalculateAverageAge(ProfileModel[] models)
    {
        var today = DateTime.Today;
        var totalAge = 0;

        foreach (var profile in models)
        {
            var age = today.Year - profile.BirthDate.Year;
            
            // Корректировка возраста, если день рождения еще не наступил в этом году
            if (profile.BirthDate.Date < today)
            {
                age--;
            }

            totalAge += age;
        }

        return totalAge / models.Length;
    }
    private string GetAgeForm(int age)
    {
        // Получаем последнюю цифру возраста
        var lastDigit = age % 10;
        
        // Получаем последние 2 цифры (для исключений 11-14)
        var lastTwoDigits = age % 100;
        if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
        {
            return "лет";
        }
        
        return lastDigit switch
        {
            1 => "год",
            2 or 3 or 4 => "года",
            _ => "лет"
        };
    }

    private string GetPopularLanguage(ProfileModel[] models)
    {
        return models.Select(x => x.ProgrammingLanguage)
            .GroupBy(p => p)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
    }

    private string GetMostExperienced(ProfileModel[] models)
    {
        var mostExperience = models.Max(x => x.ExperienceYears);
        return models.First(x => x.ExperienceYears == mostExperience).FullName;
    }
}