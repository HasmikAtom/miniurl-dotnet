using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;


namespace Herxagon.MiniUrl.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class MiniUrlController : ControllerBase
{
    
    private readonly ILogger<MiniUrlController> _logger;
    private readonly string? _domain;
    private readonly RedisActions _redis;
    
    
    public MiniUrlController(IConfiguration configuration, ILogger<MiniUrlController> logger, IConnectionMultiplexer multiplexer)
    {
        _logger = logger;
        _domain = configuration["Domain"];
        _redis = new RedisActions(multiplexer.GetDatabase(0), multiplexer.GetDatabase(1));
    }

    
    [HttpPost(Name = "Minify")]
    public async Task<MinifyResponse> Post([FromBody] MinifyRequest body)
    {
        
        // TODO: add rate limiting
        // TODO: rate limiting should be based on the IP address
        // TODO: rate limiting should be x number of attempts every x amount of time
        // TODO: rate limiting time should be reset after x amount of time
        
        // TODO: add validation for the url
        // TODO: prevent shortening of this app's url (localhost, etc)
        
        // TODO: lookup redis async/await
        
        string urlId = Guid.NewGuid().ToString();
        urlId = urlId.Substring(0, 8);
        
        await _redis.Store( urlId, body.URL); 

        var res = new MinifyResponse()
        {
            MinifiedURL = _domain + "/miniurl/" + urlId,
            Message = "URL Minified"
        };
        
        return res;
    }
    
    [HttpGet("{shortUrl}", Name = "Resolve")]
    public async Task<RedirectResult> Get([FromRoute] ResolveRequest req)
    {
        // TODO: rate limiting, decrease api usage counter for the specific IP after every call
        var fullUrl = await _redis.Get(req.ShortURL);
        Console.Write(fullUrl);
        // TODO: handle cases like urlid doesnt exist
        return Redirect(fullUrl);
    }
    
}