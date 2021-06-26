using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTrack
{
	public partial class ControlTrackPublicGetByRangeArguments : IArgumentsBase
	{
		           public int?       WorkerId { get; private set; }
		           public int?       ItemId   { get; private set; }
		[Required] public XpDateTime Since    { get; private set; }
		[Required] public XpDateTime Until    { get; private set; }

		#region Constructors
		public ControlTrackPublicGetByRangeArguments(int? workerId, int? itemId, XpDateTime since, XpDateTime until)
		{
			if (since == null)
				throw new ArgumentNullException("since");
			if (until == null)
				throw new ArgumentNullException("until");

			WorkerId = workerId;
			ItemId = itemId;
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}
