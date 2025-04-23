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

    public ProfileInputCommand(IProfileBuilder profileBuilder, IAppStateMachine appStateMachine)
    {
        _profileBuilder = profileBuilder;
        _appStateMachine = appStateMachine;
    }
    
    public void Execute(params string[] args)
    {
        if (args.Length == 0)
        {
            return;
        }
        
        var success = _profileBuilder.TryProcessAnswer(args[0]);
        if (!success)
        {
            return;
        }
        
        if (_profileBuilder.IsProfileCompleted)
        {
            _appStateMachine.SetInitialState();
        }
        else
        {
            _profileBuilder.ShowCurrentQuestion();
        }
    }
}