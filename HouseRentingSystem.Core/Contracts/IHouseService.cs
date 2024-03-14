using HouseRentingSystem.Core.Enumerations;
using HouseRentingSystem.Core.Models.Home;
using HouseRentingSystem.Core.Models.House;

namespace HouseRentingSystem.Core.Contracts
{
    public interface IHouseService
    {
        Task<IEnumerable<HouseIndexServiceModel>> LastThreeHousesAsync();

        Task<IEnumerable<HouseCategoryServiceModel>> AllCategoriesAsync();

        Task<bool> CategoryExistsAsync(int cateogryId);

        Task<int> CreateAsync(HouseFormModel model, int agentId);

        Task<HouseQueryServiceModel> AllAsync(
            string? category = null,
            string? searchTerm = null,
            HouseSorting sorting = HouseSorting.Newest,
            int currentPage = 1,
            int housesPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNamesAsync();

        Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int agentId);
        Task<IEnumerable<HouseServiceModel>> AllHousesByUsertId(string userId);
        Task<bool> ExistsAsync(int id);
        Task<HouseDetailsServiceModel> HouseDetailsByIdAsync(int id);
        Task EditAsync(int houseId, HouseFormModel model);
        Task<bool> HasAgentWithIdAsync(int houseId, string userId);
        Task<HouseFormModel?> GetHouseFormModelByIdAsync(int id);
    }
}
