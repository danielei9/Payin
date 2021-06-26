using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssignTemplate
{
	public partial class ControlFormAssignTemplateGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ControlFormAssignTemplateGetArguments(int id)
		{
			Id = id;
		}
		public ControlFormAssignTemplateGetArguments()
		{
		}
		#endregion Constructors
	}
}
