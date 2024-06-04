using Herxagon.MiniUrl.Api.Services;
using Microsoft.AspNetCore.Mvc;



namespace Herxagon.MiniUrl.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class MiniUrlController : ControllerBase
{
    
    private readonly ILogger<MiniUrlController> _logger;
    private readonly IStorageService _redis;
    private readonly string? _domain;
    private readonly TimeSpan? _urlExpiration;
    
    
    public MiniUrlController(IConfiguration configuration, ILogger<MiniUrlController> logger, IStorageService redis)
    {
        _logger = logger;
        _redis = redis;
        _domain = configuration["Domain"];
        _urlExpiration = configuration.GetValue<TimeSpan>("UrlExpiration");
    }

    
    [HttpPost(Name = "Minify")]
    public async Task<ActionResult> Post([FromBody] MinifyRequest body)
    {
        // TODO: add rate limiting
        // TODO: rate limiting should be based on the IP address
        // TODO: rate limiting should be x number of attempts every x amount of time
        // TODO: rate limiting time should be reset after x amount of time
        
        // TODO: add validation for the url
        // TODO: prevent shortening of this app's url (localhost, etc)
        
        // TODO: lookup redis async/await
        
        var url = await _redis.Store(body.URL, _urlExpiration); 

        var res = new MinifyResponse() 
        {
            MinifiedURL = _domain + url,
            Message = "URL Minified"
        };

        return Ok(res);
    }
    
    [HttpGet("{shortUrl}", Name = "Resolve")]
    public async Task<ActionResult> Get([FromRoute] ResolveRequest req)
    {
        // TODO: rate limiting, decrease api usage counter for the specific IP after every call
        var fullUrl = await _redis.Get(req.ShortURL);
        Console.Write(fullUrl);
        // TODO: handle cases like urlid doesnt exist
        return Redirect(fullUrl);
    }
    
}