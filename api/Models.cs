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
