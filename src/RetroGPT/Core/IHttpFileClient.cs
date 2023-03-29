// <copyright file="IHttpFileClient.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace RetroGPT.Core;

public interface IHttpFileClient
{
    Task<string> UploadFile(string filePath, MultipartFormDataContent? dataForm = default);

    Task<string> DownloadFile(Uri uri, string? downloadPath, string? filename, bool useCache = true);

    string DefaultDownloadPath { get; }
}