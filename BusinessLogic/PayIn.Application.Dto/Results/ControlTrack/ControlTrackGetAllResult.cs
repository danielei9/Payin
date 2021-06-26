using System.Collections.Generic;
using Xp.Application;

namespace PayIn.Application.Dto.Results.ControlTrack
{
	public class ControlTrackGetAllResult : ITrace<ControlTrackGetAllResult_Item>
	{

		public int               Id         { get; set; }
		public int               WorkerId   { get; set; }
		public string            WorkerName { get; set; }
		public int               ItemId     { get; set; }
		public string            ItemName   { get; set; }
		public ControlTrackGetAllResult_Item              Since      { get; set; }
		public ControlTrackGetAllResult_Item              Until      { get; set; }
		public IEnumerable<ControlTrackGetAllResult_Item> Items      { get; set; }
	}
}
