using System.Collections.Generic;
using Xp.Application;

namespace PayIn.Application.Dto.Results.ControlTrack
{
	public partial class ControlTrackGetItemResult : ITrace<ControlTrackGetItemResult_Item>
	{
		public int               Id          { get; set; }
		public int               WorkerId    { get; set; }
		public string            WorkerName  { get; set; }
		public int               ItemId      { get; set; }
		public string            ItemName    { get; set; }
		public ControlTrackGetItemResult_Item              Since       { get; set; }
		public ControlTrackGetItemResult_Item              Until       { get; set; }
		public IEnumerable<ControlTrackGetItemResult_Item> Items       { get; set; }
	}
}
