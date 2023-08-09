using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public IMemoryCache _memoryCache;

        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("set/{cachedValue}")]
        public void SetCache(string cachedValue)
        {
            _memoryCache.Set("cached", cachedValue);
        }

        [HttpGet]
        public string GetCache()
        {
            if (_memoryCache.TryGetValue<string>("cached", out string name))
            {
                return name;
            }
            return "";
            //  return _memoryCache.Get<string>("cached");
        }

        //Absolute time: Cache'deki datanın ne kadar tutulacağına dair net ömrünün belirtilmesidir. Belirtilen süre sona erdiğinde cache temizlenir.
        //Sliding Time:  Cache'deki datanın memoryde belirtilen süre bpyunda tutulmasını belirtir. Belirtilen süre peridyoru içerisinde cache'e yapılan erişim neticesinde de datanın ömrü bir o kadar uzatılacaktır. Belirlenen süre içerisinde dataya erişilmezse cache temizlenir.
        //Absolute time ve Sliding Time aynı anda kullanılabilir.

        [HttpGet("setDateValueCache")]
        public void SetDateValueCache()
        {
            _memoryCache.Set<DateTime>("date", DateTime.Now,options: new()
            {
                AbsoluteExpiration= DateTime.Now.AddSeconds(30),
                SlidingExpiration= TimeSpan.FromSeconds(5)
            });
        }

        [HttpGet]
        public DateTime GetDateValueCache() 
        {
            return _memoryCache.Get<DateTime>("date");
        }

    }
}
