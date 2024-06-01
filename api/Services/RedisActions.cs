using StackExchange.Redis;

namespace Herxagon.MiniUrl.Api.Services;

public class RedisActions
{
    private  readonly IDatabase _redis0;
    private  readonly IDatabase _redis1;

    public RedisActions(IDatabase redis0, IDatabase redis1)
    {
        _redis0 = redis0;
        _redis1 = redis1;
    }
    public async Task<string> Store(string url)
    {
        string urlId = Guid.NewGuid().ToString();
        urlId = urlId.Substring(0, 8);
        
        await _redis0.StringSetAsync(urlId, url).ConfigureAwait(false);

        return "/miniurl/" + urlId;
    }

    public async Task<string> Get( string urlId)
    {
        return await _redis0.StringGetAsync(urlId).ConfigureAwait(false);
    }
}