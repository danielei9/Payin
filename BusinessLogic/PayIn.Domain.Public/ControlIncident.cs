using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlIncident : IEntity
	{
		public int          Id           { get; set; }
		public IncidentType Type         { get; set; }
		public DateTime     CreatedAt    { get; set; }
		public String       Observations { get; set; }
		public String       Source       { get; set; }
		public int          SourceId     { get; set; }
	}
}
