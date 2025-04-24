using ProfilesApp.Services;
using ProfilesApp.StateMachine;

namespace ProfilesApp.Commands;

public class ExitCommand : ICommand, IHasNameAndDescription
{
    public string Name => "exit";
    public string Description => "Выйти из приложения";
    
    private readonly IProfileBuilder _profileBuilder;
    private readonly IAppStateMachine _stateMachine;
    private readonly IUserInterfaceService _userInterfaceService;

    public ExitCommand(IAppStateMachine stateMachine, IProfileBuilder profileBuilder, 
        IUserInterfaceService userInterfaceService)
    {
        _stateMachine = stateMachine;
        _profileBuilder = profileBuilder;
        _userInterfaceService = userInterfaceService;
    }
    
    public void Execute(params string[] args)
    {
        if (_profileBuilder.IsProfileCompleted)
        {
            _userInterfaceService.ShowMessage("Есть несохраненные изменения, вы уверены что хотите выйти? (д/н)");
            
            var answer = _userInterfaceService.ReadLineInput();
            if (string.Equals(answer, "н", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
        }
        
        _stateMachine.SetExitState();
    }
}