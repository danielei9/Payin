using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class Ticket : IEntity
	{
		                                                                    public int         Id             { get; set; }
		                                                                    public string      AddressName    { get; set; }
		                                                                    public int?        AddressNumber  { get; set; }
		                                                                    public decimal     Amount         { get; set; }
		                                                                    public string      CityName       { get; set; }
		[Required(AllowEmptyStrings = true)]                                public string      ConcessionName { get; set; }
		[Required(AllowEmptyStrings = false)] [MinLength(3)] [MaxLength(3)] public string      Currency       { get; set; } // http://es.wikipedia.org/wiki/ISO_4217
		                                                                    public DateTime    Date           { get; set; }
		[Required(AllowEmptyStrings = false)]                               public string      Reference      { get; set; }
																																				public ServiceType ServiceType    { get; set; }
		[Required(AllowEmptyStrings = false)]																public string      Title          { get; set; }
																																				public DateTime?   Until          { get; set; }

		#region Details
		[InverseProperty("Ticket")]
		public ICollection<TicketDetail> Details { get; set; }
		#endregion Details

		#region Payments
		[InverseProperty("Ticket")]
		public ICollection<Payment> Payments { get; set; }
		#endregion Payments

		#region Supplier
		public int SupplierId { get; set; }

		public ServiceSupplier Supplier { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string SupplierName { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string SupplierTaxName { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string SupplierTaxAddress { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string SupplierTaxNumber { get; set; }
		#endregion Supplier

		#region Zone
		public ServiceZone Zone { get; set; }

		public int? ZoneId { get; set; }
		
		[Required(AllowEmptyStrings = true)]	
		public string ZoneName { get; set; }
		#endregion Zone

		#region Constructors
		public Ticket()
		{
			Payments = new List<Payment>();
			Details = new List<TicketDetail>();
		}
		#endregion Constructors
	}
}