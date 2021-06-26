using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceConcession : IEntity
	{
		                                      public int             Id				    { get; set; }
		[Required(AllowEmptyStrings = false)] public string          Name			    { get; set; }
		                                      public ServiceType     Type			    { get; set; }
		                                      public int             MaxWorkers		    { get; set; }
		                                      public ConcessionState State			    { get; set; } 
		                                      public TimeSpan        Interval		    { get; set; }
                                              public bool            VisibleCommerce    { get; set; }

        #region Zones
        [InverseProperty("Concession")]
		public ICollection<ServiceZone> Zones { get; set; } = new List<ServiceZone>();
		#endregion Zones

		#region Supplier
		public int SupplierId { get; set; }
		[ForeignKey("SupplierId")]
		public ServiceSupplier Supplier { get; set; }
		#endregion Supplier

		#region FreeDays
		[InverseProperty("Concession")]
		public ICollection<ServiceFreeDays> FreeDays { get; set; } = new List<ServiceFreeDays>();
		#endregion FreeDays

		#region Items
		[InverseProperty("Concession")]
		public ICollection<ControlItem> Items { get; set; } = new List<ControlItem>();
		#endregion Items

		#region Forms
		[InverseProperty("Concession")]
		public ICollection<ControlForm> Forms { get; set; } = new List<ControlForm>();
		#endregion Forms

		#region ServiceUser
		[InverseProperty("Concession")]
		public ICollection<ServiceUser> ServiceUsers { get; set; } = new List<ServiceUser>();
		#endregion ServiceUser

		#region Cards
		[InverseProperty("Concession")]
		public ICollection<ServiceCard> Cards { get; set; } = new List<ServiceCard>();
		#endregion Cards

		#region SystemCardOwners
		[InverseProperty("ConcessionOwner")]
		public ICollection<SystemCard> SystemCardOwners { get; set; } = new List<SystemCard>();
        #endregion SystemCardOwners

        #region CardBatchs
        [InverseProperty("Concession")]
        public ICollection<ServiceCardBatch> CardBatchs { get; set; } = new List<ServiceCardBatch>();
        #endregion CardBatchs

        #region Categories
        [InverseProperty("ServiceConcession")]
        public ICollection<ServiceCategory> Categories { get; set; } = new List<ServiceCategory>();
        #endregion Categories

        #region Cities
        [InverseProperty("Concession")]
        public ICollection<ServiceCity> Cities { get; set; } = new List<ServiceCity>();
        #endregion Cities

        #region Incidences
        [InverseProperty(nameof(ServiceIncidence.Concession))]
        public ICollection<ServiceIncidence> Incidences { get; set; } = new List<ServiceIncidence>();
        #endregion Incidences
    }
}
