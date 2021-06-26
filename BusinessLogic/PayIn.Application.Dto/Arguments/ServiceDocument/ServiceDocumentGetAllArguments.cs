using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ServiceDocumentGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceDocumentGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors 
	}
}
