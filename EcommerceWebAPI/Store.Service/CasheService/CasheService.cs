using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Service.CasheService
{
    public class CasheService : ICasheService
    {
        private readonly IDatabase _database;

        public CasheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<string> GetCasheResponeAsync(string key)
        {
            var cashedRespone=await _database.StringGetAsync(key);
            if (cashedRespone.IsNullOrEmpty)
                return null;
            return cashedRespone.ToString();
        }

        public async Task SetCasheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if (response is null)
                return;
            var options=new JsonSerializerOptions {PropertyNamingPolicy=JsonNamingPolicy.CamelCase};
            var serialzedResponse=JsonSerializer.Serialize(response, options);
            await _database.StringSetAsync(key, serialzedResponse, timeToLive);
        }
    }
}
