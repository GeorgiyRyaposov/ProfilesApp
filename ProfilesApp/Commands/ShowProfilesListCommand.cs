using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class ShowProfilesListCommand : ICommand, IHasNameAndDescription
{
    public string Name => "list";
    public string Description => "Показать список названий файлов всех сохранённых анкет";
    
    private readonly IProfilesRepository _profilesRepository;

    public ShowProfilesListCommand(IProfilesRepository profilesRepository)
    {
        _profilesRepository = profilesRepository;
    }
    
    public void Execute(params string[] args)
    {
        var files = _profilesRepository.GetAllFiles();
        if (files.Length == 0)
        {
            Console.WriteLine("Список анкет пуст");
            return;
        }
        
        foreach (var file in files)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            Console.WriteLine(name);
        }
    }
}