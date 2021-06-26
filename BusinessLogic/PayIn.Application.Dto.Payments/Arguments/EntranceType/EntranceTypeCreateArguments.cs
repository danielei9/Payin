using Newtonsoft.Json;
using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EntranceTypeCreateArguments : IArgumentsBase
	{
		[JsonIgnore]
		public int EventId { get; set; }

		[Display(Name = "resources.entranceType.name")]
		[Required]
		public string Name											{ get; set; }

		[Display(Name = "resources.entranceType.description")]
		public string Description									{ get; set; }
		
        [Display(Name = "resources.entranceType.code")]
        public string Code                                          { get; set; }

        [Display(Name = "resources.entranceType.shortDescription")]
        public string ShortDescription                              { get; set; }

        [Display(Name = "resources.entranceType.condition")]
        public string Conditions                                    { get; set; }

        [Display(Name = "resources.entranceType.price")]
		[Required]
		[DataType(DataType.Currency)]
		public decimal Price										{ get; set; }

		[Display(Name = "resources.entranceType.maxEntrance")]
		public int? MaxEntrance										{ get; set; }

        [Display(Name = "resources.entranceType.maxSending")]
        public int MaxSendingCount                                  { get; set; }

		/// <summary>
		/// Inicio de la venta
		/// </summary>
		[Display(Name = "resources.entranceType.sellStart")]
		public XpDateTime SellStart									{ get; set; }

		/// <summary>
		/// Fin de la venta
		/// </summary>
		[Display(Name = "resources.entranceType.sellEnd")]
		public XpDateTime SellEnd									{ get; set; }

		/// <summary>
		/// Apertura de las puertas
		/// </summary>
		[Display(Name = "resources.entranceType.checkInStart")]
		public XpDateTime CheckInStart								{ get; set; }

		/// <summary>
		/// Cierre de las puertas
		/// </summary>
		[Display(Name = "resources.entranceType.checkInEnd")]
		public XpDateTime CheckInEnd								{ get; set; }

		[Display(Name = "resources.entranceType.extraPrice")]
		[Required]
		[DataType(DataType.Currency)]
		public decimal? ExtraPrice									{ get; set; }

		/// <summary>
		/// Inicio del rango para validadoras de tipo rango
		/// </summary>
		[Display(Name = "resources.entranceType.rangeMin")]
		public int? RangeMin										{ get; set; }

		/// <summary>
		/// Fin del rango para validadoras de tipo rango
		/// </summary>
		[Display(Name = "resources.entranceType.rangeMax")]
		public int? RangeMax										{ get; set; }

		/// <summary>
		/// Numero de jornadas para las que será válida la entrada
		/// </summary>
		[Display(Name = "resources.entranceType.numDay")]
		public int? NumDays											{ get; set; }

		/// <summary>
		/// Hora de inicio de la jornada
		/// </summary>
		[Display(Name = "resources.entranceType.startDay")]
		public XpTime StartDay										{ get; set; }

		/// <summary>
		/// Hora de fin de la jornada
		/// </summary>
		[Display(Name = "resources.entranceType.endDay")]
		public XpTime EndDay										{ get; set; }

		/// <summary>
		/// Visibilidad del tipo de entrada
		/// </summary>
		[Display(Name = "resources.entranceType.visibility")]
		public EntranceTypeVisibility Visibility					{ get; set; }

		#region Constructors
		public EntranceTypeCreateArguments(string name, string description, 
            decimal price, int? maxEntrance, XpDateTime sellStart, XpDateTime sellEnd, 
            XpDateTime checkInStart, XpDateTime checkInEnd, decimal? extraPrice, string code,
            int? rangeMin, int? rangeMax, int maxSendingCount, string shortDescription, string conditions,
			int? numDays, XpTime startDay, XpTime endDay, EntranceTypeVisibility visibility)
		{
			Name = name;
			Description = description ?? "";
            Code = code ?? "";
            ShortDescription = shortDescription ?? "";
            Conditions = conditions ?? "";
            Price = price;
			MaxEntrance = maxEntrance ?? int.MaxValue;
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
