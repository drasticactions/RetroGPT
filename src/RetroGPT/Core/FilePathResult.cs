// <copyright file="FilePathResult.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace RetroGPT.Core;

public class FilePathResult
{
    public FilePathResult(string filepath, string generatedFilePath, string generatedFilename, string md5)
    {
        this.FilePath = filepath;
        this.GeneratedFilePath = generatedFilePath;
        this.GeneratedFileName = generatedFilename;
        this.Md5 = md5;
    }

    public string FilePath { get; }

    public string GeneratedFilePath { get; }

    public string GeneratedFileName { get; }

    public string Md5 { get; }
}