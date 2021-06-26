using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class SentiloDataDeleteSensorsArguments : IArgumentsBase
	{
		public int ProviderId { get; set; }

		#region Constructors
		public SentiloDataDeleteSensorsArguments(int providerId)
		{
			ProviderId = providerId;
		}
		#endregion Constructors
	}
}
