// <copyright file="RetroGPTOptions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Text.Json;

namespace RetroGPT.Core;

public class RetroGPTOptions
{
    public OpenAIServiceOptions OpenAIServiceOptions { get; set; } = new OpenAIServiceOptions();

    public static RetroGPTOptions FromSettingsFile()
    {
        var settingsPath = Helpers.GetLocalFilePath("APITokens.json");
        if (File.Exists(settingsPath))
        {
            var text = File.ReadAllText(settingsPath);
            return JsonSerializer.Deserialize<RetroGPTOptions>(text) ?? new RetroGPTOptions();
        }

        var defaultOptions = new RetroGPTOptions();
        File.WriteAllText(settingsPath, JsonSerializer.Serialize(defaultOptions, new JsonSerializerOptions() { WriteIndented = true }));
        return defaultOptions;
    }
}

public class OpenAIServiceOptions
{
    public string ApiKey { get; set; } = string.Empty;

    public string Organization { get; set; } = string.Empty;
}