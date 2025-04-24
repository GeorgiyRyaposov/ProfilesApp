using Microsoft.Extensions.DependencyInjection;
using ProfilesApp.Commands;
using ProfilesApp.Services;

namespace ProfilesApp.StateMachine.States;

public class InitialState : IState
{
    private readonly ICommandsService _commandsService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserInterfaceService _userInterfaceService;

    public InitialState(ICommandsService commandsService, IServiceProvider serviceProvider, 
        IUserInterfaceService userInterfaceService)
    {
        _commandsService = commandsService;
        _serviceProvider = serviceProvider;
        _userInterfaceService = userInterfaceService;
    }
    
    public void Enter()
    {
        RegisterCommands();

        _userInterfaceService.ShowMessage("Выберите действие:");
    }

    private void RegisterCommands()
    {
        _commandsService.Clear();
        
        RegisterCommand<NewProfileCommand>();
        RegisterCommand<SaveProfileCommand>();
        RegisterCommand<ShowStatisticsCommand>();
        RegisterCommand<FindProfileCommand>();
        RegisterCommand<DeleteFileCommand>();
        RegisterCommand<ShowProfilesListCommand>();
        RegisterCommand<ShowTodayProfilesListCommand>();
        RegisterCommand<ZipProfileCommand>();
        RegisterCommand<HelpCommand>();
        RegisterCommand<ExitCommand>();
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