using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class DeleteFileCommand : ICommand, IHasNameAndDescription
{
    public string Name => "delete";
    public string Description => "<Имя файла анкеты> - Удалить указанную анкету";
    
    private readonly IProfilesRepository _profilesRepository;
    private readonly IUserInterfaceService _userInterfaceService;

    public DeleteFileCommand(IProfilesRepository profilesRepository, 
        IUserInterfaceService userInterfaceService)
    {
        _profilesRepository = profilesRepository;
        _userInterfaceService = userInterfaceService;
    }
    
    public void Execute(params string[] args)
    {
        if (args.Length == 0)
        {
            _userInterfaceService.ShowMessage("Укажите ФИО для удаления");
            return;
        }
        
        if (_profilesRepository.DeleteProfile(args[0]))
        {
            _userInterfaceService.ShowMessage("Анкета успешно удалена");
        }
        else
        {
            _userInterfaceService.ShowMessage("Не удалось удалить анкету");
        }
    }
}