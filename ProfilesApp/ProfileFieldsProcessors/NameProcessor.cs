using ProfilesApp.Models;

namespace ProfilesApp.ProfileFieldsProcessors;

public class NameProcessor : IFieldProcessor
{
    public int Order => 1;
    public string Question => "Введите фамилию имя отчество:";
        
    private string _value;

    public bool TryProcessInput(string input, out string errorMessage)
    {
        var parts = input.Split();

        if (parts.Length == 3)
        {
            _value = input;
            errorMessage = string.Empty;
            return true;
        }
            
        errorMessage = "ФИО введены не полностью";
        return false;
    }

    public void ApplyValue(ProfileModel model)
    {
        model.FullName = _value;
    }
}