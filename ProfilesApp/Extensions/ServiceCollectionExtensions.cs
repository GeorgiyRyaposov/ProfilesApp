using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ProfilesApp.Commands;
using ProfilesApp.ProfileFieldsProcessors;

namespace ProfilesApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommands(this IServiceCollection services)
    {
        var commandTypes = GetAllTypes<ICommand>();

        foreach (var type in commandTypes)
        {
            services.AddTransient(type);
        }
    }

    public static void AddFieldsProcessors(this IServiceCollection services)
    {
        var fieldProcessors = GetAllTypes<IFieldProcessor>();

        var typeOfIFieldProcessor = typeof(IFieldProcessor);
        foreach (var type in fieldProcessors)
        {
            services.AddTransient(typeOfIFieldProcessor, type);
        }
    }

    private static Type[] GetAllTypes<T>()
    {
        var assembly = Assembly.GetExecutingAssembly();

        return assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(T).IsAssignableFrom(t))
            .ToArray();
    }
}