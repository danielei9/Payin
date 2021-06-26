using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ProductCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.product.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name						{ get; set; }
		[Display(Name = "resources.product.description")]
		public string Description				{ get; set; }
        [Display(Name = "resources.product.code")]
        public string Code { get; set; }
        [Display(Name = "resources.product.price")]
		[DataType(DataType.Currency)]
		public decimal? Price						{ get; set; }

        [Display(Name = "resources.product.price")]
        public int? FamilyId						{ get; set; }

		[Display(Name = "resources.product.isVisible")]
		public bool IsVisible { get; set; }
		[Display(Name = "resources.product.visibility")]
		public ProductVisibility Visibility { get; set; }

		[Display(Name = "resources.product.toConsult")]
		public bool ToConsult { get; set; }

		#region Constructors
		public ProductCreateArguments(string name, string code, string description, decimal? price, int? familyId, ProductVisibility visibility, bool toConsult)
		{
			Name = name;
            Code = code ?? "";
            Description = description ?? "";
			Price = price;
			FamilyId = familyId;
			Visibility = visibility;
			ToConsult = toConsult;
		}
		#endregion Constructors
	}
}
