using System.Text;
using ImageIdentifier;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

Console.InputEncoding = Encoding.Unicode;
Console.OutputEncoding = Encoding.Unicode;

try
{
    // Validate client
    var credentials = new ApiKeyServiceClientCredentials(Variables.Instance.CognitiveServiceKey);
    var client = new ComputerVisionClient(credentials)
    {
        Endpoint = Variables.Instance.CognitiveServiceEndpoint,
    };

    // Loop input
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Enter an image via URL or local path...");

        // Get input
        var input = Console.ReadLine()?.Trim() ?? "";

        // "Q" to Quit application
        if (input.ToLower().Equals("q"))
            break;

        try
        {
            // Identify Image
            if (Uri.TryCreate(input, UriKind.Absolute, out var uri))
            {
                ImageDescription? imageDescription;

                Console.WriteLine("\nAnalyzing image...");

                if (uri.IsFile)
                    // Handle file
                    imageDescription = await client.DescribeImageInStreamAsync(File.OpenRead(uri.LocalPath));
                else
                    // Handle link
                    imageDescription = await client.DescribeImageAsync(uri.AbsoluteUri);

                if (imageDescription is not null)
                {
                    // Print result
                    var confidence = $"{Math.Round(imageDescription.Captions[0].Confidence * 100)}%";
                    var description = imageDescription.Captions[0].Text;

                    Console.WriteLine($"I am {confidence} sure that is an image of {description}!\n");
                }
            }
            else
            {
                throw new Exception("Bad input");
            }
        }
        catch
        {
            Console.WriteLine("\nSomething went wrong!");
        }


        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}