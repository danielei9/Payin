using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;
using System;

namespace PayIn.Domain.Public
{
	public class ControlItem : IEntity
	{
		public int        Id                    { get; set; }
		public string     Name                  { get; set; }
		public string     Observations          { get; set; }
		public bool       SaveTrack             { get; set; }
		public bool       SaveFacialRecognition { get; set; }
		public bool       CheckTimetable        { get; set; }
		public DateTime   TrackFrecuency        { get; set; }    

		#region Concession
		public int ConcessionId { get; set; }
		public ServiceConcession Concession { get; set; }
		#endregion Concession

		#region Plannings
		[InverseProperty("Item")]
		public ICollection<ControlPlanning> Plannings { get; set; }
		#endregion Plannings

		#region Presences

		#endregion Presences

		#region CheckPoints
		[InverseProperty("Item")]
		public ICollection<ServiceCheckPoint> CheckPoints { get; set; }
		#endregion CheckPoints

		#region Templates
		[InverseProperty("Item")]
		public ICollection<ControlTemplate> Templates { get; set; }
		#endregion Templates

		#region Tracks
		[InverseProperty("Item")]
		public ICollection<ControlTrack> Tracks { get; set; }
		#endregion Tracks

		#region Constructors
		public ControlItem()
		{
			Plannings = new List<ControlPlanning>();
			CheckPoints = new List<ServiceCheckPoint>();
			Templates = new List<ControlTemplate>();
			Tracks = new List<ControlTrack>();
		}
		#endregion Constructors
	}
}
