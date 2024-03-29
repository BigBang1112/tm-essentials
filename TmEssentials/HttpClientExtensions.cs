﻿#if NET6_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER

using System.Net.Http;

namespace TmEssentials;

/// <summary>
/// Provides extension methods for <see cref="HttpClient"/> to enhance its functionality.
/// This includes a method for sending a HEAD request asynchronously.
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Send a HEAD request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="http">HttpClient.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<HttpResponseMessage> HeadAsync(this HttpClient http, string? requestUri, CancellationToken cancellationToken = default)
    {
        var head = new HttpRequestMessage(HttpMethod.Head, requestUri);
        return await http.SendAsync(head, cancellationToken);
    }

    /// <summary>
    /// Send a HEAD request to the specified Uri as an asynchronous operation.
    /// </summary>
    /// <param name="http">HttpClient.</param>
    /// <param name="requestUri">The Uri the request is sent to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<HttpResponseMessage> HeadAsync(this HttpClient http, Uri? requestUri, CancellationToken cancellationToken = default)
    {
        var head = new HttpRequestMessage(HttpMethod.Head, requestUri);
        return await http.SendAsync(head, cancellationToken);
    }
}

#endif
