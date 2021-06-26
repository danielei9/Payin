using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTemplate
{
	public partial class ControlTemplateDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ControlTemplateDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
