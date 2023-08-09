using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("Set")]
        public async Task<IActionResult> Set(string cachedValue,string cachedValueBinary)
        {
            await _distributedCache.SetStringAsync("redisCachedValue", cachedValue, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
            await _distributedCache.SetAsync("redisCachedBinaryValue", Encoding.UTF8.GetBytes(cachedValueBinary), options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cachedValue  = await _distributedCache.GetStringAsync("redisCachedValue");
            var cachedValueBinary = await _distributedCache.GetAsync("redisCachedBinaryValue");
            var cachedValue2 = Encoding.UTF8.GetString(cachedValueBinary);
            return Ok(new
            {
                cachedValue,
                cachedValue2
            });
        }
    }
}
