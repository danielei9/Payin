using PayIn.Domain.Payments;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Transport
{
	public class TransportOperationTitle : Entity
	{
		public decimal? Quantity { get; set; }
		public DateTime? Caducity { get; set; }
		public EigeZonaEnum? Zone { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Unities { get; set; } = "";
        public int? Slot { get; set; }

        #region Operation
        public int OperationId { get; set; }
		[ForeignKey("OperationId")]
		public TransportOperation Operation { get; set; }
		#endregion Operation

		#region Title	
		public int? TitleId { get; set; }
		[ForeignKey("TitleId")]
		public TransportTitle Title { get; set; }
        #endregion Title

        #region Purse	
        public int? PurseId { get; set; }
        [ForeignKey("PurseId")]
        public Purse Purse { get; set; }
        #endregion Purse
    }
}
