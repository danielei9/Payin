using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Purse
{
	public class PurseGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public PurseGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
