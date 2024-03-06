using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Home;
using HouseRentingSystem.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Core.Services.House
{
    public class HouseService : IHouseService
    {
        private readonly IRepository _repository;

        public HouseService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<HouseIndexServiceModel>> LastThreeHouses()
        {
            return await _repository
                .AllReadOnly<Infrastructure.Data.Models.House>()
                .OrderByDescending(h => h.Id)
                .Take(3)
                .Select(h => new HouseIndexServiceModel()
                {
                    Id = 3,
                    ImageUrl = h.ImageUrl,
                    Title = h.Title
                })
                .ToListAsync();
        }
    }
}
