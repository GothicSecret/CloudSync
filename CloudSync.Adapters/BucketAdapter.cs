using Google.Cloud.Storage.V1;

namespace CloudSync.Adapters;

public class BucketAdapter
{
    private const string ContentType = "application/x-directory";
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public BucketAdapter(StorageClient storageClient, string bucketName)
    {
        _storageClient = storageClient;
        _bucketName = bucketName;
    }

    public async Task<bool> DirectoryExistsAsync(string path)
    {
        path = EnsurePathIsValid(path);
        var directory = await _storageClient.GetObjectAsync(_bucketName, path);
        return directory != null;
    }

    public async Task CreateDirectoryAsync(string path)
    {
        path = EnsurePathIsValid(path);
        await _storageClient.UploadObjectAsync(_bucketName, path, ContentType, new MemoryStream());
    }

    public string EnsurePathIsValid(string path)
    {
        return path[path.Length - 1] == '/' ? path : path + '/';
    }
}
