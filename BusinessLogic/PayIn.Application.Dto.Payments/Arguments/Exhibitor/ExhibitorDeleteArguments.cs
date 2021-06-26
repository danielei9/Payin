using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class ExhibitorDeleteArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public ExhibitorDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
