using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using static HouseRentingSystem.Core.Constants.MessageConstants;
using static HouseRentingSystem.Infrastructure.Constants.DataConstants;

namespace HouseRentingSystem.Core.Models.House
{
    public class HouseFormModel
    {
        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(HouseTitleMaxLength,
            MinimumLength = HouseTitleMinLength,
            ErrorMessage = LengthMessage)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(HouseAddressMaxLength,
           MinimumLength = HouseAddressMinLength,
           ErrorMessage = LengthMessage)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = RequiredMessage)]
        [StringLength(HouseDescriptionMaxLength,
           MinimumLength = HouseDescriptionMinLength,
           ErrorMessage = LengthMessage)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Range(typeof(decimal), HouseRentingPriceMinimum, HouseRentingPriceMaximum,
            ErrorMessage = "Price per month must be a positive number and less than {2} leva.")]
        [Display(Name = "Price per month")]
        public decimal PricePerMonth { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<HouseCategoryServiceModel> Categories { get; set; } = new List<HouseCategoryServiceModel>();
    }
}
