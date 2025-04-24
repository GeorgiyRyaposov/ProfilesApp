using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class FindProfileCommand : ICommand, IHasNameAndDescription
{
    public string Name => "find";
    public string Description => "<Имя файла анкеты> - Найти анкету";
    
    private readonly IProfilesRepository _profilesRepository;
    private readonly IUserInterfaceService _userInterfaceService;

    public FindProfileCommand(IProfilesRepository profilesRepository, IUserInterfaceService userInterfaceService)
    {
        _profilesRepository = profilesRepository;
        _userInterfaceService = userInterfaceService;
    }
    
    public void Execute(params string[] args)
    {
        if (args.Length == 0)
        {
            _userInterfaceService.ShowMessage("Укажите ФИО");
            return;
        }
        
        var profilePath = _profilesRepository.FindProfilePath(args[0]);
        if (string.IsNullOrEmpty(profilePath))
        {
            _userInterfaceService.ShowMessage("Анкета не найдена");
            return;
        }

        var text = File.ReadAllText(profilePath);
        _userInterfaceService.ShowMessage(text);
    }
}