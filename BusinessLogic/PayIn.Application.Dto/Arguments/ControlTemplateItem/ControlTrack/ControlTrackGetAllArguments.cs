using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTrack
{
	public partial class ControlTrackGetAllArguments : IArgumentsBase
	{
		           public string Filter   { get; set; }
		[Required] public XpDate Date     { get; set; }
		           public int?   WorkerId { get; set; }
		           public int?   ItemId   { get; set; }

		#region Constructors
		public ControlTrackGetAllArguments(string filter, XpDate date, int? workerId, int? itemId)
		{
			if (date == null)
				throw new ArgumentNullException("date");

			Filter = filter ?? "";
			Date = date;
			WorkerId = workerId;
			ItemId = itemId;
		}
		#endregion Constructors
	}
}
