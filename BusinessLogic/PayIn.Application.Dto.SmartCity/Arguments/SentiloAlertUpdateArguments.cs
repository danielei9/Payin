using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class SentiloAlertUpdateArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public string Message { get; set; }

		#region Constructors
		public SentiloAlertUpdateArguments(string message)
		{
			Message = message;
		}
		#endregion Constructors
	}
}
