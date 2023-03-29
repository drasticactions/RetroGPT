// <copyright file="ChatPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using RetroGPT.Core;
using UAParser;

namespace RetroGPT.Site;

/// <summary>
/// Chat Page.
/// </summary>
public class ChatPage : IPage
{
    private HandlebarsTemplateRenderer templateRenderer;
    private OpenAIService service;
    private UAParser.Parser parser;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatPage"/> class.
    /// </summary>
    /// <param name="templateRenderer"><see cref="HandlebarsTemplateRenderer"/>.</param>
    public ChatPage(OpenAIService service, HandlebarsTemplateRenderer templateRenderer)
    {
        this.parser = Parser.GetDefault();
        this.service = service;
        this.templateRenderer = templateRenderer;
    }

    /// <inheritdoc/>
    public string Route => "/chat";

    /// <inheritdoc/>
    public string PageName => "RetroGPT - Chat";

    /// <inheritdoc/>
    public RequestType RequestType => RequestType.GET;

    /// <inheritdoc/>
    public string TemplateName => "chat.html.hbs";

    /// <inheritdoc/>
    public async Task Invoke(HttpContext context)
    {
        // Try to get the user agent and reference that. If it doesn't work, use the default.
        var olderComputer = "You are running in a web browser on a older browser.";
        try
        {
            var userAgentString = context.Request.Headers["User-Agent"].ToString();
            var userAgent = this.parser.Parse(userAgentString);
            if (userAgent is not null)
            {
                var os = string.IsNullOrEmpty(userAgent.OS.ToString()) ? "an old OS" : userAgent.OS.ToString();
                olderComputer = $"The user is talking to you on {os}. " +
                                $"Reference and joke about their operating system in your introduction, " +
                                $"make special note if the operating system and browser are recent, as you are designed " +
                                $" to run on retro computers.";
            }
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e);
        }

        var completionResult = await this.service.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("You are a helpful assistant called RetroGPT, an assistant designed to run on retro computers. " +
                                       olderComputer + " Format your response as an HTML 2.0 compatible div."),
                ChatMessage.FromUser("Hello RetroGPT. Please introduce yourself. Format your response as an HTML 2.0 compatible div."),
            },
            Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo,
            MaxTokens = 250,
        });

        if (completionResult.Successful)
        {
            var content = this.templateRenderer.RenderHtml(this.TemplateName, new ChatViewModel() { InitialText = completionResult.Choices.FirstOrDefault()?.Message.Content ?? string.Empty });
            await context.WriteContentsWithEncodingAsync(content);
        }
        else
        {
            if (completionResult.Error == null)
            {
                throw new Exception("Unknown Error");
            }

            throw new Exception($"{completionResult.Error.Code}: {completionResult.Error.Message}");
        }
    }

    public class ChatViewModel
    {
        public bool HasResponseText => !string.IsNullOrEmpty(this.ResponseText);

        public string InitialText { get; set; } = string.Empty;

        public string ResponseText { get; set; } = string.Empty;
    }
}