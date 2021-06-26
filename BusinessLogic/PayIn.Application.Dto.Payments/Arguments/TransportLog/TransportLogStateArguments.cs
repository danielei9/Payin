using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.TransportLog
{
	public class TransportLogStateArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TransportLogStateArguments(int id)
		{
			Id = 5;
		}
		#endregion Constructors
	}
}
