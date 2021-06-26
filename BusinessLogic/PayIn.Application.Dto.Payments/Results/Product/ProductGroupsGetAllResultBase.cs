using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class ProductGroupsGetAllResultBase : ResultBase<ProductGroupsGetAllResult>
	{
		public string ProductName						{ get; set; }
    }
}
