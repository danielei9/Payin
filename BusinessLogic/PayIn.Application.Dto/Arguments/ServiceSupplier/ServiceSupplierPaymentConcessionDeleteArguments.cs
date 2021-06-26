using System;
using System.Collections.Generic;
using System.Text;
using Xp.Common.Dto.Arguments;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace PayIn.Application.Dto.Arguments.ServiceSupplier
{
	public class ServiceSupplierPaymentConcessionDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int SupplierId { get; set; }

		#region Constructors
		public ServiceSupplierPaymentConcessionDeleteArguments(int id, int supplierId)
		{
			Id = id;
			SupplierId = supplierId;
		}
		#endregion Constructors
	}
}