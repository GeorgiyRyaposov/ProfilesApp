using Microsoft.Extensions.DependencyInjection;
using ProfilesApp.Models;
using ProfilesApp.ProfileFieldsProcessors;

namespace ProfilesApp.Services;

public interface IProfileBuilder
{
    bool IsProfileCompleted { get; }
    string GetCurrentQuestion();
    bool GoToQuestion(int questionNumber, out string errorMessage);
    void TryGoToPreviousQuestion();
    bool TryProcessAnswer(string answer, out string errorMessage);
    ProfileModel BuildProfile();
    void Reset();
}

/// <summary>
/// Следит за порядком заполнения анкеты.
/// В результате опроса создает профиль <see cref="ProfileModel"/>
/// </summary>
public class ProfileBuilder : IProfileBuilder
{
    public bool IsProfileCompleted { get; private set; }

    private readonly ILocalizationService _localizationService;
    private readonly IFieldProcessor[] _fieldsProcessors;
    private int _currentProcessorIndex;

    public ProfileBuilder(IServiceProvider serviceProvider, ILocalizationService localizationService)
    {
        _localizationService = localizationService;
        _fieldsProcessors = serviceProvider.GetServices<IFieldProcessor>()
            .OrderBy(o => o.Order)
            .ToArray();
    }

    public bool TryProcessAnswer(string answer, out string errorMessage)
    {
        if (string.IsNullOrEmpty(answer))
        {
            errorMessage = "Поле не может быть пустым";
            return false;
        }

        if (_fieldsProcessors[_currentProcessorIndex].TryProcessInput(answer, out errorMessage))
        {
            _currentProcessorIndex++;

            IsProfileCompleted = _currentProcessorIndex >= _fieldsProcessors.Length;

            return true;
        }

        return false;
    }

    public string GetCurrentQuestion()
    {
        var processor = _fieldsProcessors[_currentProcessorIndex];
        var question = _localizationService.Get(processor.Question);
        return $"{processor.Order}. {question}";
    }

    public ProfileModel BuildProfile()
    {
        var model = new ProfileModel
        {
            CreatedAt = DateTime.UtcNow
        };

        foreach (var answerProcessor in _fieldsProcessors)
        {
            answerProcessor.ApplyValue(model);
        }

        return model;
    }

    public void Reset()
    {
        _currentProcessorIndex = 0;
        IsProfileCompleted = false;
    }

    public bool GoToQuestion(int questionNumber, out string errorMessage)
    {
        var index = questionNumber - 1;
        if (index > _currentProcessorIndex)
        {
            errorMessage = "Не заполнены предыдущие поля";
            return false;
        }
        
        SetCurrentProcessor(index);
        
        errorMessage = string.Empty;
        return true;
    }

    public void TryGoToPreviousQuestion()
    {
        SetCurrentProcessor(_currentProcessorIndex - 1);
    }
    
    private void SetCurrentProcessor(int index)
    {
        if (index < 0)
        {
            index = 0;
        }

        if (index >= _fieldsProcessors.Length)
        {
            index = _fieldsProcessors.Length - 1;
        }

        _currentProcessorIndex = index;
    }
}