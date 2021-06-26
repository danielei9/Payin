using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTemplateItem
{
	public partial class ControlTemplateItemGetArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ControlTemplateItemGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
