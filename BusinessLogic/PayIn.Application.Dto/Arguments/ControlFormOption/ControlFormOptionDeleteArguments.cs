using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ControlFormOptionDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructor
		public ControlFormOptionDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
