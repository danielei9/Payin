using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
    public class ServiceIncidence : Entity
    {
        public IncidenceType        Type                 { get; set; }
        public IncidenceCategory    Category             { get; set; }
        public IncidenceSubCategory SubCategory          { get; set; }
        public DateTime DateTime { get; set; }
        public IncidenceState        State               { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string                Name                { get; set; }
		[Required(AllowEmptyStrings = true)]
		public string               InternalObservations { get; set; } = "";

        #region Notifications
        [InverseProperty(nameof(ServiceNotification.Incidence))]
        public ICollection<ServiceNotification> Notifications { get; set; } = new List<ServiceNotification>();
        #endregion Notifications

        #region Concession
        public int ConcessionId { get; set; }
        [ForeignKey(nameof(ServiceIncidence.ConcessionId))]
        public ServiceConcession Concession { get; set; }
		#endregion Concession
	}
}
