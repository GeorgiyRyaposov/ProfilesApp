using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class GoToPreviousQuestionCommand : ICommand, IHasNameAndDescription
{
    public string Name => "goto_prev_question";
    public string Description => "Вернуться к предыдущему вопросу";
    
    private readonly IProfileBuilder _profileBuilder;
    private readonly IUserInterfaceService _userInterfaceService;

    public GoToPreviousQuestionCommand(IProfileBuilder profileBuilder, 
        IUserInterfaceService userInterfaceService)
    {
        _profileBuilder = profileBuilder;
        _userInterfaceService = userInterfaceService;
    }
    
    public void Execute(params string[] args)
    {
        _profileBuilder.TryGoToPreviousQuestion();
        
        var question = _profileBuilder.GetCurrentQuestion();
        _userInterfaceService.ShowMessage(question);
    }
}