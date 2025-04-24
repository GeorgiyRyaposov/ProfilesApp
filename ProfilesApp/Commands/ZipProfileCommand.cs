using System.IO.Compression;
using ProfilesApp.Services;

namespace ProfilesApp.Commands;

public class ZipProfileCommand : ICommand, IHasNameAndDescription
{
    public string Name => "zip";

    public string Description =>
        "<Имя файла анкеты> <Путь для сохранения архива> - Запаковать указанную анкету в архив и сохранить архив по указанному пути";

    private readonly IProfilesRepository _profilesRepository;
    private readonly IUserInterfaceService _userInterfaceService;

    public ZipProfileCommand(IProfilesRepository profilesRepository, IUserInterfaceService userInterfaceService)
    {
        _profilesRepository = profilesRepository;
        _userInterfaceService = userInterfaceService;
    }

    public async void Execute(params string[] args)
    {
        if (args.Length < 2)
        {
            _userInterfaceService.ShowMessage("Введите <Имя файла анкеты> <Путь для сохранения архива>");
            return;
        }

        var fileName = args[0];
        var fullPath = _profilesRepository.FindProfilePath(fileName);
        if (string.IsNullOrEmpty(fullPath))
        {
            _userInterfaceService.ShowMessage("Анкета не найдена");
            return;
        }


        var zipPath = args[1];
        var attributes = File.GetAttributes(zipPath);
        if (!attributes.HasFlag(FileAttributes.Directory))
        {
            _userInterfaceService.ShowMessage("Путь не является директорией");
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

        _userInterfaceService.ShowMessage($"Сжатие файла {sourceFile} завершено.");
        _userInterfaceService.ShowMessage($"Исходный размер: {sourceStream.Length}  сжатый размер: {targetStream.Length}");
    }
}