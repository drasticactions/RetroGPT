using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using RetroGPT.Core;

namespace RetroGPT.Site;

/// <summary>
/// Image Response Page.
/// </summary>
public class ImageResponsePage : IPage
{
    private HandlebarsTemplateRenderer templateRenderer;
    private OpenAIService service;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageResponsePage"/> class.
    /// </summary>
    /// <param name="templateRenderer"><see cref="HandlebarsTemplateRenderer"/>.</param>
    public ImageResponsePage(OpenAIService service, HandlebarsTemplateRenderer templateRenderer)
    {
        this.service = service;
        this.templateRenderer = templateRenderer;
    }

    /// <inheritdoc/>
    public string Route => "/image";

    /// <inheritdoc/>
    public string PageName => "RetroGPT - Image";

    /// <inheritdoc/>
    public RequestType RequestType => RequestType.POST;

    /// <inheritdoc/>
    public string TemplateName => "image.html.hbs";

    /// <inheritdoc/>
    public async Task Invoke(HttpContext context)
    {
        var result = context.Request.Form.TryGetValue("editor", out var prompt);
        if (string.IsNullOrEmpty(prompt) || prompt.Count > 1000)
        {
            var defaultResponse = this.templateRenderer.RenderHtml(this.TemplateName, null);
            await context.WriteContentsWithEncodingAsync(defaultResponse);
            return;
        }

        var imageResult = await this.service.Image.CreateImage(new ImageCreateRequest
        {
            Prompt = prompt!,
            N = 1,
            Size = StaticValues.ImageStatics.Size.Size256,
            ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
            User = "RetroGPT User",
        });

        var imageModel = new ImageResponseViewModel();

        if (imageResult.Successful)
        {
            imageModel.ImageUrl = imageResult.Results.FirstOrDefault()?.Url;
        }

        imageModel.Prompt = prompt;
        var content = this.templateRenderer.RenderHtml(this.TemplateName, imageModel);
        await context.WriteContentsWithEncodingAsync(content);
    }

    private class ImageResponseViewModel
    {
        public bool HasImage => !string.IsNullOrEmpty(this.ImageUrl);

        public string? ImageUrl { get; set; }
        
        public string? Prompt { get; set; }
    }
}