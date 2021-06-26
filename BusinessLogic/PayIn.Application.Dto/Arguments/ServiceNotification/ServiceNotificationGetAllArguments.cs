using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceNotificationGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public string Type { get; set; }
		public XpDate Since { get; set; }
		public XpDate Until { get; set; }

		#region Constructors
		public ServiceNotificationGetAllArguments(string filter, string type, XpDate since, XpDate until)
		{
			Filter = filter ?? "";
			Type = type ?? "";
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}
