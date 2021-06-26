using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceUserGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceUserGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
