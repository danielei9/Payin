using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTrack
{
	public partial class ControlTrackPublicGetByDayArguments : IArgumentsBase
	{
		           public int?   WorkerId { get; private set; }
		           public int?   ItemId   { get; private set; }
		[Required] public XpDate Date     { get; private set; }

		#region Constructors
		public ControlTrackPublicGetByDayArguments(int? workerId, int? itemId, XpDate date)
		{
			if (date == null)
				throw new ArgumentNullException("date");

			WorkerId = workerId;
			ItemId = itemId;
			Date = date;
		}
		#endregion Constructors
	}
}
