// <copyright file="ImagePage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using OpenAI.GPT3.Managers;
using RetroGPT.Core;

namespace RetroGPT.Site;

/// <summary>
/// Image Page.
/// </summary>
public class ImagePage : IPage
{
    private HandlebarsTemplateRenderer templateRenderer;
    private OpenAIService service;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImagePage"/> class.
    /// </summary>
    /// <param name="templateRenderer"><see cref="HandlebarsTemplateRenderer"/>.</param>
    public ImagePage(OpenAIService service, HandlebarsTemplateRenderer templateRenderer)
    {
        this.service = service;
        this.templateRenderer = templateRenderer;
    }

    /// <inheritdoc/>
    public string Route => "/image";

    /// <inheritdoc/>
    public string PageName => "RetroGPT - Image";

    /// <inheritdoc/>
    public RequestType RequestType => RequestType.GET;

    /// <inheritdoc/>
    public string TemplateName => "image.html.hbs";

    /// <inheritdoc/>
    public async Task Invoke(HttpContext context)
    {
        var content = this.templateRenderer.RenderHtml(this.TemplateName, null);
        await context.WriteContentsWithEncodingAsync(content);
    }
}