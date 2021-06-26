using System.Collections.Generic;
using Xp.Application;

namespace PayIn.Application.Dto.Results.ControlTrack
{
	public class ControlTrackPublicGetByDayResult : ITrace<ControlTrackPublicGetByDayResult_Item>
	{
		public int               Id         { get; set; }
		public int               WorkerId   { get; set; }
		public string            WorkerName { get; set; }
		public int               ItemId     { get; set; }
		public string            ItemName   { get; set; }
		public ControlTrackPublicGetByDayResult_Item              Since      { get; set; }
		public ControlTrackPublicGetByDayResult_Item              Until      { get; set; }
		public IEnumerable<ControlTrackPublicGetByDayResult_Item> Items      { get; set; }
	}
}
