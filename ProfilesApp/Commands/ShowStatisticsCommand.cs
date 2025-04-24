using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class ShowStatisticsCommand : ICommand, IHasNameAndDescription
{
    public string Name => "statistics";
    public string Description => "Показать статистику всех заполненных анкет";
    
    private readonly IStatisticsService _statisticsService;
    private readonly IProfilesRepository _profilesRepository;
    private readonly IUserInterfaceService _userInterfaceService;

    public ShowStatisticsCommand(IStatisticsService statisticsService,
        IProfilesRepository profilesRepository, IUserInterfaceService userInterfaceService)
    {
        _statisticsService = statisticsService;
        _profilesRepository = profilesRepository;
        _userInterfaceService = userInterfaceService;
    }
    
    public void Execute(params string[] args)
    {
        var models = _profilesRepository.GetAll();
        var statistics = _statisticsService.GetOverallStatistics(models);
        if (string.IsNullOrEmpty(statistics))
        {
            _userInterfaceService.ShowMessage("Нет данных о профилях");
        }
        else
        {
            _userInterfaceService.ShowMessage(statistics);
        }
    }
}