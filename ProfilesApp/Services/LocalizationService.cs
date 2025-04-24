namespace ProfilesApp.Services;

public interface ILocalizationService
{
    string Get(string key, params object[] args);
}

/// <summary>
/// Сервис локализации, в котором по ключу можно получить строку с переводом
/// </summary>
public class LocalizationService : ILocalizationService
{
    public string Get(string key, params object[] args)
    {
        //todo:
        return string.Format(key, args);
    }
}