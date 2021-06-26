using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTrack
{
	public partial class ControlTrackGetItemArguments : IArgumentsBase
	{
		public int    ItemId  { get; private set; }
		public int?   TrackId { get; private set; }
		public XpDate Date    { get; private set; }

		#region Constructors
		public ControlTrackGetItemArguments(int itemId, int? trackId, XpDate date)
		{
			ItemId = itemId;
			TrackId = trackId;
			Date = date;
		}
		#endregion Constructors
	}
}
