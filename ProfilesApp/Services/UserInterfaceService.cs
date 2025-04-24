namespace ProfilesApp.Services;

public interface IUserInterfaceService
{
    void ShowMessage(string key, params object[] args);
    string ReadLineInput();
    char ReadKeyInput();
}

/// <summary>
/// Инкапсулирует логику работы с пользователем
/// </summary>
public class UserInterfaceService : IUserInterfaceService
{
    private readonly ILocalizationService _localizationService;

    public UserInterfaceService(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public void ShowMessage(string key, params object[] args)
    {
        var message = _localizationService.Get(key, args);
        Console.WriteLine(message);
    }
    
    public string ReadLineInput()
    {
        return Console.ReadLine();
    }

    public char ReadKeyInput()
    {
        var key = Console.ReadKey();
        return key.KeyChar;
    }
}