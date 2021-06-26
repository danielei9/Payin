using System;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public partial class ServiceConcessionGetArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ServiceConcessionGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
