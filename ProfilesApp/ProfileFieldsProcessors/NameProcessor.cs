using ProfilesApp.Models;

namespace ProfilesApp.ProfileFieldsProcessors;

public class NameProcessor : IFieldProcessor
{
    public int Order => 1;
    public string Question => "Введите фамилию имя отчество:";
        
    private string _value;

    public bool TryProcessInput(string input)
    {
        var parts = input.Split();

        if (parts.Length == 3)
        {
            _value = input;
            return true;
        }
            
        Console.WriteLine("ФИО введены не полностью");
        return false;
    }

    public void ApplyValue(ProfileModel model)
    {
        model.FullName = _value;
    }
}