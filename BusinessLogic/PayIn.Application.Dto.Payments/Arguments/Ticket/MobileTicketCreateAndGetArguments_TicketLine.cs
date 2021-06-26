using PayIn.Common;
using System;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobileTicketCreateAndGetArguments_TicketLine
    {
        /// <summary>
        /// Texto del ticket
        /// </summary>
		public string Title { get; set; }
        /// <summary>
        /// Precio unitario de la linea
        /// </summary>
        public decimal? Amount { get { return amount; } set { amount = value == null ? (decimal?)null : Math.Round(value.Value, 2); } }
        private decimal? amount;
        /// <summary>
        /// Cantidad de items de la linea
        /// </summary>
        public decimal Quantity { get { return quantity; } set { quantity = Math.Round(value, 2); } }
        private decimal quantity;
		/// <summary>
		/// Id de la entrada para el caso de la devolucion.
		/// </summary>
		public int? EntranceId { get; set; }
		/// <summary>
		/// Id del tipo de entrada si se corresponde a la compra de una entrada
		/// </summary>
		public int? EntranceTypeId { get; set; }
        /// <summary>
        /// Id del producto si se corresponde a la compra de un producto
        /// </summary>
		public int? ProductId { get; set; }
        /// <summary>
        /// Tipo de línea 
        /// </summary>
		public TicketLineType Type { get; set; } = TicketLineType.Buy;
        /// <summary>
        /// Id de la linea de campaña si se le aplica
        /// </summary>
		public int? CampaignLineId { get; set; }
        /// <summary>
        /// Id de la campaña si se le aplica
        /// </summary>
		public int? CampaignId { get; set; }
        /// <summary>
        /// Código de la campaña si se le aplica
        /// </summary>
		public long? CampaignCode { get; set; }
		/// <summary>
		/// Número de monedero al que afecta la línea
		/// </summary>
		public int? PurseId { get; set; }
	}
}
