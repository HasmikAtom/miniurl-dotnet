using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using NRedisStack;
using NRedisStack.RedisStackCommands;


namespace Herxagon.MiniUrl.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class MiniUrlController : ControllerBase
{
    
    private readonly ILogger<MiniUrlController> _logger;
    private readonly IConfiguration _config;
    private readonly string _domain;
    private readonly IDatabase _redis0; // shortened url store
    private readonly IDatabase _redis1; // rate limiting management store
    
    
    public MiniUrlController(IConfiguration configuration, ILogger<MiniUrlController> logger, IConnectionMultiplexer multiplexer)
    {

        _config = configuration;
        _logger = logger;
        _domain = _config["Domain"];
        _redis0 = multiplexer.GetDatabase(0);
        _redis1 = multiplexer.GetDatabase(1);
    }

    
    [HttpPost(Name = "Minify")]
    public ActionResult<MinifyResponse> Post([FromBody] MinifyRequest body)
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
        
        _redis0.StringSet(urlId, body.URL);
        var res = new MinifyResponse()
        {
            MinifiedURL = _domain + "/miniurl/" + urlId,
            Message = "URL Minified"
        };
        
        return res;
    }
    
    [HttpGet("{shortUrl}", Name = "Resolve")]
    public ActionResult Get([FromRoute] ResolveRequest req)
    {
        // TODO: rate limiting, decrease api usage counter for the specific IP after every call
        
        var fullUrl = _redis0.StringGet(req.ShortURL).ToString();
        // TODO: handle cases like urlid doesnt exist
        return Redirect(fullUrl);
    }
    
}