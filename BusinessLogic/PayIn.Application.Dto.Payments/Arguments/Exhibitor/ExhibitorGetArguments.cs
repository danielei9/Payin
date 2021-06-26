using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ExhibitorGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ExhibitorGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
