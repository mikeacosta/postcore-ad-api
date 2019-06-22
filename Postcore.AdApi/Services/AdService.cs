using AutoMapper;
using Postcore.AdApi.Infrastructure.Models;
using Postcore.AdApi.Infrastructure.Repositories;
using Postcore.AdApi.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Postcore.AdApi.Services
{
    public class AdService : IAdService
    {
        private readonly IAdRepository _repo;
        private readonly IMapper _mapper;

        public AdService(IAdRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AdDto>> GetAll()
        {
            var ads = await _repo.GetAll();
            return _mapper.Map<List<AdDto>>(ads);
        }

        public async Task<string> Add(AdDto dto)
        {
            var dbModel = _mapper.Map<AdData>(dto);

            dbModel.Id = Guid.NewGuid().ToString();
            dbModel.CreationDateTime = DateTime.UtcNow;
            dbModel.Status = AdDataStatus.Pending;

            await _repo.Add(dbModel);

            return dbModel.Id;
        }

        public async Task Confirm(ConfirmAdDto dto)
        {
            var ad = await _repo.Get(dto.Id);
            if (ad == null)
                throw new KeyNotFoundException($"A record with ID={dto.Id} was not found.");

            if (dto.Status != AdStatus.Active)
            {
                await _repo.Delete(ad);
                return;
            }

            ad.FilePath = dto.FilePath;
            ad.Status = AdDataStatus.Active;
            await _repo.Add(ad);
        }
    }
}
