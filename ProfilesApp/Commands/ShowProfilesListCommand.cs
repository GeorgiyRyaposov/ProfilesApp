using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class ShowProfilesListCommand : ICommand, IHasNameAndDescription
{
    public string Name => "list";
    public string Description => "Показать список названий файлов всех сохранённых анкет";
    
    private readonly IProfilesRepository _profilesRepository;
    private readonly IUserInterfaceService _userInterfaceService;

    public ShowProfilesListCommand(IProfilesRepository profilesRepository, 
        IUserInterfaceService userInterfaceService)
    {
        _profilesRepository = profilesRepository;
        _userInterfaceService = userInterfaceService;
    }
    
    public void Execute(params string[] args)
    {
        var files = _profilesRepository.GetAllFiles();
        if (files.Length == 0)
        {
            _userInterfaceService.ShowMessage("Список анкет пуст");
            return;
        }
        
        foreach (var file in files)
        {
            var name = Path.GetFileNameWithoutExtension(file);
            _userInterfaceService.ShowMessage(name);
        }
    }
}