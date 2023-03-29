using HandlebarsDotNet;

namespace RetroGPT.Core;

/// <summary>
/// Handlebars Template Renderer.
/// </summary>
public class HandlebarsTemplateRenderer : IHandlebarsTemplateRenderer
{
    private Dictionary<string, HandlebarsTemplate<object, object>> templateCache = new Dictionary<string, HandlebarsTemplate<object, object>>();

    private IHandlebars handlebars;

    /// <summary>
    /// Initializes a new instance of the <see cref="HandlebarsTemplateRenderer"/> class.
    /// </summary>
    /// <param name="baseDirectory">The base directory for the templates.</param>
    public HandlebarsTemplateRenderer(string baseDirectory)
    {
        this.BaseDirectory = baseDirectory;
        this.handlebars = HandlebarsDotNet.Handlebars.Create();
    }

    /// <inheritdoc/>
    public string BaseDirectory { get; }

    /// <inheritdoc/>
    public void RegisterPartialTemplate(string name, string templateHtml)
    {
        this.handlebars.RegisterTemplate(name, templateHtml);
    }

    /// <inheritdoc/>
    public string RenderHtml(string templatePath, object? viewModel)
    {
        HandlebarsTemplate<object, object>? template;

        if (!Path.IsPathRooted(templatePath))
        {
            templatePath = Path.Combine(this.BaseDirectory, templatePath);
        }

        this.templateCache.TryGetValue(templatePath, out template);
        if (!File.Exists(templatePath))
        {
            throw new NullReferenceException($"{templatePath} template missing.");
        }

        if (template is null)
        {
            var templateHtml = File.ReadAllText(templatePath);
            template = this.handlebars.Compile(templateHtml);
            this.templateCache.Add(templatePath, template);
        }

        return template.Invoke(viewModel ?? new { });
    }
}