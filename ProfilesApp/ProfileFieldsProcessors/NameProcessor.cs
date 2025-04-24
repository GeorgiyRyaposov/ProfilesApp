using ProfilesApp.Models;
using ProfilesApp.Services;

namespace ProfilesApp.ProfileFieldsProcessors;

public class NameProcessor : IFieldProcessor
{
    public int Order => 1;
    public string Question => "Введите фамилию имя отчество:";

    private readonly IProfilesRepository _profilesRepository;

    private string _value;

    public NameProcessor(IProfilesRepository profilesRepository)
    {
        _profilesRepository = profilesRepository;
    }

    public bool TryProcessInput(string input, out string errorMessage)
    {
        if (_profilesRepository.HasProfile(input))
        {
            errorMessage = "Такой профиль уже существует";
            return false;
        }

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