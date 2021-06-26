using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportValidation
{
	public partial class TransportValidationGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		[Required]
		public XpDate Since { get; set; }
		[Required]
		public XpDate Until { get; set; }
		public int? ConcessionId { get; set; }

		#region Constructors
		public TransportValidationGetAllArguments(string filter, XpDate since, XpDate until, int? concessionId)
		{
			Filter = filter ?? "";
			Since = since;
			Until = until;
			ConcessionId = concessionId;
		}
		#endregion Constructors
	}
}