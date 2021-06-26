using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PublicCampaignGetAllWithLinesArguments : IArgumentsBase
	{
		[Required] public XpDateTime Now { get; set; }
		[Required] public int? EventId { get; set; }
		           public string Filter { get; set; }

		#region Constructors
		public PublicCampaignGetAllWithLinesArguments(
			int? eventId,
			string filter
#if DEBUG
			,XpDateTime now
#endif //DEBUG
		)
		{
#if DEBUG
			Now = now ?? DateTime.UtcNow;
#else //DEBUG
			Now = DateTime.UtcNow;
#endif //DEBUG
			EventId = eventId;
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
