using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssignCheckPoint
{
    public partial class ControlFormAssignCheckPointCreateArguments : IArgumentsBase
	{
		[Display(Name="resources.controlFormAssignTemplate.form")]  [Required] public int FormId  { get; private set; }
		[Display(Name="resources.controlFormAssignTemplate.check")] [Required] public int CheckId { get; private set; }

		#region Constructors
		public ControlFormAssignCheckPointCreateArguments(int formId, int checkId)
		{
			FormId = formId;
			CheckId = checkId;
		}
		#endregion Constructors
	}
}
