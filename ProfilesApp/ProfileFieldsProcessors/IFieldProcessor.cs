using ProfilesApp.Models;

namespace ProfilesApp.ProfileFieldsProcessors;

/// <summary>
/// Вспомогательный инструмент для обработки полей профиля.
/// Содержит описание требований к полю, а также методы для обработки ввода
/// </summary>
public interface IFieldProcessor
{
    int Order { get; }
    string Question { get; }
    bool TryProcessInput(string input, out string errorMessage);
    void ApplyValue(ProfileModel model);
}