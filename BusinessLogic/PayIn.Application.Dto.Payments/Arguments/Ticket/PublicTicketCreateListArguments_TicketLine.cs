using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class PublicTicketCreateListArguments_TicketLine
    {
        /// <summary>
        /// Texto del ticket
        /// </summary>
		public string Title { get; set; }
        /// <summary>
        /// Precio unitario de la linea
        /// </summary>
		public decimal? Amount { get; set; }
        /// <summary>
        /// Cantidad de items de la linea
        /// </summary>
		public decimal Quantity { get; set; }
        /// <summary>
        /// Id del tipo de entrada si se corresponde a la compra de una entrada
        /// </summary>
		public int? EntranceTypeId { get; set; }
        /// <summary>
        /// Id del producto si se corresponde a la compra de un producto
        /// </summary>
		public int? ProductId { get; set; }
        /// <summary>
        /// Codidgo del producto si se corresponde a la compra de un producto (se usa si productId es null)
        /// </summary>
        public string ProductCode { get; set; }
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
    }
}
