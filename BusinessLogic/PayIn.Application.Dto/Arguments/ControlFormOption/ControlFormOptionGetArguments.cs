using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class  ControlFormOptionGetArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int ArgumentId { get; set; }
		public int Id { get; set; }

		#region Constructors
		public ControlFormOptionGetArguments(string filter, int argumentId, int id)
		{
			Filter = filter ?? "";
			ArgumentId = argumentId;
			Id = id;
		}
		#endregion Constructors
	}
}
