using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceNumberPlate
{
	public partial class ServiceNumberPlateCreateArguments : IArgumentsBase
	{
		public string NumberPlate	    { get; set; }
		public string Color				{ get; set; }
		public string Model				{ get; set; }

		#region Constructors
		public ServiceNumberPlateCreateArguments(string numberPlate, string color, string model)
		{
			NumberPlate = numberPlate;
			Color = color;
			Model = model;
		}
		public ServiceNumberPlateCreateArguments()
		{
		}
		#endregion Constructors
	}
}
