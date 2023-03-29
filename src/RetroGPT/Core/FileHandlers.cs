// <copyright file="FileHandlers.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace RetroGPT.Core;

public class FileHandlers
{
    public async Task Images(HttpContext context)
    {
        var splitPath = context.Request.Path.Value?.Split('/') ?? Array.Empty<string>();
        var path = System.IO.Path.Combine(splitPath);
        var filePath = Helpers.GetLocalFileViaSites(SiteSetup.Sites, path);
        if (string.IsNullOrEmpty(filePath))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
        else
        {
            await context.Response.SendFileAsync(filePath);
        }
    }
}