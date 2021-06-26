using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Purse
{
	public partial class PurseGetUsersArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PurseGetUsersArguments(int id)
		{
			Id = id;
		}	
		#endregion Constructors
	}
}
