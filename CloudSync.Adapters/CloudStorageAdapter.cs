using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;

namespace CloudSync.Adapters;

public class CloudStorageAdapter
{
    private readonly StorageClientFactory _storageClientFactory;

    public CloudStorageAdapter(StorageClientFactory storageClientFactory)
    {
        _storageClientFactory = storageClientFactory;
    }

    public async Task<Bucket> GetBucketAsync(StorageClient storageClient)
    {
        return await storageClient.GetBucketAsync("test-bucket");
    }

    public async Task CreateBucketAsync()
    {
        var bucketName = "8tb-archive";
        //var bucket = await _storageClient.GetBucketAsync(bucketName);

        var storageClient = await _storageClientFactory.CreateAsync();
        var response = storageClient.ListObjectsAsync(bucketName);

        await foreach (var bucketObject in response)
        {
            try
            {
                await Console.Out.WriteLineAsync($"Downloading {bucketObject.Name} {bucketObject.Size / 1024 / 1024} MB");
                var objPath = Path.Combine("R:\\8tb-archive", bucketObject.Name);
                var folder = Path.GetDirectoryName(objPath);
                FolderHelper.CreateFolderStructure(folder);
                if (bucketObject.Name.EndsWith("/"))
                {
                    continue;
                }
                if (File.Exists(objPath))
                {
                    var md5Helper = Md5Helper.CalculateMD5(objPath);
                    var bucketObjectCrc32 = bucketObject.Md5Hash;
                    if (md5Helper == bucketObjectCrc32)
                    {
                        continue;
                    }
                }
                using var fs = new FileStream(objPath, FileMode.Create, FileAccess.Write);
                await storageClient.DownloadObjectAsync(bucketName, bucketObject.Name, fs);
                await fs.FlushAsync();
                await Console.Out.WriteLineAsync($"Download completed");
            }
            catch (Exception)
            {
                File.AppendAllText(Path.Combine("R:\\8tb-archive", "errors.txt"), bucketObject.Name + "\r\n");
            }
        }
    }
}
