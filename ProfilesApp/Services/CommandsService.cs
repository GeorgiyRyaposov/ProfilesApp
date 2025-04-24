using System.Text;
using ProfilesApp.Commands;

namespace ProfilesApp.Services;

public interface ICommandsService
{
    void Register(ICommand command);
    void Unregister(ICommand command);
    void SetFallbackCommand(ICommand command);
    bool TryExecute(string input);
    void Clear();
    ICommand[] GetActiveCommands();
}

/// <summary>
/// Содержит список активных команд, выполняет команду по имени
/// Если команда не найдена - выполняет fallback команду
/// </summary>
public class CommandsService : ICommandsService
{
    private readonly HashSet<ICommand> _commands = new();
    private ICommand _fallbackCommand;

    public void Register(ICommand command)
    {
        _commands.Add(command);
    }
    
    public void Unregister(ICommand command)
    {
        _commands.Remove(command);
    }

    public void SetFallbackCommand(ICommand command)
    {
        _fallbackCommand = command;
    }

    public bool TryExecute(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }
        
        var inputs = SplitCommandLine(input);
        var commandName = inputs[0].Trim('-');
        if (TryGetCommand(commandName, out var command))
        {
            var args = inputs.Skip(1).ToArray();
            command.Execute(args);
            
            return true;
        }

        if (_fallbackCommand != null)
        {
            _fallbackCommand.Execute(input);
            return true;
        }
        
        return false;
    }

    public void Clear()
    {
        _fallbackCommand = null;
        _commands.Clear();
    }

    public ICommand[] GetActiveCommands()
    {
        return _commands.ToArray();
    }

    private bool TryGetCommand(string commandName, out ICommand command)
    {
        foreach (var cmd in _commands)
        {
            if (cmd is not IHasNameAndDescription namedCmd)
            {
                continue;
            }
            
            if (string.Equals(namedCmd.Name, commandName, StringComparison.OrdinalIgnoreCase))
            {
                command = cmd;
                return true;
            }
        }

        command = null;
        return false;
    }
    
    private static string[] SplitCommandLine(string input)
    {
        var args = new List<string>();
        var currentArg = new StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];
            if (c == '"')
            {
                if (inQuotes && i + 1 < input.Length && input[i + 1] == '"')
                {
                    // Экранированная кавычка ("")
                    currentArg.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (char.IsWhiteSpace(c) && !inQuotes)
            {
                if (currentArg.Length > 0)
                {
                    args.Add(currentArg.ToString());
                    currentArg.Clear();
                }
            }
            else
            {
                currentArg.Append(c);
            }
        }

        if (currentArg.Length > 0)
        {
            args.Add(currentArg.ToString());
        }

        return args.ToArray();
    }
}