using PayIn.Application.Dto.Results.ServiceNumberPlate;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceNumberPlate
{
	public partial class ServiceNumberPlateUpdateArguments : IArgumentsBase
	{
		public int		Id					{ get; set; }
		public string NumberPlate	        { get; set; }
		public string Color				    { get; set; }
		public string Model				    { get; set; }

		#region Cast from ServiceNumberPlateGetAllResult
		public static implicit operator ServiceNumberPlateUpdateArguments(ServiceNumberPlateGetAllResult arguments)
		{
			return new ServiceNumberPlateUpdateArguments
			{
				Id = arguments.Id,
				NumberPlate = arguments.NumberPlate,
				Model = arguments.Model,
				Color = arguments.Color
			};
		}
		#endregion Cast from ServiceNumberPlateGetAllResult
	}
}
