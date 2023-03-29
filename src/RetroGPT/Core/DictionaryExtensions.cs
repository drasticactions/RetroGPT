// <copyright file="DictionaryExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Web;

namespace RetroGPT.Core;

public static class DictionaryExtensions
{
    public static string ToQueryString(this Dictionary<string, string> dictionary)
    {
        var queryString = string.Join("&", dictionary.Select(x => $"{HttpUtility.UrlEncode(x.Key)}={HttpUtility.UrlEncode(x.Value)}"));
        return queryString;
    }
}