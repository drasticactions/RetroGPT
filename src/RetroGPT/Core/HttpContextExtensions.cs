// <copyright file="HttpContextExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Globalization;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;

namespace RetroGPT.Core;

/// <summary>
/// HttpContext Extension Methods.
/// </summary>
public static class HttpContextExtensions
{
    public static async Task WriteContentsWithEncodingAsync(this HttpContext context, string content)
    {
        var culture = GetCulutreInfoViaHeaders(context);
        await context.Response.WriteAsync(content, System.Text.Encoding.GetEncoding(culture.TextInfo.OEMCodePage));
    }

    public static CultureInfo GetCulutreInfoViaHeaders(this HttpContext context)
    {
        StringValues ua = string.Empty;
        StringValues lang = string.Empty;

        context.Request.Headers.TryGetValue("User-Agent", out ua);
        context.Request.Headers.TryGetValue("Accept-Language", out lang);
        IOrderedEnumerable<StringWithQualityHeaderValue> languages = lang.ToString().Split(',')
            .Select(StringWithQualityHeaderValue.Parse)
            .OrderByDescending(s => s.Quality.GetValueOrDefault(1));

        return GetCultureInfoViaAcceptLanguage(lang.ToString());
    }

    public static CultureInfo GetCultureInfoViaAcceptLanguage(string acceptLanguage)
    {
        if (string.IsNullOrEmpty(acceptLanguage))
        {
            return CultureInfo.InvariantCulture;
        }

        var lang = GetSupportedLanguages(acceptLanguage).FirstOrDefault();
        if (lang == null)
        {
            return CultureInfo.InvariantCulture;
        }

        var culture = CultureInfo.GetCultureInfo(lang.Value);
        return culture;
    }

    public static IOrderedEnumerable<StringWithQualityHeaderValue> GetSupportedLanguages(string acceptLanguage)
    {
        return acceptLanguage.ToString().Split(',')
            .Select(StringWithQualityHeaderValue.Parse)
            .OrderByDescending(s => s.Quality.GetValueOrDefault(1));
    }
}