using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.SystemCard
{
	public partial class SystemCardGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public SystemCardGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
