using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Queries
{
	public partial class PaymentMediaGetAllQuery : IArgumentsBase
	{
		public string Filter { get; set; }

		public PaymentMediaGetAllQuery(string filter)
		{
			Filter = filter;
		}
	}
}
