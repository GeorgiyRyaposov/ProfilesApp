using Microsoft.Extensions.DependencyInjection;
using ProfilesApp.Commands;
using ProfilesApp.Services;

namespace ProfilesApp.StateMachine.States;

/// <summary>
/// Состояние создания профиля.
/// Ограничивает количество команд необходимыми для этого состояния
/// </summary>
public class ProfileCreationState : IState
{
    private readonly ICommandsService _commandsService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IProfileBuilder _profileBuilder;
    private readonly IUserInterfaceService _userInterfaceService;

    public ProfileCreationState(ICommandsService commandsService, 
        IServiceProvider serviceProvider,
        IProfileBuilder profileBuilder, 
        IUserInterfaceService userInterfaceService)
    {
        _commandsService = commandsService;
        _serviceProvider = serviceProvider;
        _profileBuilder = profileBuilder;
        _userInterfaceService = userInterfaceService;
    }
    
    public void Enter()
    {
        RegisterCommands();
        
        _profileBuilder.Reset();
        var question = _profileBuilder.GetCurrentQuestion();
        _userInterfaceService.ShowMessage(question);
    }

    private void RegisterCommands()
    {
        _commandsService.Clear();
        
        RegisterCommand<HelpCommand>();
        RegisterCommand<ExitCommand>();
        RegisterCommand<GoToPreviousQuestionCommand>();
        RegisterCommand<GoToQuestionCommand>();
        RegisterCommand<RestartProfileCommand>();

        var profileInputCommand = _serviceProvider.GetService<ProfileInputCommand>();
        _commandsService.SetFallbackCommand(profileInputCommand);
    }
    
    private void RegisterCommand<T>() where T : ICommand
    {
        var cmd = _serviceProvider.GetService<T>();
        if (cmd == null)
        {
            Console.Error.WriteLine($"Не найдена команда типа {typeof(T).Name}");
            return;
        }
        
        _commandsService.Register(cmd);
    }
}