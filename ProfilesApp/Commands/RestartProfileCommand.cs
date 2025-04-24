using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class RestartProfileCommand: ICommand, IHasNameAndDescription
{
    public string Name => "restart_profile";
    public string Description => "Заполнить анкету заново";
    
    private readonly IProfileBuilder _profileBuilder;
    private readonly IUserInterfaceService _userInterfaceService;

    public RestartProfileCommand(IProfileBuilder profileBuilder, IUserInterfaceService userInterfaceService)
    {
        _profileBuilder = profileBuilder;
        _userInterfaceService = userInterfaceService;
    }
    
    public void Execute(params string[] args)
    {
        _profileBuilder.Reset();
        var question = _profileBuilder.GetCurrentQuestion();
        _userInterfaceService.ShowMessage(question);
    }
}