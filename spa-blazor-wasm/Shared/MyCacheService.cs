using Microsoft.Extensions.Caching.Memory;

namespace BlazorWasm.Shared
{
    public class MyCacheService
    {
        private readonly IMemoryCache _cache;

        public MyCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void SetCacheValue(string key, object value, MemoryCacheEntryOptions options = null)
        {
            _cache.Set(key, value, options ?? new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) });
        }

        public bool TryGetCacheValue<T>(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }
    }
}
