using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;

namespace CloudSync.Adapters;

public class CloudStorageAdapter
{
    private readonly string path = "C:\\code\\github\\gothicsecret\\CloudSync\\CloudSync.App.Net6\\Credentials\\gcp.json";
    private readonly StorageClient _storageClient;

    public CloudStorageAdapter(StorageClient storageClient)
    {
        _storageClient = storageClient;
    }

    

    public async Task<UserCredential> GetCreds()
    {
        GoogleClientSecrets clientSecrets = GoogleClientSecrets.FromFile(path);
        UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //clientSecrets.Secrets,
            new ClientSecrets()
            {
                ClientId = "418984556101-r6suv4f4035s3ih6qr07ndkckfu1ugi4.apps.googleusercontent.com",
                ClientSecret = "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"
            },
            new[] { StorageService.Scope.DevstorageFullControl },
            "user",
            CancellationToken.None
        );
        return credential;
    }

    public async Task<StorageClient> GetStorageClientAsync()
    {
        // Replace with your Google Cloud Storage project ID
        string projectId = "api-project-418984556101";

        // Create an instance of the storage client
        var userCredential = await GetCreds();

        var storageClientBuilder = new StorageClientBuilder()
        {
            Credential = userCredential
        };
        var storage = await storageClientBuilder.BuildAsync();
        return storage;
    }

    public async Task<Bucket> GetBucketAsync(StorageClient storageClient)
    {
        return await storageClient.GetBucketAsync("test-bucket");
    }


    public async Task CreateBucket()
    {
        // Replace with your Google Cloud Storage project ID
        string projectId = "api-project-418984556101";

        // Create an instance of the storage client
        var userCredential = await GetCreds();

        var storageClientBuilder = new StorageClientBuilder()
        {
            Credential = userCredential
        };
        var storage = await storageClientBuilder.BuildAsync();

        // Create a bucket in your project
        var bucketName = $"my-new-bucket1-{projectId}";
        //storage.CreateBucket(projectId, bucketName);

        //Get existing bucket
        var bucket = await storage.GetBucketAsync(bucketName);

        // Upload a file to the bucket
        var objectName = "folder3/gcp.json";
        var filePath = "C:\\code\\gcp.json";
        using var fileStream = File.OpenRead(filePath);
        storage.UploadObject(bucketName, objectName, null, fileStream);
        //create directory
        //storage.UploadObject(bucketName, "folder2/", "application/x-directory", new MemoryStream());


        //// Download the file from the bucket
        //var downloadedObject = storage.GetObject(bucketName, objectName);
        //using var downloadedStream = new MemoryStream();
        //downloadedObject.MediaLink.DownloadAsync(downloadedStream).Wait();
        //var downloadedText = Encoding.UTF8.GetString(downloadedStream.ToArray());
    }
}
