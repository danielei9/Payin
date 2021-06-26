using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.SystemCard
{
	public class SystemCardDeleteArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructors
		public SystemCardDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors 
	}
}
