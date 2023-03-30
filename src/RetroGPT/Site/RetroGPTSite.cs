// <copyright file="RetroGPTSite.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Reflection;
using OpenAI.GPT3.Managers;
using RetroGPT.Core;

namespace RetroGPT.Site;

public class RetroGPTSite : ISite
{
    private HandlebarsTemplateRenderer templateRenderer;
    private OpenAIService service;

    /// <summary>
    /// Initializes a new instance of the <see cref="RetroGPTSite"/> class.
    /// </summary>
    public RetroGPTSite(OpenAIService service)
    {
        this.service = service;
        var basePath = Path.GetDirectoryName(Helpers.GetAppLocation()) ?? string.Empty;
        var templatePath = Path.Combine(basePath, "Templates", "RetroGPT");
        if (!Directory.Exists(templatePath))
        {
            throw new ArgumentException($"Could not find base template path, {templatePath}");
        }

        this.templateRenderer = new HandlebarsTemplateRenderer(templatePath);

        this.RegisterPartialTemplates();
    }

    /// <inheritdoc/>
    public string Route => string.Empty;

    /// <inheritdoc/>
    public string Type => "RetroGPT";

    /// <inheritdoc/>
    public string Name => "RetroGPT";

    /// <inheritdoc/>
    public IReadOnlyList<IPage> Pages => new List<IPage>()
    {
        new IndexPage(this.templateRenderer),
        new ChatPage(this.service, this.templateRenderer),
        new ImagePage(this.service, this.templateRenderer),
        new ImageResponsePage(this.service, this.templateRenderer),
        new ChatResponsePage(this.service, this.templateRenderer),
    };

    public void RegisterPartialTemplates()
    {
        var assemblyPath = Path.GetDirectoryName(Helpers.GetAppLocation()) ?? string.Empty;
        var path = Path.Combine(assemblyPath, "Templates", "RetroGPT");
        foreach (var item in Directory.EnumerateFiles(path, "*_partial*"))
        {
            var templateHtml = File.ReadAllText(item);
            var filename = Path.GetFileName(item).Split(".").First();
            this.templateRenderer.RegisterPartialTemplate(filename, templateHtml);
        }
    }
}