using System.Globalization;
using ProfilesApp.Models;

namespace ProfilesApp.ProfileFieldsProcessors;

public class DateBirthProcessor : IFieldProcessor
{
    public int Order => 2;
    public string Question => "Введите дату рождения в формате день.месяц.год:";
        
    private DateTime _value;
        
    public bool TryProcessInput(string input, out string errorMessage)
    {
        var parsed = DateTime.TryParseExact(input, "dd.mm.yyyy", 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out _value);

        if (!parsed)
        {
            errorMessage = "Неверный формат даты, попробуйте ещё раз";
        }

        errorMessage = string.Empty;
        return parsed;
    }

    public void ApplyValue(ProfileModel model)
    {
        model.BirthDate = _value;
    }
}