using System;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceSupplier
{
	public class ServiceSupplierGetArguments : IArgumentsBase
    {
		public int Id { get; private set; }

		#region Constructors
		public ServiceSupplierGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
