using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceGroupServiceUsersCreateGetNameArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceGroupServiceUsersCreateGetNameArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
