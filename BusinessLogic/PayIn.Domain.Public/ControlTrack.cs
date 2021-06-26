using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlTrack : IEntity
	{
		                           public int      Id              { get; set; }
		                           public DateTime CreatedAt       { get; set; }
		                           public int      WorkerId        { get; set; }
		                           public int      ItemId          { get; set; }

		                           public ControlPresence               PresenceSince { get; set; }
		                           public ControlPresence               PresenceUntil { get; set; }
		                           public ServiceWorker                 Worker        { get; set; }
		                           public ControlItem                   Item          { get; set; }
		[InverseProperty("Track")] public ICollection<ControlTrackItem> Items         { get; set; }

		#region Constructors
		public ControlTrack()
		{
			Items = new List<ControlTrackItem>();
		}
		#endregion Constructors
	}
}
