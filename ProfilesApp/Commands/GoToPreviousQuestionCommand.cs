using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class GoToPreviousQuestionCommand : ICommand, IHasNameAndDescription
{
    public string Name => "goto_prev_question";
    public string Description => "Вернуться к предыдущему вопросу";
    
    private readonly IProfileBuilder _profileBuilder;

    public GoToPreviousQuestionCommand(IProfileBuilder profileBuilder)
    {
        _profileBuilder = profileBuilder;
    }
    
    public void Execute(params string[] args)
    {
        _profileBuilder.GoToPreviousQuestion();
        _profileBuilder.ShowCurrentQuestion();
    }
}