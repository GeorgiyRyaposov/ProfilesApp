using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class FindProfileCommand : ICommand, IHasNameAndDescription
{
    public string Name => "find";
    public string Description => "<Имя файла анкеты> - Найти анкету";
    
    private readonly IProfilesRepository _profilesRepository;

    public FindProfileCommand(IProfilesRepository profilesRepository)
    {
        _profilesRepository = profilesRepository;
    }
    
    public void Execute(params string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Укажите ФИО");
            return;
        }
        
        var profilePath = _profilesRepository.FindProfilePath(args[0]);
        if (string.IsNullOrEmpty(profilePath))
        {
            Console.WriteLine("Анкета не найдена");
            return;
        }

        var text = File.ReadAllText(profilePath);
        Console.WriteLine(text);
    }
}