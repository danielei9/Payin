using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Transport
{
	public class TransportCard : Entity
	{
		                                        public DeviceTypeEnum DeviceType { get; set; }
		[Required(AllowEmptyStrings = true)]	public string DeviceAddress { get; set; }
		[Required(AllowEmptyStrings = true)]	public string Name { get; set; }
												public int? Entry { get; set; }
												public long Uid { get; set; }		
		[Required(AllowEmptyStrings = true)]    public string RandomId { get; set; }										
												public DateTime LastUse { get; set; }
												public DateTime? ExpiryDate { get; set; }
		[Required(AllowEmptyStrings = false)]	public string Login { get; set; }
		[Required(AllowEmptyStrings = true)]	public string ImageUrl { get; set; }
												public TransportCardState State { get; set; }

		#region TransportSystem
		public int TransportSystemId { get; set; }
		public TransportSystem TransportSystem { get; set; }
		#endregion TransportSystem

		#region Concession
		public int TransportConcessionId { get; set; }
		public TransportConcession TransportConcession { get; set; }
		#endregion Concession

		#region TransportCardTitle
		[InverseProperty("TransportCard")]
		public ICollection<TransportCardTitle> TransportCardTitles { get; set; } = new List<TransportCardTitle>();
		#endregion TransportCardTitle
	}
}
