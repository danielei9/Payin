using PayIn.Common;
using System;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlTrackItem : IEntity
	{
		                  public int      Id        { get; set; }
											public DateTime CreatedAt { get; set; }
		                  public DateTime Date      { get; set; }
		[Precision(9, 6)] public decimal  Latitude  { get; set; }
		[Precision(9, 6)] public decimal  Longitude { get; set; }
		                  public int      Quality   { get; set; }
		                  public float    Speed     { get; set; }

		#region Track
		public int TrackId { get; set; }
		public ControlTrack Track { get; set; }
		#endregion Track
	}
}
