using ProfilesApp.Models;

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

    private string _availableOptions = string.Join(", ", AllowedOptions);
    private string _value;

    public bool TryProcessInput(string input)
    {
        _value = AllowedOptions.FirstOrDefault(x =>
            string.Equals(x, input, StringComparison.OrdinalIgnoreCase));
        var success = !string.IsNullOrEmpty(_value);

        if (!success)
        {
            Console.WriteLine($"Неверное значение, попробуйте ещё раз (доступные значения: {_availableOptions})");
        }

        return success;
    }

    public void ApplyValue(ProfileModel model)
    {
        model.ProgrammingLanguage = _value;
    }
}