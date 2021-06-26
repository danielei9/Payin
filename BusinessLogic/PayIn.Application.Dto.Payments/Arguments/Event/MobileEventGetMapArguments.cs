using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileEventGetMapArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public MobileEventGetMapArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
