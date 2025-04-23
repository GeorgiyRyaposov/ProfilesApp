using System.IO.Compression;
using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class ZipProfileCommand : ICommand, IHasNameAndDescription
{
    public string Name => "zip";

    public string Description =>
        "<Имя файла анкеты> <Путь для сохранения архива> - Запаковать указанную анкету в архив и сохранить архив по указанному пути";

    private readonly IProfilesRepository _profilesRepository;

    public ZipProfileCommand(IProfilesRepository profilesRepository)
    {
        _profilesRepository = profilesRepository;
    }

    public async void Execute(params string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Введите <Имя файла анкеты> <Путь для сохранения архива>");
            return;
        }

        var fileName = args[0];
        var fullPath = _profilesRepository.FindProfilePath(fileName);
        if (string.IsNullOrEmpty(fullPath))
        {
            Console.WriteLine("Анкета не найдена");
            return;
        }


        var zipPath = args[1];
        var attributes = File.GetAttributes(zipPath);
        if (!attributes.HasFlag(FileAttributes.Directory))
        {
            Console.WriteLine("Путь не является директорией");
            return;
        }

        Directory.CreateDirectory(zipPath);

        var zipFullPath = Path.Combine(zipPath, $"{fileName}.gz");
        await CompressAsync(fullPath, zipFullPath);
    }

    private async Task CompressAsync(string sourceFile, string compressedFile)
    {
        await using var sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate);
        await using var targetStream = File.Create(compressedFile);
        await using var compressionStream = new GZipStream(targetStream, CompressionMode.Compress);

        await sourceStream.CopyToAsync(compressionStream);

        Console.WriteLine($"Сжатие файла {sourceFile} завершено.");
        Console.WriteLine($"Исходный размер: {sourceStream.Length}  сжатый размер: {targetStream.Length}");
    }
}