using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssign
{
	public partial class ControlFormAssignGetCheckArguments : IArgumentsBase
	{
		           public string Filter { get; set; }
		[Required] public int    CheckId { get; set; }

		#region Constructors
		public ControlFormAssignGetCheckArguments(string filter, int checkId)
		{
			Filter = filter ?? "";
			CheckId = checkId;
		}
		#endregion Constructors
	}
}
