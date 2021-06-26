using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.GreyList
{
	public class GreyListDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public GreyListDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
