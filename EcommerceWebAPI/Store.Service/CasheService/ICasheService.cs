using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.CasheService
{
    public  interface ICasheService
    {
        Task SetCasheResponseAsync(string key, object response, TimeSpan timeToLive);
        Task<string> GetCasheResponeAsync(string key);
    }
}
