using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class SaveProfileCommand : ICommand, IHasNameAndDescription
{
    public string Name => "save";
    public string Description => "Сохранить заполненную анкету";

    private readonly IProfileBuilder _profileBuilder;
    private readonly IProfilesRepository _profilesRepository;

    public SaveProfileCommand(IProfileBuilder profileBuilder, IProfilesRepository profilesRepository)
    {
        _profileBuilder = profileBuilder;
        _profilesRepository = profilesRepository;
    }
    
    public void Execute(params string[] args)
    {
        if (_profileBuilder.IsProfileCompleted)
        {
            var profile = _profileBuilder.BuildProfile();
            _profilesRepository.Save(profile);
            _profileBuilder.Reset();
        }
        else
        {
            Console.WriteLine("Не все поля заполнены, попробуйте ещё раз");
        }
    }
}