using PayIn.Domain.Payments;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ProductFamilyUpdateArguments : IUpdateArgumentsBase<ProductFamily>
	{
		public int Id                       { get; set; }
		[Display(Name = "resources.productFamily.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name					{ get; set; }
		[Display(Name = "resources.productFamily.description")]

		public string Description				{ get; set; }

        [Display(Name = "resources.productFamily.parentId")]
        public int? ParentId                { get; set; }

		[Display(Name = "resources.productFamily.isVisible")]
		public bool IsVisible { get; set; }

        [Display(Name = "resources.productFamily.code")]
        public string Code { get; set; }

        #region Constructors
        public ProductFamilyUpdateArguments(string name, string description, int? parentId, bool isVisible, string code)
		{
			Name = name;
			Description = description ?? "";
            ParentId = parentId;
            Code = code ?? "";
            IsVisible = isVisible;
		}
		#endregion Constructors
	}
}
