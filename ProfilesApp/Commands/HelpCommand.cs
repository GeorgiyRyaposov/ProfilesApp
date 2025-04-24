using System.Text;
using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class HelpCommand : ICommand, IHasNameAndDescription
{
    public string Name => "help";
    public string Description => "Показать список доступных команд с описанием";

    private readonly ICommandsService _commandsService;
    private readonly IUserInterfaceService _userInterfaceService;
    private readonly ILocalizationService _localizationService;

    public HelpCommand(ICommandsService commandsService,
        IUserInterfaceService userInterfaceService, ILocalizationService localizationService)
    {
        _commandsService = commandsService;
        _userInterfaceService = userInterfaceService;
        _localizationService = localizationService;
    }
    
    public void Execute(params string[] args)
    {
        var commands = _commandsService.GetActiveCommands();
        
        var sb = new StringBuilder();
        var header = _localizationService.Get("Список доступных команд:");
        sb.AppendLine(header);
        
        foreach (var cmd in commands)
        {
            if (cmd is IHasNameAndDescription namedCmd)
            {
                var description = _localizationService.Get(namedCmd.Description);
                sb.AppendLine($"'-{namedCmd.Name}' {description}");
            }
        }
        
        _userInterfaceService.ShowMessage(sb.ToString());
    }
}