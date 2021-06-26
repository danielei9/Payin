using PayIn.Common;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlPlanningCheck
{
	public class ControlPlanningCheckMobileGetResult
    {
		public int                   Id                    { get; set; }
		public string                Name                  { get; set; }
		public string                Observations          { get; set; }
		public bool?                 SaveTrack             { get; set; }
		public bool?                 SaveFacialRecognition { get; set; }
		public bool?                 CheckTimetable        { get; set; }
		public PresenceType          PresenceType          { get; set; }
		public IEnumerable<ControlPlanningCheckMobileGetResult_Planning> Plannings             { get; set; }
    }
}
