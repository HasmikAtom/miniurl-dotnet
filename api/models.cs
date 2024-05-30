using System.Text.Json;
using StackExchange.Redis;

namespace Herxagon.MiniUrl.Api;

using Microsoft.AspNetCore.Mvc;


public class MinifyRequest
{
    public string URL { get; set; }
}
public class MinifyResponse
{
    public string MinifiedURL { get; set; }
    public string Message { get; set; }
}
public class ResolveRequest
{
    [FromRoute(Name = "shortUrl")] // wouldnt work without this
    public string ShortURL { get; set; }
}


public class RedisActions
{
    private  readonly IDatabase _redis0;
    private  readonly IDatabase _redis1;

    public RedisActions(IDatabase redis0, IDatabase redis1)
    {
        _redis0 = redis0;
        _redis1 = redis1;
    }
    public async Task<bool> Store( string urlId, string url)
    {
        return await _redis0.StringSetAsync(urlId, url).ConfigureAwait(false);
    }

    public async Task<string> Get( string urlId)
    {
         return await _redis0.StringGetAsync(urlId).ConfigureAwait(false);
    }
}