using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Event : Entity
	{
		[Precision(9, 6)]                           public decimal? Longitude { get; set; }
		[Precision(9, 6)]                           public decimal? Latitude { get; set; }
		[Required(AllowEmptyStrings = true)]        public string Place { get; set; }
		[Required(AllowEmptyStrings = false)]       public string Name { get; set; }
		[Required(AllowEmptyStrings = true)]        public string Description { get; set; }
		[Required(AllowEmptyStrings = true)]        public string PhotoUrl { get; set; }
        [Required(AllowEmptyStrings = true)]        public string PhotoMenuUrl { get; set; } = "";
                                                    public int? Capacity { get; set; }
		                                            public DateTime EventStart { get; set; }
		                                            public DateTime EventEnd { get; set; }
		                                            public DateTime CheckInStart { get; set; }
		                                            public DateTime CheckInEnd { get; set; }
		                                            public EventState State { get; set; }
		                                            public bool IsVisible { get; set; }
		                                            public long? Code { get; set; }
		                                            public int MaxEntrancesPerCard { get; set; } = int.MaxValue;
                                                    public decimal? MaxAmountToSpend { get; set; }
        [Required(AllowEmptyStrings = true)]        public string ShortDescription { get; set; }
		[Required(AllowEmptyStrings = true)]        public string Conditions { get; set; }
		                                            public EventVisibility Visibility { get; set; }
        //Imagen que mostrará la localización de los diferentes puestos del evento
        [Required(AllowEmptyStrings = true)]        public string MapUrl { get; set; }

        #region PaymentConcession
        public int PaymentConcessionId { get; set; }
		[ForeignKey("PaymentConcessionId")]
		public PaymentConcession PaymentConcession { get; set; }
        #endregion PaymentConcession

        #region Exhibitors
        [InverseProperty("Events")]
        public ICollection<Exhibitor> Exhibitors { get; set; } = new List<Exhibitor>();
        #endregion Exhibitors

        #region EntranceTypes
        [InverseProperty("Event")]
		public ICollection<EntranceType> EntranceTypes { get; set; } = new List<EntranceType>();
		#endregion EntranceTypes

		#region Entrances
		[InverseProperty("Event")]
		public ICollection<Entrance> Entrances { get; set; } = new List<Entrance>();
        #endregion Entrances

        #region Tickets
        [InverseProperty("Event")]
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        #endregion Tickets

        #region EntranceSystem
        public int EntranceSystemId { get; set; }
		[ForeignKey("EntranceSystemId")]
		public EntranceSystem EntranceSystem { get; set; }
		#endregion EntranceSystem

		#region EventImages
		[InverseProperty("Event")]
		public ICollection<EventImage> EventImages { get; set; } = new List<EventImage>();
		#endregion EventImages

		#region EventForm
		[InverseProperty("Event")]
		public ICollection<EventForm> EventForms { get; set; } = new List<EventForm>();
		#endregion EventForm

		#region Profile
		public int? ProfileId { get; set; }
		[ForeignKey("ProfileId")]
		public Profile Profile { get; set; }
		#endregion Profile

		#region Contact
		[InverseProperty("Event")]
		public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
		#endregion Contact

		#region Activity
		[InverseProperty("Event")]
		public ICollection<Activity> Activities { get; set; } = new List<Activity>();
		#endregion Activity

		#region Campaigns
		[InverseProperty("TargetEvents")]
		public ICollection<Campaign> Campaigns { get; set; } = new List<Campaign>();
        #endregion Campaigns

        #region Operations
        //[InverseProperty("Event")] // It makes bucle
        public ICollection<ServiceOperation> Operations { get; set; } = new List<ServiceOperation>();
        #endregion Operations

        #region Notices
        [InverseProperty("Event")]
		public ICollection<Notice> Notices { get; set; } = new List<Notice>();
        #endregion Notices

        #region TranslationNames
        [InverseProperty(nameof(Translation.EventName))]
        public ICollection<Translation> TranslationNames { get; set; } = new List<Translation>();
        #endregion TranslationNames

        #region TranslationDescriptions
        [InverseProperty(nameof(Translation.EventDescription))]
        public ICollection<Translation> TranslationDescriptions { get; set; } = new List<Translation>();
        #endregion TranslationDescriptions

        #region TranslationShortDescriptions
        [InverseProperty(nameof(Translation.EventShortDescription))]
        public ICollection<Translation> TranslationShortDescriptions { get; set; } = new List<Translation>();
        #endregion TranslationShortDescriptions

        #region TranslationPlaces
        [InverseProperty(nameof(Translation.EventPlace))]
        public ICollection<Translation> TranslationPlaces { get; set; } = new List<Translation>();
        #endregion TranslationPlaces

        #region TranslationConditions
        [InverseProperty(nameof(Translation.EventConditions))]
        public ICollection<Translation> TranslationConditions { get; set; } = new List<Translation>();
        #endregion TranslationConditions
    }
}
