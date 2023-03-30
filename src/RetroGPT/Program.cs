// <copyright file="Program.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Text;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using RetroGPT;
using RetroGPT.Core;
using RetroGPT.Site;

// Create the downloads directory.
Directory.CreateDirectory(Path.Combine(Helpers.GetLocalFilePath(string.Empty), "Downloads"));

// In order for encodings to work when writing out HTML for some providers
// Like Shift-JIS, we need to register the encoding handler.
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var settings = RetroGPTOptions.FromSettingsFile();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5001");

var app = builder.Build();

HttpFileClient client = new HttpFileClient();

IImageProxyHandler imageProxy = new ImageSharpProxyHandler(client);
FileHandlers fileHandlers = new FileHandlers();

app.MapGet("/proxy/image/{*remander}", imageProxy.InvokeImageProxy);

if (!string.IsNullOrEmpty(settings.OpenAIServiceOptions.ApiKey))
{
    var openAiService = new OpenAIService(new OpenAiOptions()
    {
        ApiKey = settings.OpenAIServiceOptions.ApiKey,
        Organization = settings.OpenAIServiceOptions.Organization,
    });
    SiteSetup.Sites.Add(new RetroGPTSite(openAiService));
}
else
{
    SiteSetup.Sites.Add(new WelcomeSite());
}

foreach (var site in SiteSetup.Sites)
{
    foreach (var page in site.Pages)
    {
        string endpointName;

        // Index of the site.
        if (site.Route == "/" & page.Route == "/")
        {
            endpointName = "/";
        }

        // Index of the feature page.
        else if (site.Route != "/" && page.Route == string.Empty)
        {
            endpointName = site.Route;
        }

        // Nested page within the feature page.
        else
        {
            endpointName = site.Route + page.Route;
        }

        switch (page.RequestType)
        {
            case RequestType.GET:
                app.MapGet(endpointName, page.Invoke);
                break;
            case RequestType.POST:
                app.MapPost(endpointName, page.Invoke);
                break;
        }
    }
}

app.MapGet("/RetroGPT/images/{*remander}", fileHandlers.Images);
app.MapGet("/Welcome/images/{*remander}", fileHandlers.Images);

Console.WriteLine("RetroGPT is now running! Try accessing it on one of these IP Addresses");

Console.WriteLine("http://127.0.0.1:5005");
try
{
    foreach (var ip in NetworkUtils.DeviceIps())
    {
        Console.WriteLine($"http://{ip}:5005");
    }
}
catch (Exception ex)
{
    // Todo: Ignore for now.
}

// TODO: Allow for other ports.
app.Run("http://*:5005");