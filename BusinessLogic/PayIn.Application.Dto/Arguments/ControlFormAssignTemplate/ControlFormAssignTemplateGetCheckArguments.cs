using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssignTemplate
{
	public partial class ControlFormAssignTemplateGetCheckArguments : IArgumentsBase
	{
		           public string Filter { get; set; }
		[Required] public int    CheckId { get; set; }

		#region Constructors
		public ControlFormAssignTemplateGetCheckArguments(string filter, int checkId)
		{
			Filter = filter ?? "";
			CheckId = checkId;
		}
		#endregion Constructors
	}
}
