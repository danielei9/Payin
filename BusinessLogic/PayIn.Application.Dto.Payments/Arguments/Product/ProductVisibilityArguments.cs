using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ProductVisibilityArguments : IArgumentsBase
	{
															public int					Id				{ get; set; }
		[Display(Name = "resources.product.visibility")]	public ProductVisibility	Visibility		{ get; set; }

		#region Constructors
		public ProductVisibilityArguments(int id, ProductVisibility visibility)
		{
			Id = id;
			Visibility = visibility;
		}
		#endregion Constructors
	}
}
