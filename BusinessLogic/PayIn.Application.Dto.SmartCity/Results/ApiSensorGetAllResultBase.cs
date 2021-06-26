using Xp.Application.Dto;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public class ApiSensorGetAllResultBase : ResultBase<ApiSensorGetAllResult>
	{
		public int? Id { get; set; }
		public string Name { get; set; }
	}
}
