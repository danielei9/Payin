using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceDocumentGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ServiceDocumentGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
