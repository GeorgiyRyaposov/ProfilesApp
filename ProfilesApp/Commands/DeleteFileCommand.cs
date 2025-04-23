using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class DeleteFileCommand : ICommand, IHasNameAndDescription
{
    public string Name => "delete";
    public string Description => "<Имя файла анкеты> - Удалить указанную анкету";
    
    private readonly IProfilesRepository _profilesRepository;

    public DeleteFileCommand(IProfilesRepository profilesRepository)
    {
        _profilesRepository = profilesRepository;
    }
    
    public void Execute(params string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Укажите ФИО для удаления");
            return;
        }
        
        if (_profilesRepository.DeleteProfile(args[0]))
        {
            Console.WriteLine("Анкета успешно удалена");
        }
        else
        {
            Console.WriteLine("Не удалось удалить анкету");
        }
    }
}