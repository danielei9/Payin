using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceFreeDays
{
	public partial class ServiceFreeDaysGetAllArguments : IArgumentsBase
	{
		public string Filter       { get; set; }
		public int    ConcessionId { get; set; }

		#region Constructors
		public ServiceFreeDaysGetAllArguments(string filter, int concessionId)
		{
			Filter = filter ?? "";
			ConcessionId = concessionId;
		}
		#endregion Constructors
	}
}
