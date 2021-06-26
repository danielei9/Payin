using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssignTemplate
{
	public partial class ControlFormAssignTemplateUpdateArguments : IArgumentsBase
	{
		public int                Id     { get; set; }
		public IEnumerable<ControlFormAssignTemplateUpdateArguments_Value> Values { get; set; }

		#region Constructors
		public ControlFormAssignTemplateUpdateArguments(int id, IEnumerable<ControlFormAssignTemplateUpdateArguments_Value> values)
		{
			Id = id;
			Values = values;
		}
		#endregion Constructors
	}
}
