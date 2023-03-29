// <copyright file="FileResult.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace RetroGPT.Core;

public class FileResult
{
    public FileResult(string? filePath, string? md5)
    {
        this.FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        this.Md5 = md5 ?? throw new ArgumentNullException(nameof(md5));
    }

    public int Id { get; }

    public string FilePath { get; }

    public string Md5 { get; }
}