using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTemplate
{
	public partial class ControlTemplateGetArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ControlTemplateGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
