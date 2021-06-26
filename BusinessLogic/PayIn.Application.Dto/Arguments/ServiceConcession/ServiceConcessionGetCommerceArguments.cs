using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public class ServiceConcessionGetCommerceArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ServiceConcessionGetCommerceArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
