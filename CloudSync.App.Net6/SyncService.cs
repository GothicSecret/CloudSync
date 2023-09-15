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

    public async Task SyncAsync(string basePath)
    {
        foreach (var directory in Directory.EnumerateDirectories(basePath))
        {
            await SyncDirectoryAsync(directory);
        }
    }

    public async Task SyncDirectoryAsync(string directoryPath)
    {
        await EnsureDirectoryExistsAsync(directoryPath);
        foreach (var file in Directory.EnumerateFiles(directoryPath))
        {
            await SyncFilesAsync(directoryPath);
        }
        foreach (var directory in Directory.EnumerateDirectories(directoryPath))
        {
            await SyncDirectoryAsync(directory);
        }
    }

    public Task SyncFilesAsync(string directoryPath)
    {
        return Task.CompletedTask;
    }

    public async Task EnsureDirectoryExistsAsync(string directoryPath)
    {
        if (await _bucketAdapter.DirectoryExistsAsync(directoryPath))
        {
            return;
        }
        await _bucketAdapter.CreateDirectoryAsync(directoryPath);
    }
}
