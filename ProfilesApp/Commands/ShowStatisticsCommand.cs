using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class ShowStatisticsCommand : ICommand, IHasNameAndDescription
{
    public string Name => "statistics";
    public string Description => "Показать статистику всех заполненных анкет";
    
    private readonly IStatisticsService _statisticsService;
    private readonly IProfilesRepository _profilesRepository;

    public ShowStatisticsCommand(IStatisticsService statisticsService,
        IProfilesRepository profilesRepository)
    {
        _statisticsService = statisticsService;
        _profilesRepository = profilesRepository;
    }
    
    public void Execute(params string[] args)
    {
        var models = _profilesRepository.GetAll();
        _statisticsService.Show(models);
    }
}