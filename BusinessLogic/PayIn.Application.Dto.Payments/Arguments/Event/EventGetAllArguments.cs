using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EventGetAllArguments : IArgumentsBase
	{
		/// <summary>
		/// Cadena de texto que se va a búscar dentro de los principales campos de los eventos
		/// </summary>
		public string Filter { get; set; }
		/// <summary>
		/// Id de la empresa sobre la que se quiere consultar el listado de eventos
		/// </summary>
		[Display(Name = "")]
		public int? PaymentConcessionId { get; set; }

		#region Constructors
		public EventGetAllArguments (string filter, int? paymentConcessionId)
		{
			Filter = filter ?? "";
			PaymentConcessionId = paymentConcessionId;
		}
		#endregion Constructors
	}
}
