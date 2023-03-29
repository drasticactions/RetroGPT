// <copyright file="ISite.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace RetroGPT.Core;

/// <summary>
/// Site.
/// </summary>
public interface ISite
{
    /// <summary>
    /// Gets the route for the site.
    /// </summary>
    string Route { get; }

    /// <summary>
    /// Gets the type of site this is. Used for sorting.
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Gets the name of the site.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets a list of <see cref="IPage"/>.
    /// </summary>
    IReadOnlyList<IPage> Pages { get; }
}