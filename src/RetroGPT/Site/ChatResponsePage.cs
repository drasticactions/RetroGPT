// <copyright file="ChatResponsePage.cs" company="Drastic Actions">
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
public class ChatResponsePage : IPage
{
    private HandlebarsTemplateRenderer templateRenderer;
    private OpenAIService service;
    private UAParser.Parser parser;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatResponsePage"/> class.
    /// </summary>
    /// <param name="templateRenderer"><see cref="HandlebarsTemplateRenderer"/>.</param>
    public ChatResponsePage(OpenAIService service, HandlebarsTemplateRenderer templateRenderer)
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
    public RequestType RequestType => RequestType.POST;

    /// <inheritdoc/>
    public string TemplateName => "chat.html.hbs";

    /// <inheritdoc/>
    public async Task Invoke(HttpContext context)
    {
        var result = context.Request.Form.TryGetValue("editor", out var prompt);
        if (string.IsNullOrEmpty(prompt) || prompt.Count > 4000)
        {
            var defaultResponse = this.templateRenderer.RenderHtml(this.TemplateName, null);
            await context.WriteContentsWithEncodingAsync(defaultResponse);
            return;
        }

        var completionResult = await this.service.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("You are a helpful assistant called RetroGPT, an assistant designed to run on retro computers. " +
                                       $"You speak in the language the user speaks. Format your response as an HTML 2.0 compatible div."),
                ChatMessage.FromAssistant("<div><p>Hello! I'm RetroGPT! What is your question?</p></div>"),
                ChatMessage.FromUser(prompt!),
            },
            Model = OpenAI.GPT3.ObjectModels.Models.ChatGpt3_5Turbo,
            MaxTokens = 600,
        });

        if (completionResult.Successful)
        {
            var content = this.templateRenderer.RenderHtml(this.TemplateName, new ChatViewModel() { ResponseText = prompt!, InitialText = completionResult.Choices.FirstOrDefault()?.Message.Content ?? string.Empty });
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