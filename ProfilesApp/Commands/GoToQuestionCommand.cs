using System.Globalization;
using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class GoToQuestionCommand : ICommand, IHasNameAndDescription
{
    public string Name => "goto_question";
    public string Description => "Вернуться к указанному вопросу (-goto_question <Номер вопроса>)";
    
    private readonly IProfileBuilder _profileBuilder;

    public GoToQuestionCommand(IProfileBuilder profileBuilder)
    {
        _profileBuilder = profileBuilder;
    }
    
    public void Execute(params string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Укажите номер вопроса");
            return;
        }
        
        if (int.TryParse(args[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var questionNumber))
        {
            _profileBuilder.GoToQuestion(questionNumber);
            _profileBuilder.ShowCurrentQuestion();
        }
        else
        {
            Console.WriteLine("Неверное значение, попробуйте ещё раз");
        }
    }
}