using System.Text;
using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class HelpCommand : ICommand, IHasNameAndDescription
{
    public string Name => "help";
    public string Description => "Показать список доступных команд с описанием";

    private readonly ICommandsService _commandsService;

    public HelpCommand(ICommandsService commandsService)
    {
        _commandsService = commandsService;
    }
    
    public void Execute(params string[] args)
    {
        var commands = _commandsService.GetActiveCommands();
        
        var sb = new StringBuilder();
        sb.AppendLine("Список доступных команд:");
        
        foreach (var cmd in commands)
        {
            if (cmd is IHasNameAndDescription namedCmd)
            {
                sb.AppendLine($"'-{namedCmd.Name}' {namedCmd.Description}");
            }
        }
        
        Console.Write(sb.ToString());
    }
}