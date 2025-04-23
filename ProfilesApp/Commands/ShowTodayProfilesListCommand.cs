using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class ShowTodayProfilesListCommand : ICommand, IHasNameAndDescription
{
    public string Name => "list_today";
    public string Description => "Показать список названий файлов всех сохранённых анкет, созданных сегодня";

    private readonly IProfilesRepository _profilesRepository;

    public ShowTodayProfilesListCommand(IProfilesRepository profilesRepository)
    {
        _profilesRepository = profilesRepository;
    }

    public void Execute(params string[] args)
    {
        var files = _profilesRepository.GetAllFiles(DateTime.UtcNow);
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