using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTag
{
	public partial class ServiceTagDeleteArguments : IDeleteArgumentsBase<PayIn.Domain.Public.ServiceTag>
  {
		public int Id { get; private set; }

		#region Constructors
		public ServiceTagDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
