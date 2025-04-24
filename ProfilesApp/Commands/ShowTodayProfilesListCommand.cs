using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class ShowTodayProfilesListCommand : ICommand, IHasNameAndDescription
{
    public string Name => "list_today";
    public string Description => "Показать список названий файлов всех сохранённых анкет, созданных сегодня";

    private readonly IProfilesRepository _profilesRepository;
    private readonly IUserInterfaceService _userInterfaceService;

    public ShowTodayProfilesListCommand(IProfilesRepository profilesRepository, 
        IUserInterfaceService userInterfaceService)
    {
        _profilesRepository = profilesRepository;
        _userInterfaceService = userInterfaceService;
    }

    public void Execute(params string[] args)
    {
        var files = _profilesRepository.GetAllFiles(DateTime.UtcNow);
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