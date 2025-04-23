using Microsoft.Extensions.DependencyInjection;
using ProfilesApp.Models;
using ProfilesApp.ProfileFieldsProcessors;

namespace ProfilesApp.Services;

public interface IProfileBuilder
{
    bool IsProfileCompleted { get; }
    void ShowCurrentQuestion();
    void GoToQuestion(int questionNumber);
    void GoToPreviousQuestion();
    bool TryProcessAnswer(string answer);
    ProfileModel BuildProfile();
    void Reset();
}

public class ProfileBuilder : IProfileBuilder
{
    public bool IsProfileCompleted { get; private set; }

    private readonly IFieldProcessor[] _fieldsProcessors;
    private int _currentProcessorIndex;

    public ProfileBuilder(IServiceProvider serviceProvider)
    {
        _fieldsProcessors = serviceProvider.GetServices<IFieldProcessor>()
            .OrderBy(o => o.Order)
            .ToArray();
    }

    public void ShowCurrentQuestion()
    {
        Console.WriteLine(GetCurrentQuestion());
    }

    public bool TryProcessAnswer(string answer)
    {
        if (string.IsNullOrEmpty(answer))
        {
            return false;
        }

        if (_fieldsProcessors[_currentProcessorIndex].TryProcessInput(answer))
        {
            _currentProcessorIndex++;

            IsProfileCompleted = _currentProcessorIndex >= _fieldsProcessors.Length;

            return true;
        }

        return false;
    }

    private string GetCurrentQuestion()
    {
        var processor = _fieldsProcessors[_currentProcessorIndex];
        return $"{processor.Order}. {processor.Question}";
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

    public void GoToQuestion(int questionNumber)
    {
        var index = questionNumber - 1;
        if (index > _currentProcessorIndex)
        {
            Console.WriteLine("Не заполнены предыдущие поля");
            return;
        }
        
        SetCurrentProcessor(index);
    }

    public void GoToPreviousQuestion()
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