// <copyright file="HttpFileClient.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Reflection;

namespace RetroGPT.Core;

/// <summary>
    /// Http File Client.
    /// </summary>
    public class HttpFileClient : IHttpFileClient
    {
        private const string Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        private const string Language = "ja,en-US;q=0.8,en;q=0.6";
        private const string Encoding = "gzip, deflate, sdch";
        private const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2593.0 Safari/537.36";

        private HttpClient httpClient;
        private ILogger<HttpFileClient>? logger;
        private string defaultDownloadPath;

        public HttpFileClient(HttpClient? client = default, ILogger<HttpFileClient>? logger = default, string? defaultDownloadPath = default)
        {
            this.httpClient = client ?? this.GenerateClient();
            this.logger = logger ?? default;
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            this.defaultDownloadPath = Path.Combine(path, defaultDownloadPath ?? "Downloads");
            Directory.CreateDirectory(this.defaultDownloadPath);
        }

        /// <inheritdoc/>
        public string DefaultDownloadPath => this.defaultDownloadPath;

        /// <inheritdoc/>
        public async Task<string> DownloadFile(Uri uri, string? downloadPath = default, string? filename = default, bool useCache = true)
        {
            ArgumentNullException.ThrowIfNull(uri, nameof(uri));
            this.logger?.LogInformation($"Downloading File from {uri}.");

            downloadPath = downloadPath ?? this.defaultDownloadPath;
            filename = filename ?? Path.GetFileName(uri.AbsolutePath);
            var fullpath = Path.Combine(downloadPath, filename);
            var fileInfo = new FileInfo(fullpath);
            if (fileInfo.Exists && useCache)
            {
                // If we already have the file in our cache, return that instead.
                return fileInfo.FullName;
            }

            var response = await this.httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            await using var ms = await response.Content.ReadAsStreamAsync();
            await using var fs = File.Create(fileInfo.FullName);
            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(fs);
            this.logger?.LogInformation($"File saved as [{fileInfo.Name}].");

            return fileInfo.FullName;
        }

        /// <inheritdoc/>
        public Task<string> UploadFile(string filePath, MultipartFormDataContent? dataForm = null)
        {
            throw new NotImplementedException();
        }

        private HttpClient GenerateClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", Accept);
            client.DefaultRequestHeaders.Add("Accept-Language", Language);
            client.DefaultRequestHeaders.Add("Accept-Encoding", Encoding);
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            return client;
        }
    }