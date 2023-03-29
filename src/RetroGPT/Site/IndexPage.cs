using RetroGPT.Core;

namespace RetroGPT.Site;

/// <summary>
/// Index Page.
/// </summary>
public class IndexPage : IPage
{
    private HandlebarsTemplateRenderer templateRenderer;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexPage"/> class.
    /// </summary>
    /// <param name="templateRenderer"><see cref="HandlebarsTemplateRenderer"/>.</param>
    public IndexPage(HandlebarsTemplateRenderer templateRenderer)
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