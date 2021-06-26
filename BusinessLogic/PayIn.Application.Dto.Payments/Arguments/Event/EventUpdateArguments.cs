using Newtonsoft.Json;
using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EventUpdateArguments : IArgumentsBase
	{
		[JsonIgnore] public int Id { get; set; }

		[Display(Name = "resources.event.longitude")]                   public decimal?			Longitude { get; set; }
		[Display(Name = "resources.event.latitude")]                    public decimal?			Latitude { get; set; }
		[Display(Name = "resources.event.place")]                       public string			Place { get; set; }
		[Display(Name = "resources.event.capacity")]                    public int?				Capacity { get; set; }
		[Display(Name = "resources.event.eventStart")]                  public XpDateTime		EventStart { get; set; }
		[Display(Name = "resources.event.eventEnd")]                    public XpDateTime		EventEnd { get; set; }
		[Display(Name = "resources.event.checkinStart")]                public XpDateTime		CheckInStart { get; set; }
		[Display(Name = "resources.event.checkinEnd")]                  public XpDateTime		CheckInEnd { get; set; }
		[Display(Name = "resources.event.state")]                       public EventState		State { get; set; }
		[Display(Name = "resources.event.entranceSystem")]   [Required] public int				EntranceSystemId { get; set; }
		[Display(Name = "resources.event.isVisible")]                   public bool				IsVisible { get; set; }
        [Display(Name = "resources.event.code")]                        public long?			Code { get; set; }
        [Display(Name = "resources.event.conditions")]                  public string			Conditions { get; set; }
		[Display(Name = "resources.event.visibility")]					public EventVisibility	Visibility { get; set; }
		[Display(Name = "resources.event.profileId")]                   public int?				ProfileId { get; set; }
		[Display(Name = "resources.event.maxEntrancesPerCard")]			public int?				MaxEntrancesPerCard { get; set; }
		[Display(Name = "resources.event.maxAmountToSpend")]			public decimal?			MaxAmountToSpend { get; set; }

		[Display(Name = "resources.event.name")]
		[Required(AllowEmptyStrings = false)]
		[Multilanguage("translationsName", "{id:item.id||0,translationType:0,eventId:arguments.id}")]
		public string Name { get; set; }

        [Display(Name = "resources.event.shortDescription")]
		[Multilanguage("translationsShortDescription", "{id:item.id||0,translationType:2,eventId:arguments.id}")]
		public string ShortDescription { get; set; }

		[Display(Name = "resources.event.description")]
		[Multilanguage("translationsDescription", "{id:item.id||0,translationType:1,eventId:arguments.id}")]
		[Formatable]
		public string Description { get; set; }

		#region Constructors
		public EventUpdateArguments(int id, decimal? longitude, decimal? latitude, string place, string name, string description, 
            int? capacity, XpDateTime eventStart, XpDateTime eventEnd, XpDateTime checkInStart, XpDateTime checkInEnd, EventState state, 
            int entranceSystemId, bool isVisible, long? code, string shortDescription, string conditions, EventVisibility visibility,
			int? profileId, int? maxEntrancesPerCard, decimal? maxAmountToSpend)
		{
			Id = id;
			Longitude = longitude;
			Latitude = latitude;
			Place = place ?? "";
			Name = name;
			Description = description ?? "";
			Capacity = capacity;
			EventStart = eventStart ?? XpDateTime.MinValue;
			EventEnd = eventEnd ?? XpDateTime.MaxValue;
			CheckInStart = checkInStart ?? XpDateTime.MinValue;
			CheckInEnd = checkInEnd ?? XpDateTime.MaxValue;
			State = state;
			EntranceSystemId = entranceSystemId;
			IsVisible = isVisible;
            Code = code;
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
