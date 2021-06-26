using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Purse
{
	public partial class PurseGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PurseGetArguments(int id)
		{
			Id = id;
		}
		public PurseGetArguments()
		{
		}
		#endregion Constructors
	}
}
