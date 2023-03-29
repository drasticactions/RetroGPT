// <copyright file="IPage.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace RetroGPT.Core;

/// <summary>
/// Page.
/// </summary>
public interface IPage
{
    /// <summary>
    /// Gets the route for the page.
    /// </summary>
    public string Route { get; }

    /// <summary>
    /// Gets the page name.
    /// </summary>
    public string PageName { get; }

    /// <summary>
    /// Gets the template name for the page.
    /// </summary>
    public string TemplateName { get; }

    /// <summary>
    /// Gets the request type for the page (GET, POST, etc).
    /// </summary>
    public RequestType RequestType { get; }

    /// <summary>
    /// Invoke action on page.
    /// </summary>
    /// <param name="context"><see cref="HttpContext"/>.</param>
    /// <returns>Task.</returns>
    Task Invoke(HttpContext context);
}