using System.Globalization;
using System.Text;
using ProfilesApp.Models;

namespace ProfilesApp.Services;

public interface IProfilesRepository
{
    void Save(ProfileModel profile);
    ProfileModel[] GetAll();
    string FindProfilePath(string fileName);
    bool HasProfile(string fileName);
    bool DeleteProfile(string fileName);
    string[] GetAllFiles();
    string[] GetAllFiles(DateTime atDate);
}

/// <summary>
/// Содержит логику по работе с файлами анкет: сохранение, загрузка, поиск, удаление
/// </summary>
public class ProfilesRepository : IProfilesRepository
{
    #region Consts

    private const string Extension = "txt";
    private const string BirthDateFormat = "dd.mm.yyyy";
    private const string CreationDateFormat = "dd.MM.yyyy HH:mm:ss";

    private const string FullNameProperty = "ФИО";
    private const string DateBirthProperty = "Дата рождения";
    private const string ProgrammingLanguageProperty = "Любимый язык программирования";
    private const string ExperienceYearsProperty = "Опыт программирования на указанном языке";
    private const string PhoneProperty = "Мобильный телефон";
    private const string CreatedAtProperty = "Анкета заполнена";

    #endregion //Consts
    
    private readonly string _directory = Path.Combine(Directory.GetCurrentDirectory(), "Анкеты");

    private readonly IUserInterfaceService _userInterfaceService;

    public ProfilesRepository(IUserInterfaceService userInterfaceService)
    {
        _userInterfaceService = userInterfaceService;
    }

    public void Save(ProfileModel profile)
    {
        Directory.CreateDirectory(_directory);

        var content = new StringBuilder();
        content.AppendLine($"{FullNameProperty}: {profile.FullName}");
        content.AppendLine($"{DateBirthProperty}: {profile.BirthDate.ToString(BirthDateFormat)}");
        content.AppendLine($"{ProgrammingLanguageProperty}: {profile.ProgrammingLanguage}");
        content.AppendLine($"{ExperienceYearsProperty}: {profile.ExperienceYears}");
        content.AppendLine($"{PhoneProperty}: {profile.Phone}");
        content.AppendLine($"{CreatedAtProperty}: {profile.CreatedAt.ToString(CreationDateFormat)}");

        var filePath = Path.Combine(_directory, $"{profile.FullName}.{Extension}");
        File.WriteAllText(filePath, content.ToString(), Encoding.Unicode);

        _userInterfaceService.ShowMessage("Анкета сохранена в {0}", filePath);
    }

    public ProfileModel[] GetAll()
    {
        if (!Directory.Exists(_directory))
        {
            return Array.Empty<ProfileModel>();
        }

        return Directory.GetFiles(_directory, $"*.{Extension}")
            .Select(ParseProfile)
            .ToArray();
    }

    public string FindProfilePath(string fileName)
    {
        if (!Directory.Exists(_directory))
        {
            return string.Empty;
        }

        fileName = Path.ChangeExtension(fileName, Extension);
        var entries = Directory.EnumerateFiles(_directory);
        foreach (var entry in entries)
        {
            var entryName = Path.GetFileName(entry);
            if (string.Equals(entryName, fileName))
            {
                return entry;
            }
        }

        return string.Empty;
    }

    public bool HasProfile(string fileName)
    {
        return !string.IsNullOrEmpty(FindProfilePath(fileName));
    }

    public bool DeleteProfile(string fileName)
    {
        var path = FindProfilePath(fileName);
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        File.Delete(path);
        return true;
    }

    public string[] GetAllFiles()
    {
        if (!Directory.Exists(_directory))
        {
            return Array.Empty<string>();
        }

        return Directory.GetFiles(_directory, $"*.{Extension}");
    }

    public string[] GetAllFiles(DateTime atDate)
    {
        var files = GetAllFiles();
        if (files.Length == 0)
        {
            return Array.Empty<string>();
        }

        var date = new DateTime(atDate.Year, atDate.Month, atDate.Day);
        
        return files
            .Where(x =>
            {
                var profile = ParseProfile(x);
                return profile.CreatedAt >= date && profile.CreatedAt < date.AddDays(1);
            })
            .ToArray();
    }

    private static ProfileModel ParseProfile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Файл анкеты не найден.");
        }

        var lines = File.ReadAllLines(filePath);
        var model = new ProfileModel();

        foreach (var line in lines)
        {
            var parts = line.Split(new[] { ": " }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                continue;
            }

            var key = parts[0];
            var value = parts[1];

            switch (key)
            {
                case FullNameProperty:
                    model.FullName = value;
                    break;

                case DateBirthProperty:
                    ParseDateTime(value, BirthDateFormat, out var birthDate);
                    model.BirthDate = birthDate;
                    break;

                case ProgrammingLanguageProperty:
                    model.ProgrammingLanguage = value;
                    break;

                case ExperienceYearsProperty:
                    if (int.TryParse(value, out var exp))
                    {
                        model.ExperienceYears = exp;
                    }

                    break;

                case PhoneProperty:
                    model.Phone = value;
                    break;

                case CreatedAtProperty:
                    ParseDateTime(value, CreationDateFormat, out var createdAt);
                    model.CreatedAt = createdAt;
                    break;
            }
        }

        return model;
    }

    private static void ParseDateTime(string value, string format, out DateTime result)
    {
        if (!DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
        {
            throw new FormatException($"Неверный формат даты: {value}");
        }
    }
}