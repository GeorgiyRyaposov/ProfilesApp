namespace ProfilesApp.Services;

public interface ILocalizationService
{
    string Get(string key, params object[] args);
}

public class LocalizationService : ILocalizationService
{
    public string Get(string key, params object[] args)
    {
        //todo:
        return string.Format(key, args);
    }
}