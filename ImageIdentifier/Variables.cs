using Microsoft.Extensions.Configuration;

namespace ImageIdentifier;

public class Variables
{
    public static readonly Variables Instance = new();
    public readonly string CognitiveServiceEndpoint;
    public readonly string CognitiveServiceKey;

    Variables()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var configuration = builder.Build();

        CognitiveServiceEndpoint = configuration["CognitiveServiceEndpoint"];
        CognitiveServiceKey = configuration["CognitiveServiceKey"];
    }
}