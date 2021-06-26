using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssign
{
	public partial class ControlFormAssignCreateArguments : IArgumentsBase
	{
		[Display(Name="resources.controlFormAssign.form")]  [Required] public int FormId  { get; private set; }
		[Display(Name="resources.controlFormAssign.check")] [Required] public int CheckId { get; private set; }

		#region Constructors
		public ControlFormAssignCreateArguments(int formId, int checkId)
		{
			FormId = formId;
			CheckId = checkId;
		}
		#endregion Constructors
	}
}
