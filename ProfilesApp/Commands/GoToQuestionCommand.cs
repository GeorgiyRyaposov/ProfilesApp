using System.Globalization;
using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class GoToQuestionCommand : ICommand, IHasNameAndDescription
{
    public string Name => "goto_question";
    public string Description => "Вернуться к указанному вопросу (-goto_question <Номер вопроса>)";
    
    private readonly IProfileBuilder _profileBuilder;
    private readonly IUserInterfaceService _userInterfaceService;

    public GoToQuestionCommand(IProfileBuilder profileBuilder, IUserInterfaceService userInterfaceService)
    {
        _profileBuilder = profileBuilder;
        _userInterfaceService = userInterfaceService;
    }
    
    public void Execute(params string[] args)
    {
        if (args.Length == 0)
        {
            _userInterfaceService.ShowMessage("Укажите номер вопроса");
            return;
        }
        
        if (int.TryParse(args[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var questionNumber))
        {
            var success = _profileBuilder.GoToQuestion(questionNumber, out var errorMessage);
            if (!success)
            {
                _userInterfaceService.ShowMessage(errorMessage);                
            }
            
            var question = _profileBuilder.GetCurrentQuestion();
            _userInterfaceService.ShowMessage(question);
        }
        else
        {
            _userInterfaceService.ShowMessage("Неверное значение, попробуйте ещё раз");
        }
    }
}