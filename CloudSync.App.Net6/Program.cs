using System.Net;
using System.Reflection;
using CloudSync.Adapters;

Directory.SetCurrentDirectory(
    Path.GetDirectoryName(
        Assembly.GetExecutingAssembly().Location));

var cloudSyncEnv = Environment.GetEnvironmentVariable("CloudSync");

var builder = WebApplication.CreateBuilder();

builder.WebHost
#pragma warning disable RCS1163 // Unused parameter.
    .ConfigureAppConfiguration((context, configBuilder) =>
#pragma warning restore RCS1163 // Unused parameter.
    {
        configBuilder.SetBasePath(Directory.GetCurrentDirectory());
        configBuilder.AddJsonFile("hostsettings.json", optional: false);
        if (string.IsNullOrWhiteSpace(cloudSyncEnv))
            configBuilder.AddJsonFile($"hostsettings.{cloudSyncEnv}.json", optional: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddCloudSyncAdapters(context.Configuration);
    })
    .ConfigureKestrel(options =>
    {
        var ipAddress = IPAddress.Parse("127.0.0.1");
        options.Listen(new IPEndPoint(ipAddress, 65100));
    });
var app = builder.Build();

await app.RunAsync();

//string projectId = "api-project-418984556101";
//var storageClient = await storageClientFactory.CreateAsync();
//await cloudStorageAdapter.CreateBucketAsync();
