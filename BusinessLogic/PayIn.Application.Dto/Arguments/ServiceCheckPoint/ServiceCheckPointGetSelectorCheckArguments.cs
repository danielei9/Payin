using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceCheckPoint
{
	public partial class ServiceCheckPointGetSelectorCheckArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int? ItemId { get; set; }

		#region Constructors
		public ServiceCheckPointGetSelectorCheckArguments(string filter, int? itemId)
		{
			Filter = filter ?? "";
			ItemId = itemId;
		}
		#endregion Constructors
	}
}

