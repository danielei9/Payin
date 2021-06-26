using System.ComponentModel.DataAnnotations;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssignCheckPoint
{
	public partial class ControlFormAssignCheckPointGetCheckArguments : IArgumentsBase
	{
		           public string Filter { get; set; }
		[Required] public int    CheckId { get; set; }

		#region Constructors
		public ControlFormAssignCheckPointGetCheckArguments(string filter, int checkId)
		{
			Filter = filter ?? "";
			CheckId = checkId;
		}
		#endregion Constructors
	}
}
