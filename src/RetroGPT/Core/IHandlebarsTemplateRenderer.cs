// <copyright file="IHandlebarsTemplateRenderer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace RetroGPT.Core;

/// <summary>
/// Handlebars Template Renderer.
/// </summary>
public interface IHandlebarsTemplateRenderer : ITemplateRenderer
{
    /// <summary>
    /// Register Partial Template for the given Template Renderer.
    /// </summary>
    /// <param name="name">The name of the template.</param>
    /// <param name="templateHtml">The template HTML.</param>
    void RegisterPartialTemplate(string name, string templateHtml);
}