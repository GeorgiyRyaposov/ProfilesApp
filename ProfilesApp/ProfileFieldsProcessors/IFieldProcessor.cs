using ProfilesApp.Models;

namespace ProfilesApp.ProfileFieldsProcessors;

public interface IFieldProcessor
{
    int Order { get; }
    string Question { get; }
    bool TryProcessInput(string input);
    void ApplyValue(ProfileModel model);
}