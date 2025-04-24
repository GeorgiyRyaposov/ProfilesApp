using ProfilesApp.Models;

namespace ProfilesApp.ProfileFieldsProcessors;

public interface IFieldProcessor
{
    int Order { get; }
    string Question { get; }
    bool TryProcessInput(string input, out string errorMessage);
    void ApplyValue(ProfileModel model);
}