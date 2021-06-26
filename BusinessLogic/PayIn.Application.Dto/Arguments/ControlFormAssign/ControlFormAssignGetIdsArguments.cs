using System;
using System.Collections.Generic;
using System.Linq;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssign
{
	public partial class ControlFormAssignGetIdsArguments : IArgumentsBase
	{
		public IEnumerable<int> Ids { get; set; }

		#region Constructors
		public ControlFormAssignGetIdsArguments(string ids)
		{
			Ids = (ids ?? "").Split(',').Select(x => Convert.ToInt32(x));
		}
		#endregion Constructors
	}
}
