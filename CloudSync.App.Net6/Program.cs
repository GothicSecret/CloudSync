// See https://aka.ms/new-console-template for more information

using CloudSync.Adapters;

Console.WriteLine("Hello, World!");

var cloudStorageAdapter = new CloudStorageAdapter();
await cloudStorageAdapter.CreateBucket();


Console.ReadLine();