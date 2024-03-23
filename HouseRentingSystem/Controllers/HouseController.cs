using HouseRentingSystem.Attributes;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Exceptions;
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
        private readonly ILogger _logger;

        public HouseController(
            IHouseService houseService,
            IAgentService agentService,
            ILogger<HouseController> logger)
        {
            _houseService = houseService;
            _agentService = agentService;
            _logger = logger;

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
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist!");
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
            if (await _houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }

            if (await _houseService.HasAgentWithIdAsync(id, User.Id()) == false)
            {
                return Unauthorized();
            }

            var model = await _houseService.GetHouseFormModelByIdAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseFormModel model)
        {
            if (await _houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }

            if (await _houseService.HasAgentWithIdAsync(id, User.Id()) == false)
            {
                return Unauthorized();
            }

            if (await _houseService.CategoryExistsAsync(model.CategoryId) == false)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist!");
            }

            if (ModelState.IsValid == false)
            {
                model.Categories = await _houseService.AllCategoriesAsync();

                return View(model);
            }

            await _houseService.EditAsync(id, model);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }

            if (await _houseService.HasAgentWithIdAsync(id, User.Id()) == false)
            {
                return Unauthorized();
            }

            var house = await _houseService.HouseDetailsByIdAsync(id);

            var model = new HouseDetailsViewModel()
            {
                Id = house.Id,
                Address = house.Address,
                ImageUrl = house.ImageUrl,
                Title = house.Title
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HouseDetailsViewModel model)
        {
            if (await _houseService.ExistsAsync(model.Id) == false)
            {
                return BadRequest();
            }

            if (await _houseService.HasAgentWithIdAsync(model.Id, User.Id()) == false)
            {
                return Unauthorized();
            }

            await _houseService.DeleteAsync(model.Id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            if (await _houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }

            if (await _agentService.ExistsByIdAsync(User.Id()))
            {
                return Unauthorized();
            }

            if (await _houseService.IsRentedAsync(id))
            {
                return BadRequest();
            }

            await _houseService.RentAsync(id, User.Id());

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            if (await _houseService.ExistsAsync(id) == false)
            {
                return BadRequest();
            }

            try
            {
                await _houseService.LeaveAsync(id, User.Id());
            }
            catch (UnauthorizedActionException uae)
            {
                _logger.LogError(uae, "HouseController/leave");

                return Unauthorized();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
