using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.GreyList
{
	public class GreyListGetAllArguments : IArgumentsBase
	{
		public string Filter { get; private set; }


		#region Constructors
		public GreyListGetAllArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}
