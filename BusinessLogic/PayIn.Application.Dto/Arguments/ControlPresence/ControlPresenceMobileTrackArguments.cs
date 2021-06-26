using System.Collections.Generic;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
	public partial class ControlPresenceMobileTrackArguments : IArgumentsBase
	{
		public partial class TrackItem
		{
			public XpDateTime        Date      { get; set; }
			public decimal           Latitude  { get; set; }
			public decimal           Longitude { get; set; }
			public int               Quality   { get; set; }
			public float             Speed     { get; set; }
		}
		
		public partial class Track
		{
			public int Id { get; set; }
		}

		public IEnumerable<Track> Tracks { get; private set; }
		public IEnumerable<TrackItem> TrackItems { get; private set; }

		#region Constructors
		public ControlPresenceMobileTrackArguments(
			IEnumerable<Track> tracks,
			IEnumerable<TrackItem> trackItems
		)
		{
			Tracks = tracks;
			TrackItems = trackItems;
		}
		#endregion Constructors
	}
}
