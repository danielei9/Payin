using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class ApiSensorGetAllArguments : IArgumentsBase
	{
		public int ComponentId { get; set; }

		#region Constructors
		public ApiSensorGetAllArguments(int componentId)
		{
			ComponentId = componentId;
		}
		#endregion Constructors
	}
}
