using HouseRentingSystem.Attributes;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace HouseRentingSystem.Controllers
{
    public class HouseController : BaseController
    {
        private readonly IHouseService _houseService;
        private readonly IAgentService _agentService;

        public HouseController(
            IHouseService houseService,
            IAgentService agentService)
        {
            _houseService = houseService;
            _agentService = agentService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> All([FromQuery]AllHousesQueryModel model)
        {
            var houses = await _houseService.AllAsync(
                model.Category,
                model.SearchTerm,
                model.Sorting,
                model.CurrentPage,
                model.HousesPerPage);

            model.TotalHousesCount = houses.TotalHousesCount;
            model.Houses = houses.Houses;
            model.Categories = await _houseService.AllCategoriesNamesAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var userId = User.Id();
            IEnumerable<HouseServiceModel> model;

            if (await _agentService.ExistsByIdAsync(userId))
            {
                int agentId = await _agentService.GetAgentIdAsync(userId) ?? 0;
                model = await _houseService.AllHousesByAgentId(agentId);
            }
            else
            {
                model = await _houseService.AllHousesByUsertId(userId);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (await _houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }

            var model = await _houseService.HouseDetailsByIdAsync(id);

            return View(model);
        }

        [HttpGet]
        [MustBeAgent]
        public async Task<IActionResult> Add()
        {
            var model = new HouseFormModel()
            {
                Categories = await _houseService.AllCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        [MustBeAgent]
        public async Task<IActionResult> Add(HouseFormModel model)
        {
            if (await _houseService.CategoryExistsAsync(model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "");
            }

            if (ModelState.IsValid == false)
            {
                model.Categories = await _houseService.AllCategoriesAsync();

                return View(model);
            }

            int? agentId = await _agentService.GetAgentIdAsync(User.Id());

            int newHouseId = await _houseService.CreateAsync(model, agentId ?? 0);

            return RedirectToAction(nameof(Details), new { id = newHouseId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = new HouseFormModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(HouseFormModel model)
        {
            return RedirectToAction(nameof(Details), new { id = 1 });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = new HouseDetailsViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HouseDetailsViewModel model)
        {
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            return RedirectToAction(nameof(Mine));
        }
    }
}
