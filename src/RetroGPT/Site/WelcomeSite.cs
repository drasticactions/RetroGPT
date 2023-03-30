// <copyright file="WelcomeSite.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Reflection;
using RetroGPT.Core;

namespace RetroGPT.Site;

public class WelcomeSite : ISite
{
    private HandlebarsTemplateRenderer templateRenderer;

    /// <summary>
    /// Initializes a new instance of the <see cref="WelcomeSite"/> class.
    /// </summary>
    public WelcomeSite()
    {
        var basePath = Path.GetDirectoryName(Helpers.GetAppLocation()) ?? string.Empty;
        var templatePath = Path.Combine(basePath, "Templates", "Welcome");
        if (!Directory.Exists(templatePath))
        {
            throw new ArgumentException($"Could not find base template path, {templatePath}");
        }

        this.templateRenderer = new HandlebarsTemplateRenderer(templatePath);
    }

    /// <inheritdoc/>
    public string Route => string.Empty;

    /// <inheritdoc/>
    public string Type => "Welcome";

    /// <inheritdoc/>
    public string Name => "Welcome";

    /// <inheritdoc/>
    public IReadOnlyList<IPage> Pages => new List<IPage>()
    {
        new WelcomePage(this.templateRenderer),
    };

    /// <summary>
    /// Welcome Page.
    /// </summary>
    private class WelcomePage : IPage
    {
        private HandlebarsTemplateRenderer templateRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WelcomePage"/> class.
        /// </summary>
        /// <param name="templateRenderer"><see cref="HandlebarsTemplateRenderer"/>.</param>
        public WelcomePage(HandlebarsTemplateRenderer templateRenderer)
        {
            this.templateRenderer = templateRenderer;
        }

        /// <inheritdoc/>
        public string Route => string.Empty;

        /// <inheritdoc/>
        public string PageName => "RetroGPT";

        /// <inheritdoc/>
        public RequestType RequestType => RequestType.GET;

        /// <inheritdoc/>
        public string TemplateName => "index.html.hbs";

        /// <inheritdoc/>
        public async Task Invoke(HttpContext context)
        {
            var content = this.templateRenderer.RenderHtml(this.TemplateName, null);
            await context.WriteContentsWithEncodingAsync(content);
        }
    }
}