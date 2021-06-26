using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlTemplateItem
{
	public partial class ControlTemplateItemDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ControlTemplateItemDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
