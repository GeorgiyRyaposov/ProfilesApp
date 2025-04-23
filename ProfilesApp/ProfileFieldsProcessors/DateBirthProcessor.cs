using System.Globalization;
using ProfilesApp.Models;

namespace ProfilesApp.ProfileFieldsProcessors;

public class DateBirthProcessor : IFieldProcessor
{
    public int Order => 2;
    public string Question => "Введите дату рождения в формате день.месяц.год:";
        
    private DateTime _value;
        
    public bool TryProcessInput(string input)
    {
        var parsed = DateTime.TryParseExact(input, "dd.mm.yyyy", 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out _value);

        if (!parsed)
        {
            Console.WriteLine("Неверный формат даты, попробуйте ещё раз");
        }
        
        return parsed;
    }

    public void ApplyValue(ProfileModel model)
    {
        model.BirthDate = _value;
    }
}