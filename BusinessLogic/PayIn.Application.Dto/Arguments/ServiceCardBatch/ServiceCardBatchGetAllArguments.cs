using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardBatchGetAllArguments : IArgumentsBase
	{
		public int? SystemCardId { get; set; }
		public string Filter { get; set; }

		#region Constructors
		public ServiceCardBatchGetAllArguments(int? systemCardId, string filter)
		{
			SystemCardId = systemCardId;
			Filter = filter ?? "";
		}
		#endregion Constructors 
	}
}
