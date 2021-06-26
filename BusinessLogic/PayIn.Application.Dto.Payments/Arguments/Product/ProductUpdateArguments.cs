using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ProductUpdateArguments : IArgumentsBase
	{
		public int Id { get; set; }

		[Display(Name = "resources.product.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Display(Name = "resources.product.description")]
		public string Description { get; set; }

        [Display(Name = "resources.product.code")]
        public string Code { get; set; }

        [Display(Name = "resources.product.photo")]
		public string PhotoUrl { get; set; }

		[Display(Name = "resources.product.price")]
		[DataType(DataType.Currency)]
		public decimal? Price { get; set; }

		[Display(Name = "resources.product.family")]
		public int? FamilyId { get; set; }

		[Display(Name = "resources.product.isVisible")]
		public bool IsVisible { get; set; }

		[Display(Name = "resources.product.visibility")]
		public ProductVisibility Visibility { get; set; }

		[Display(Name = "resources.product.toConsult")]
		public bool ToConsult { get; set; }

		[Display(Name = "resources.product.sellableInTpv")]
		public bool SellableInTpv  { get; set; }

		[Display(Name = "resources.product.sellableInWeb")]
		public bool SellableInWeb { get; set; }

		#region Constructors
		public ProductUpdateArguments(int id, string name, string description, 
            string photoUrl, decimal? price, int? familyId, bool isVisible,
            string code, ProductVisibility visibility, bool toConsult, bool sellableInTpv,  bool sellableInWeb)
		{
			Id = id;
			Name = name;
            Code = code ?? "";
            Description = description ?? "";
			PhotoUrl = photoUrl ?? "";
			Price = price;
			FamilyId = familyId;
			IsVisible = isVisible;
			Visibility = visibility;
			ToConsult = toConsult;
			SellableInTpv = sellableInTpv;
			SellableInWeb = sellableInWeb;
		}
		#endregion Constructors
	}
}
