namespace PayIn.Application.Dto.Payments.Results.CampaignCode
{
    public class PublicCampaignCodeCreateResult
    {
        /// <summary>
        /// Id del código de la campaña
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Código de campaña para introducir en un QR, código de barras, ...
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Id de la campaña
        /// </summary>
        public int CampaignId { get; set; }
        /// <summary>
        /// Ćódigo de campaña para ponerlod de forma simplificada e identificar el código via manual
        /// </summary>
        public string CodeText { get; set; }
    }
}
