using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceCheckPoint
{
	public partial class ServiceCheckPointGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int? ItemId { get; set; }

		#region Constructors
		public ServiceCheckPointGetSelectorArguments(string filter, int? itemId)
		{
			Filter = filter ?? "";
			ItemId = itemId;
		}
		#endregion Constructors
	}
}

