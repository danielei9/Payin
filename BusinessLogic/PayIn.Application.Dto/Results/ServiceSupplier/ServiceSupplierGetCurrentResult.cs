using System;
using System.Collections.Generic;
using System.Text;
using PayIn.Common;

namespace PayIn.Application.Dto.Results.ServiceSupplier
{
	public class ServiceSupplierGetCurrentResult
	{
		public string SupplierName { get; set; }
		public string TaxName { get; set; }
		public string TaxNumber { get; set; }
		public string TaxAddress { get; set; }
		public bool   ShowPinForm { get; set; }
		public string Pin { get; set; }
		public string PinConfirmation { get; set; }
	}
}