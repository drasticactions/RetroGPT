// <copyright file="IImageProxyHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace RetroGPT.Core;

/// <summary>
/// Image Proxy Handler.
/// </summary>
public interface IImageProxyHandler
{
    /// <summary>
    /// Gets the default download path.
    /// </summary>
    string DefaultDownloadPath { get; }

    /// <summary>
    /// Gets the default cache path.
    /// </summary>
    string DefaultCachePath { get; }

    /// <summary>
    /// Invoke Image Proxy for a given image.
    /// </summary>
    /// <param name="context"><see cref="HttpContent"/>.</param>
    /// <returns><see cref="Task"/>.</returns>
    Task InvokeImageProxy(HttpContext context);

    /// <summary>
    /// Transcodes an image.
    /// </summary>
    /// <param name="filepath">Filepath to the image.</param>
    /// <param name="options"><see cref="ImageTranscodeOptions"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>String Path location to the image.</returns>
    Task<FileResult> TranscodeImageAsync(string filepath, ImageTranscodeOptions options, CancellationToken? cancellationToken = default);

    /// <summary>
    /// Transcodes an image.
    /// </summary>
    /// <param name="uri">URI to the image.</param>
    /// <param name="options"><see cref="ImageTranscodeOptions"/>.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
    /// <returns>String Path location to the image.</returns>
    Task<FileResult> TranscodeImageAsync(Uri uri, ImageTranscodeOptions options, CancellationToken? cancellationToken = default);
}