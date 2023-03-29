// <copyright file="ImageSharpProxyHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.Processing;

namespace RetroGPT.Core;

/// <summary>
/// Image Sharp Proxy Handler.
/// </summary>
public class ImageSharpProxyHandler : IImageProxyHandler
{
    private HttpFileClient httpClient;
    private ImageTranscodeOptions defaultOptions;
    private string cachePath;
    private SixLabors.ImageSharp.Configuration imageConfig;

    public ImageSharpProxyHandler(HttpFileClient? httpClient = default, ImageTranscodeOptions? defaultOptions = null, string? defaultDownloadPath = default)
    {
        this.imageConfig = SixLabors.ImageSharp.Configuration.Default;
        this.imageConfig.MemoryAllocator = MemoryAllocator.Default;
        var downloadPath = defaultDownloadPath ?? "Images";
        this.httpClient = httpClient ?? new HttpFileClient(defaultDownloadPath: downloadPath);
        this.defaultOptions = defaultOptions ?? new ImageTranscodeOptions();
        this.cachePath = Path.Combine(this.httpClient.DefaultDownloadPath, "Cache");
        Directory.CreateDirectory(this.cachePath);
    }

    /// <inheritdoc/>
    public string DefaultDownloadPath => this.httpClient.DefaultDownloadPath;

    /// <inheritdoc/>
    public string DefaultCachePath => this.cachePath;

    public SixLabors.ImageSharp.Configuration ImageConfig => this.imageConfig;

    /// <inheritdoc/>
    public async Task InvokeImageProxy(HttpContext context)
    {
        try
        {
            var query = context.Request.Query;
            var path = Helpers.GetUrlFromPath(context.Request.Path);
            if (path is null)
            {
                return;
            }

            var imageTranscodeOptions = ImageTranscodeOptions.FromQueryString(query);
            var resultLocation = await this.TranscodeImageAsync(path, imageTranscodeOptions);
            var bvtes = File.ReadAllBytes(resultLocation.FilePath);
            await context.Response.Body.WriteAsync(bvtes, 0, bvtes.Length);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <inheritdoc/>
    public Task<FileResult> TranscodeImageAsync(string filepath, ImageTranscodeOptions options, CancellationToken? cancellationToken = null)
        => this.TranscodeImageAsync(new Uri(filepath), options, cancellationToken);

    /// <inheritdoc/>
    public async Task<FileResult> TranscodeImageAsync(Uri uri, ImageTranscodeOptions options, CancellationToken? cancellationToken = null)
    {
        var result = await Helpers.GenerateFilePath(this.httpClient, new Uri($"{uri}?{options.FullQueryString}"), options, options.Format.ToString(), options.UseCache, this.cachePath, null);

        using var contentStream = File.OpenRead(result.FilePath);
        using var transcodeImage = await Image.LoadAsync(this.imageConfig, contentStream);
        using var fileStream = new FileStream(result.GeneratedFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        if ((options.Width > 0 && options.Width <= transcodeImage.Width) || (options.Height > 0 && options.Height <= transcodeImage.Height))
        {
            transcodeImage.Mutate(x => x.Resize(options.Width, options.Height));
        }

        switch (options.Format)
        {
            case ImageFormat.Unknown:
                break;
            case ImageFormat.Bmp:
                await transcodeImage.SaveAsBmpAsync(fileStream);
                break;
            case ImageFormat.Jpg:
                await transcodeImage.SaveAsJpegAsync(fileStream);
                break;
            case ImageFormat.Gif:
                await transcodeImage.SaveAsGifAsync(fileStream);
                break;
            case ImageFormat.Png:
                await transcodeImage.SaveAsPngAsync(fileStream);
                break;
            default:
                break;
        }

        var md5 = Helpers.GenerateKey(result.GeneratedFilePath) ?? string.Empty;
        return new FileResult(result.GeneratedFilePath, md5);
    }
}