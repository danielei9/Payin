using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServicePrice
{
	public partial class ServicePriceGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int?   ZoneId { get; set; }

		//#region Cast from TicketCreateOraArguments
		//public static implicit operator ServicePriceGetAllArguments(TicketCreateOraArguments arguments)
		//{
		//	return new ServicePriceGetAllArguments
		//	{
		//		ZoneId = arguments.ZoneId
		//	};
		//}
		//#endregion Cast from TicketCreateOraArguments

		#region Constructors
		public ServicePriceGetAllArguments(string filter, int? zoneId)
		{
			Filter = filter ?? "";
			ZoneId = zoneId;
		}
		public ServicePriceGetAllArguments()
		{
		}
		#endregion Constructors
	}
}
