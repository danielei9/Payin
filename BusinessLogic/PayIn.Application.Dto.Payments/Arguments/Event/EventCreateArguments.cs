using PayIn.Common;
using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EventCreateArguments : IArgumentsBase
    {
		[Display(Name = "resources.event.paymentConcession")] [Required]	public int				PaymentConcessionId { get; set; }
		[Display(Name = "resources.event.longitude")]						public decimal?			Longitude { get; set; }
		[Display(Name = "resources.event.latitude")]						public decimal?			Latitude { get; set; }
		[Display(Name = "resources.event.place")]							public string			Place { get; set; }
		[Display(Name = "resources.event.name")]              [Required]	public string			Name { get; set; }
        [Display(Name = "resources.event.code")]							public long?			Code { get; set; }
        [Display(Name = "resources.event.description")]						public string			Description { get; set; }
		[Display(Name = "resources.event.capacity")]						public int?				Capacity { get; set; }
		[Display(Name = "resources.event.eventStart")]						public XpDateTime		EventStart { get; set; }
		[Display(Name = "resources.event.eventEnd")]						public XpDateTime		EventEnd	{ get; set; }
		[Display(Name = "resources.event.checkinStart")]					public XpDateTime		CheckInStart { get; set; }
		[Display(Name = "resources.event.checkinEnd")]						public XpDateTime		CheckInEnd { get; set; }
		[Display(Name = "resources.event.entranceSystem")]    [Required]	public int				EntranceSystemId { get; set; }
        [Display(Name = "resources.event.shortDescription")]				public string			ShortDescription { get; set; }
        [Display(Name = "resources.event.conditions")]						public string			Conditions { get; set; }
		[Display(Name = "resources.event.visibility")]						public EventVisibility	Visibility { get; set; }
		[Display(Name = "resources.event.profileId")]						public int?				ProfileId { get; set; }
		[Display(Name = "resources.event.maxEntrancesPerCard")]				public int?				MaxEntrancesPerCard { get; set; }
		[Display(Name = "resources.event.maxAmountToSpend")]				public decimal?			MaxAmountToSpend { get; set; }

		#region Constructors
		public EventCreateArguments(int paymentConcessionId, decimal? longitude, decimal? latitude, string place, 
            string name, string description, int? capacity, DateTime? eventStart, DateTime? eventEnd,
            XpDateTime checkInStart, XpDateTime checkInEnd, int entranceSystemId, long? code,
            string shortDescription, string conditions, EventVisibility visibility, 
			int? profileId, int? maxEntrancesPerCard, decimal? maxAmountToSpend)
		{
            PaymentConcessionId = paymentConcessionId;
            Longitude = longitude;
			Latitude = latitude;
			Place = place ?? "";
            Code = code;
			Name = name;
			Description = description ?? "";
			Capacity = capacity;
			EventStart = ((XpDateTime) eventStart) ?? XpDateTime.MinValue;
			EventEnd = ((XpDateTime) eventEnd) ?? XpDateTime.MaxValue;
			CheckInStart = checkInStart ?? XpDateTime.MinValue;
			CheckInEnd = checkInEnd ?? XpDateTime.MaxValue;
			EntranceSystemId = entranceSystemId;
            Conditions = conditions ?? "";
            ShortDescription = shortDescription ?? "";
			Visibility = visibility;
			ProfileId = profileId;
			MaxEntrancesPerCard = maxEntrancesPerCard;
			MaxAmountToSpend = maxAmountToSpend;
		}
		#endregion Constructors
	}
}
