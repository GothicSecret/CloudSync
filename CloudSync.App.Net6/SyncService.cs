using System.IO;
using CloudSync.Adapters;

namespace CloudSync.App.Net6;

public class SyncService
{
    private readonly BucketAdapter _bucketAdapter;

    public SyncService(BucketAdapter bucketAdapter)
    {
        _bucketAdapter = bucketAdapter;
    }

    public async Task Sync(string basePath)
    {
        foreach (var directory in Directory.EnumerateDirectories(basePath))
        {
            await SyncDirectory(directory);
        }
    }

    public async Task SyncDirectory(string directoryPath)
    {
        await EnsureDirectoryExists(directoryPath);
        foreach (var file in Directory.EnumerateFiles(directoryPath))
        {
            await SyncFiles(directoryPath);
        }
        foreach (var directory in Directory.EnumerateDirectories(directoryPath))
        {
            await SyncDirectory(directory);
        }
    }

    public async Task SyncFiles(string directoryPath)
    {
        //sync files here
    }

    public async Task EnsureDirectoryExists(string directoryPath)
    {
        if (await _bucketAdapter.DirectoryExists(directoryPath))
        {
            return;
        }
        await _bucketAdapter.CreateDirectory(directoryPath);
    }
}
