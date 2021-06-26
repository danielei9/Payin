using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssignTemplate
{
    public partial class ControlFormAssignTemplateDeleteArguments : IArgumentsBase
  {
		public int Id { get; set; }

		#region Constructors
		public ControlFormAssignTemplateDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
