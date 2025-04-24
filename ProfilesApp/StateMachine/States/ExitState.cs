using ProfilesApp.Services;

namespace ProfilesApp.StateMachine.States;

public class ExitState : IState
{
    private readonly IUserInterfaceService _userInterfaceService;

    public ExitState(IUserInterfaceService userInterfaceService)
    {
        _userInterfaceService = userInterfaceService;
    }

    public void Enter()
    {
        _userInterfaceService.ShowMessage("Нажмите любую кнопку для выхода..");
        _userInterfaceService.ReadKeyInput();
    }
}