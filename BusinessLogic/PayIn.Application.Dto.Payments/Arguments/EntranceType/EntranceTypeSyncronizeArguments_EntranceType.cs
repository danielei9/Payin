using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EntranceTypeSyncronizeArguments_EntranceType
	{
		/// <summary>
		/// Nombre del tipo de entrada
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// Código del tipo de entrada
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Precio de la entrada
		/// </summary>
		[Required]
		public decimal Price { get; set; }

		/// <summary>
		/// Número máximo de entradas a vender
		/// </summary>
		public int? MaxEntrance { get; set; }

		/// <summary>
		/// Inicio de la venta
		/// </summary>
		public XpDateTime SellStart { get; set; }

		/// <summary>
		/// Fin de la venta
		/// </summary>
		public XpDateTime SellEnd { get; set; }

		/// <summary>
		/// Apertura de las puertas
		/// </summary>
		public XpDateTime CheckInStart { get; set; }

		/// <summary>
		/// Cierre de las puertas
		/// </summary>
		public XpDateTime CheckInEnd { get; set; }
		
		/// <summary>
		/// Precio a sumar a precio de la entrada por gestión, plataforma, ...
		/// </summary>
		[Required]
		public decimal ExtraPrice { get; set; }

		/// <summary>
		/// Numero de jornadas para las que será válida la entrada
		/// </summary>
		public int? NumDays { get; set; }

		/// <summary>
		/// Hora de inicio de la jornada
		/// </summary>
		[Display(Name = "resources.entranceType.startDay")]
		public XpTime StartDay { get; set; }

		/// <summary>
		/// Hora de fin de la jornada
		/// </summary>
		[Display(Name = "resources.entranceType.endDay")]
		public XpTime EndDay { get; set; }

		/// <summary>
		/// Si es visible en la plataforma Pay[in]
		/// </summary>
		public bool IsVisible { get; set; }

		/// <summary>
		/// Visibilidad del tipo de entrada
		/// </summary>
		[Display(Name = "resources.entranceType.visibility")]
		public EntranceTypeVisibility Visibility { get; set; }
	}
}
