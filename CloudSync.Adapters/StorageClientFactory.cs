using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;

namespace CloudSync.Adapters;

public class StorageClientFactory
{
    private readonly string path = "W:\\Project-CloudSync\\CloudSync\\CloudSync.App.Net6\\Credentials\\gcp.json";
    private readonly ClientSecretOptions _clientSecretOptions;

    public StorageClientFactory(IOptions<ClientSecretOptions> options)
    {
        _clientSecretOptions = options.Value;
    }

    public async Task<StorageClient> CreateAsync()
    {
        // Create an instance of the storage client
        var userCredential = await GetCredsAsync();

        var storageClientBuilder = new StorageClientBuilder()
        {
            Credential = userCredential
        };
        var storage = await storageClientBuilder.BuildAsync();
        return storage;
    }

    public async Task<UserCredential> GetCredsAsync()
    {
        GoogleClientSecrets clientSecrets = GoogleClientSecrets.FromFile(path);
        UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            //clientSecrets.Secrets,
            new ClientSecrets()
            {
                ClientId = _clientSecretOptions.ClientId,
                ClientSecret = _clientSecretOptions.ClientSecret
            },
            new[] { StorageService.Scope.DevstorageFullControl },
            "user",
            CancellationToken.None
        );
        return credential;
    }
}
