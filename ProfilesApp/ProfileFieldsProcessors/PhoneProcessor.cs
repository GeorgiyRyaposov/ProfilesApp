using System.Text.RegularExpressions;
using ProfilesApp.Models;

namespace ProfilesApp.ProfileFieldsProcessors;

public class PhoneProcessor : IFieldProcessor
{
    public int Order => 5;
    public string Question => "Введите номер телефона:";

    private string _value;

    public bool TryProcessInput(string input)
    {
        // Удаляем все символы, кроме цифр и плюса (если он в начале)
        var normalized = Regex.Replace(input, @"[^\d+]", "");
        _value = normalized;

        // (\+7|8) - начинается на +7 или 8
        // [0-9] - любой символ от 0 до 9
        // {10} — ровно 10 цифр после +7 или 8
        var matched = Regex.IsMatch(normalized, @"^(\+7|8)[0-9]{10}$");

        if (!matched)
        {
            Console.WriteLine("Неверный формат номера телефона, попробуйте ещё раз");
        }

        return matched;
    }

    public void ApplyValue(ProfileModel model)
    {
        model.Phone = _value;
    }
}