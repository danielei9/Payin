using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PublicCampaignGetByUserArguments : IArgumentsBase
	{
		[Required] public XpDateTime Now { get; set; }
		[Required] public string Login { get; set; }
		[Required] public int? EventId { get; set; }
		           public string Filter { get; set; }

		#region Constructors
		public PublicCampaignGetByUserArguments(
			string login,
			int eventId,
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
			Login = login;
			EventId = eventId;
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
