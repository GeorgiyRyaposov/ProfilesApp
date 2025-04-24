using ProfilesApp.Services;
using ProfilesApp.StateMachine;

namespace ProfilesApp.Commands;

/// <summary>
/// Передает ввод пользователя в <see cref="IProfileBuilder"/>
/// и выводит следующий вопрос.
/// Переводит в начальное состояние при завершении опроса
/// </summary>
public class ProfileInputCommand : ICommand
{
    private readonly IProfileBuilder _profileBuilder;
    private readonly IAppStateMachine _appStateMachine;
    private readonly IUserInterfaceService _userInterfaceService;

    public ProfileInputCommand(IProfileBuilder profileBuilder, 
        IAppStateMachine appStateMachine, IUserInterfaceService userInterfaceService)
    {
        _profileBuilder = profileBuilder;
        _appStateMachine = appStateMachine;
        _userInterfaceService = userInterfaceService;
    }
    
    public void Execute(params string[] args)
    {
        if (args.Length == 0)
        {
            return;
        }
        
        var success = _profileBuilder.TryProcessAnswer(args[0], out var errorMessage);
        if (!success)
        {
            _userInterfaceService.ShowMessage(errorMessage);
            return;
        }
        
        if (_profileBuilder.IsProfileCompleted)
        {
            _appStateMachine.SetInitialState();
        }
        else
        {
            var question = _profileBuilder.GetCurrentQuestion();
            _userInterfaceService.ShowMessage(question);
        }
    }
}