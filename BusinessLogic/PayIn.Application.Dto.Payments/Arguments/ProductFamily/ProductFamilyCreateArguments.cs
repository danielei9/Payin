using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ProductFamilyCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.productFamily.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name						{ get; set; }
		[Display(Name = "resources.productFamily.description")]
		public string Description				{ get; set; }
        public int? Id                           { get; set; }
        [Display(Name = "resources.productFamily.code")]
        public string Code { get; set; }

        #region Constructors
        public ProductFamilyCreateArguments(string name, string description,int? id, string code)
		{
			Name            =   name;
			Description     =   description ?? "";
            Id              =   id;
            Code            =   code ?? "";
        }
		#endregion Constructors
	}
}
