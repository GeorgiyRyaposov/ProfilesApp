using ProfilesApp.Models;

namespace ProfilesApp.Services;

public interface IStatisticsService
{
    void Show(ProfileModel[] models);
}

public class StatisticsService : IStatisticsService
{
    public void Show(ProfileModel[] models)
    {
        if (models == null || !models.Any())
        {
            Console.WriteLine("Нет данных о профилях");
            return;
        }
        
        var age = CalculateAverageAge(models);
        var ageForm = GetAgeForm(age);
        
        Console.WriteLine($"Средний возраст всех опрошенных: {age} {ageForm}");
        Console.WriteLine($"Самый популярный язык программирования: {GetPopularLanguage(models)}");
        Console.WriteLine($"Самый опытный программист: {GetMostExperienced(models)}");
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
    private static string GetAgeForm(int age)
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