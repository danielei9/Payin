using PayIn.Domain.Payments;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ProductIsVisibleArguments : IUpdateArgumentsBase<Product>
	{
		public int  Id        { get; set; }
		public bool IsVisible { get; set; }

		#region Constructors
		public ProductIsVisibleArguments(bool isVisible)
		{
			IsVisible = isVisible;
		}
		#endregion Constructors
	}
}
