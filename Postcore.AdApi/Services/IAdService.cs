using Postcore.AdApi.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Postcore.AdApi.Services
{
    public interface IAdService
    {
        Task<IEnumerable<AdDto>> GetAll();
        Task<string> Add(AdDto dto);
        Task Confirm(ConfirmAdDto dto);
    }
}
