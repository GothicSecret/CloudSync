namespace CloudSync.Adapters;

public class ClientSecretOptions
{
    public const string SectionName = "CloudSync";

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }
}
