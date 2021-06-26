using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public partial class TransportOperationGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		[Required] public XpDate Since { get; set; }
		[Required] public XpDate Until { get; set; }

		#region Constructors
		public TransportOperationGetAllArguments(string filter, XpDate since, XpDate until)
		{
			Filter = filter ?? "";
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}