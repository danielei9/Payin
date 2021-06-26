using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTemplateCheck
{
	public partial class ControlTemplateCheckGetArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ControlTemplateCheckGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
