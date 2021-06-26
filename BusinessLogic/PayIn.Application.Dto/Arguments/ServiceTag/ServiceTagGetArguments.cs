using PayIn.Application.Dto.Results.ServiceTag;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTag
{
	public partial class ServiceTagGetArguments : IGetIdArgumentsBase<ServiceTagGetResult, PayIn.Domain.Public.ServiceTag>
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceTagGetArguments(int id)
		{
			Id = id;
		}
		public ServiceTagGetArguments()
		{
		}
		#endregion Constructors
	}
}