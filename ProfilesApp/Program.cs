using Microsoft.Extensions.DependencyInjection;
using ProfilesApp.Extensions;
using ProfilesApp.Services;
using ProfilesApp.StateMachine;
using ProfilesApp.StateMachine.States;

namespace ProfilesApp
{
    /// <summary>
    /// Машина состояний <see cref="IAppStateMachine"/> управляет списком доступных команд.
    /// Основная часть логики работы с профилями распределена по сервисам.
    /// Команды в своей работе используют эти сервисы.
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            var serviceProvider = BuildServiceProvider();

            var userInterfaceService = serviceProvider.GetService<IUserInterfaceService>();
            var commandsService = serviceProvider.GetService<ICommandsService>();
            var stateMachine = serviceProvider.GetService<IAppStateMachine>();
            stateMachine.SetInitialState();

            while (true)
            {
                if (stateMachine.CompletedWork)
                {
                    break;
                }

                var input = userInterfaceService.ReadLineInput();
                var executed = commandsService.TryExecute(input);
                if (!executed)
                {
                    userInterfaceService.ShowMessage("Введена неверная команда, попробуйте использовать -help");
                }
            }
        }

        private static ServiceProvider BuildServiceProvider()
        {
            ServiceCollection services = new();
            services.AddSingleton<ICommandsService, CommandsService>();
            services.AddSingleton<IProfileBuilder, ProfileBuilder>();
            services.AddSingleton<IProfilesRepository, ProfilesRepository>();
            services.AddSingleton<IStatisticsService, StatisticsService>();
            services.AddSingleton<IUserInterfaceService, UserInterfaceService>();
            services.AddSingleton<ILocalizationService, LocalizationService>();

            services.AddSingleton<IAppStateMachine, AppStateMachine>();
            services.AddTransient<InitialState>();
            services.AddTransient<ProfileCreationState>();
            services.AddTransient<ExitState>();

            services.AddCommands();
            services.AddFieldsProcessors();

            return services.BuildServiceProvider();
        }
    }
}