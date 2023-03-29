// <copyright file="SiteSetup.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using RetroGPT.Core;

namespace RetroGPT;

/// <summary>
/// Site Setup.
/// </summary>
public static class SiteSetup
{
    /// <summary>
    /// Gets the lists of sites.
    /// </summary>
    public static IList<ISite> Sites { get; } = new List<ISite>();
}