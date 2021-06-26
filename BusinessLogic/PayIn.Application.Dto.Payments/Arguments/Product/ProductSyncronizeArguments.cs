using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ProductSyncronizeArguments : IArgumentsBase
	{
		public int PaymentConcessionId { get; set; }
		public IEnumerable<ProductSyncronizeArguments_Product> Products { get; set; }
	}
}
