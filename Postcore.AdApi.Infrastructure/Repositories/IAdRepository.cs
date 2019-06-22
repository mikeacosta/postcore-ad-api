using Postcore.AdApi.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Postcore.AdApi.Infrastructure.Repositories
{
    public interface IAdRepository
    {
        Task<IEnumerable<AdData>> GetAll();
        Task<AdData> Get(string id);
        Task<string> Add(AdData ad);
        Task Delete(AdData ad);
        Task<bool> CheckHealth();
    }
}
