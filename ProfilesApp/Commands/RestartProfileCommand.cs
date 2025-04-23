using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class RestartProfileCommand: ICommand, IHasNameAndDescription
{
    public string Name => "restart_profile";
    public string Description => "Заполнить анкету заново";
    
    private readonly IProfileBuilder _profileBuilder;

    public RestartProfileCommand(IProfileBuilder profileBuilder)
    {
        _profileBuilder = profileBuilder;
    }
    
    public void Execute(params string[] args)
    {
        _profileBuilder.Reset();
        _profileBuilder.ShowCurrentQuestion();
    }
}