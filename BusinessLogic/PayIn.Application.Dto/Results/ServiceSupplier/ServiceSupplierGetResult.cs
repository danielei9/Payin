using System;
using System.Collections.Generic;
using System.Text;
using PayIn.Common;

namespace PayIn.Application.Dto.Results.ServiceSupplier
{
	public class ServiceSupplierGetResult
	{
			public string Login { get; set; }
			public string Name { get; set; }
			public string TaxName { get; set; }
			public string TaxNumber { get; set; }
			public string TaxAddress { get; set; }		
	}
}