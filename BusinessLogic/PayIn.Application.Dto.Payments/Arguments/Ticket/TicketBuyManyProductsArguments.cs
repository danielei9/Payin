using System.Collections.Generic;
using System.Linq;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class TicketBuyManyProductsArguments : IArgumentsBase
    {
		public int ConcessionId { get; set; }
		public int														ServiceCardId	{ get; set; }
        public IEnumerable<TicketBuyManyProductsArguments_Product>		Products		{ get; set; }

        #region Constructors
        public TicketBuyManyProductsArguments(int concessionId, int serviceCardId, IEnumerable<TicketBuyManyProductsArguments_Product> products)
		{
			ConcessionId = concessionId;
            ServiceCardId = serviceCardId;
			Products = products;
		}
		#endregion Constructors
	}
}