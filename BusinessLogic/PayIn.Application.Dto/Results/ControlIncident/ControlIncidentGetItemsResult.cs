using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayIn.Application.Dto.Results.ControlIncident
{
    public class ControlIncidentGetItemsResult
    {
		public int     Id                    { get; set; }
		public String  Name                  { get; set; }
		public String  Observations          { get; set; }
		public Boolean SaveTrack             { get; set; }
		public Boolean SaveFacialRecognition { get; set; }
		public Boolean CheckTimetable        { get; set; }
		public PresenceType PresenceType     { get; set; }
    }
}
