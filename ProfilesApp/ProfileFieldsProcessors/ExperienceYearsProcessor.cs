using System.Globalization;
using ProfilesApp.Models;

namespace ProfilesApp.ProfileFieldsProcessors;

public class ExperienceYearsProcessor : IFieldProcessor
{
    public int Order => 4;
    public string Question => "Опыт программирования на указанном языке (Полных лет):";

    private int _value;

    public bool TryProcessInput(string input)
    {
        if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out _value))
        {
            Console.WriteLine("Неверное значение, попробуйте ещё раз");
            return false;
        }

        if (_value < 0)
        {
            Console.WriteLine("Не может быть отрицательным, попробуйте ещё раз");
            return false;
        }

        return true;
    }

    public void ApplyValue(ProfileModel model)
    {
        model.ExperienceYears = _value;
    }
}