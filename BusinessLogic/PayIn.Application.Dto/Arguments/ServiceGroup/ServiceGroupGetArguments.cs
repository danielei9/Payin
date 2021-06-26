using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceGroupGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceGroupGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
