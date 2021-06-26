using System;
using System.Collections.Generic;
using System.Text;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceSupplier
{
	public class ServiceSupplierGetAllArguments : IArgumentsBase
    {
 		public string Filter { get; set; }

		#region Constructors
		public ServiceSupplierGetAllArguments(string filter, int concessionId)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
    }
}
