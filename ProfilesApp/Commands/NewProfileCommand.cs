using ProfilesApp.StateMachine;

namespace ProfilesApp.Commands;

public class NewProfileCommand : ICommand, IHasNameAndDescription
{
    public string Name => "new_profile";
    public string Description => "Заполнить новую анкету";
    
    private readonly IAppStateMachine _stateMachine;

    public NewProfileCommand(IAppStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    
    public void Execute(params string[] args)
    {
        _stateMachine.SetProfileCreationState();
    }
}