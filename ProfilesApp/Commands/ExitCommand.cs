using ProfilesApp.Services;
using ProfilesApp.StateMachine;

namespace ProfilesApp.Commands;

public class ExitCommand : ICommand, IHasNameAndDescription
{
    public string Name => "exit";
    public string Description => "Выйти из приложения";
    
    private readonly IProfileBuilder _profileBuilder;
    private readonly IAppStateMachine _stateMachine;

    public ExitCommand(IAppStateMachine stateMachine, IProfileBuilder profileBuilder)
    {
        _stateMachine = stateMachine;
        _profileBuilder = profileBuilder;
    }
    
    public void Execute(params string[] args)
    {
        if (_profileBuilder.IsProfileCompleted)
        {
            Console.WriteLine("Есть несохраненные изменения, вы уверены что хотите выйти? (д/н)");
            
            var answer = Console.ReadLine();
            if (string.Equals(answer, "н", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
        }
        
        _stateMachine.SetExitState();
    }
}