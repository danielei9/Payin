using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssign
{
	public partial class ControlFormAssignUpdateArguments : IArgumentsBase
	{
		public IEnumerable<ControlFormAssignUpdateArguments_Assign> Assigns { get; set; }

		#region Constructors
		public ControlFormAssignUpdateArguments(IEnumerable<ControlFormAssignUpdateArguments_Assign> assigns)
		{
			Assigns = assigns;
		}
		#endregion Constructors
	}
}
