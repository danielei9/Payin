using PayIn.Common;
using System;
using System.Collections.Generic;

namespace PayIn.Presentation.Design
{
	public class PaymentD
	{
		public Payment_ItemD Arguments { get; set; }
		public IEnumerable<Payment_PaymentMediaD> PaymentMedias { get; set; }
	}
	public class Payment_ItemD
	{
		public int? SupplierId { get; set; }
		public string SupplierName { get; set; }
		public float Amount { get; set; }
		public string Currency { get; set; }
		public string Reference { get; set; }
		public string Title { get; set; }
		public DateTime Date { get; set; }
		public ServiceType ServiceType { get; set; }
	}
	public class Payment_PaymentMediaD
	{
		public string Title { get; set; }
		public string Subtitle { get; set; }
		public string Other { get; set; }
	}
}
