namespace ProfilesApp.Commands;

public interface ICommand
{
    void Execute(params string[] args);
}

public interface IHasNameAndDescription
{
    string Name { get; }
    string Description { get; }
}