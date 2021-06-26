using Newtonsoft.Json;
using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EntranceTypeUpdateArguments : IArgumentsBase
	{
		[JsonIgnore]
		public int Id										{ get; set; }

		[Display(Name = "resources.entranceType.isVisible")]
		public bool IsVisible								{ get; set; }

		[Display(Name = "resources.entranceType.name")]
		public string Name									{ get; set; }

        [Display(Name = "resources.entranceType.code")]
        public string Code                                  { get; set; }

        [Display(Name = "resources.entranceType.state")]
		public EntranceTypeState State                      { get; set; }

		[Display(Name = "resources.entranceType.description")]
		public string Description							{ get; set; }

		[Display(Name = "resources.entranceType.price")]
		[Required]
		[DataType(DataType.Currency)]
		public decimal Price								{ get; set; }

		[Display(Name = "resources.entranceType.maxEntrance")]
		public int? MaxEntrance								{ get; set; }

		[Display(Name = "resources.entranceType.sellStart")]
		public XpDateTime SellStart							{ get; set; }

		[Display(Name = "resources.entranceType.sellEnd")]
		public XpDateTime SellEnd							{ get; set; }

		[Display(Name = "resources.entranceType.checkInStart")]
		public XpDateTime CheckInStart						{ get; set; }

		[Display(Name = "resources.entranceType.checkInEnd")]
		public XpDateTime CheckInEnd						{ get; set; }

		[Display(Name = "resources.entranceType.extraPrice")]
		[Required]
		[DataType(DataType.Currency)]
		public decimal ExtraPrice							{ get; set; }

		[Display(Name = "resources.entranceType.rangeMin")]
		public int? RangeMin                                { get; set; }

		[Display(Name = "resources.entranceType.rangeMax")]
		public int? RangeMax                                { get; set; }

        [Display(Name = "resources.entranceType.shortDescription")]
        public string ShortDescription                      { get; set; }

        [Display(Name = "resources.entranceType.condition")]
        public string Conditions                            { get; set; }

        [Display(Name = "resources.entranceType.maxSending")]
        public int MaxSendingCount                          { get; set; }

		[Display(Name = "resources.entranceType.numDay")]
		public int? NumDays									{ get; set; }

		[Display(Name = "resources.entranceType.startDay")]
		public XpTime StartDay								{ get; set; }

		[Display(Name = "resources.entranceType.endDay")]
		public XpTime EndDay								{ get; set; }

		[Display(Name = "resources.entranceType.visibility")]
		public EntranceTypeVisibility Visibility			{ get; set; }

		#region Constructors
		public EntranceTypeUpdateArguments(string name, bool isVisible, string description, 
            decimal price, int? maxEntrance, XpDateTime sellStart, XpDateTime sellEnd, 
            XpDateTime checkInStart, XpDateTime checkInEnd, decimal extraPrice, int id, 
            EntranceTypeState state, int? rangeMin, int? rangeMax, string code,
            int maxSendingCount, string shortDescription, string conditions,
			int? numDays, XpTime startDay, XpTime endDay, EntranceTypeVisibility visibility)
		{
			Id = id;
			IsVisible = isVisible;
			Name = name;
            Code = code ?? "";
            State = state;
			Description = description ?? "";
            ShortDescription = shortDescription ?? "";
            Conditions = conditions ?? "";
            Price = price;
			MaxEntrance = maxEntrance;
            MaxSendingCount = maxSendingCount;
            SellStart = sellStart ?? XpDateTime.MinValue;
			SellEnd = sellEnd ?? XpDateTime.MaxValue;
			CheckInStart = checkInStart ?? XpDateTime.MinValue;
			CheckInEnd = checkInEnd ?? XpDateTime.MaxValue;
			ExtraPrice = extraPrice;
			RangeMin = rangeMin;
			RangeMax = rangeMax;
			NumDays = numDays;
			StartDay = startDay;
			EndDay = endDay;
			Visibility = visibility;
		}
		#endregion Constructors
	}
}
