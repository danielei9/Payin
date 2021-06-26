using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ExhibitorMobileGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ExhibitorMobileGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
