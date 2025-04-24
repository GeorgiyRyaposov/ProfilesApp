using ProfilesApp.Models;
using ProfilesApp.Services;

namespace ProfilesApp.ProfileFieldsProcessors;

public class ProgrammingLanguageProcessor : IFieldProcessor
{
    public int Order => 3;
    public string Question => $"Введите любимый язык программирования ({_availableOptions}):";

    private static readonly string[] AllowedOptions =
    {
        "PHP",
        "JavaScript",
        "C",
        "C++",
        "Java",
        "C#",
        "Python",
        "Ruby",
    };
    
    
    private readonly ILocalizationService _localizationService;

    private string _availableOptions = string.Join(", ", AllowedOptions);
    private string _value;

    public ProgrammingLanguageProcessor(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public bool TryProcessInput(string input, out string errorMessage)
    {
        _value = AllowedOptions.FirstOrDefault(x =>
            string.Equals(x, input, StringComparison.OrdinalIgnoreCase));
        var success = !string.IsNullOrEmpty(_value);

        errorMessage = success
            ? string.Empty
            : _localizationService.Get("Неверное значение, попробуйте ещё раз (доступные значения: {0})",
                _availableOptions);
        
        return success;
    }

    public void ApplyValue(ProfileModel model)
    {
        model.ProgrammingLanguage = _value;
    }
}