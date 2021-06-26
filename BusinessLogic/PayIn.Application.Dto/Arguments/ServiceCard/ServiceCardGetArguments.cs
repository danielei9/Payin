using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCardGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceCardGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
