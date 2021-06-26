using PayIn.Common;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlItem
{
	public class ControlItemMobileGetAllResult
    {
		public int                   Id                    { get; set; }
		public string                Name                  { get; set; }
		public string                Observations          { get; set; }
		public bool?                 SaveTrack             { get; set; }
		public bool?                 SaveFacialRecognition { get; set; }
		public bool?                 CheckTimetable        { get; set; }
		public PresenceType          PresenceType          { get; set; }
		public IEnumerable<ControlItemMobileGetAllResult_Planning> Plannings             { get; set; }
    }
}
