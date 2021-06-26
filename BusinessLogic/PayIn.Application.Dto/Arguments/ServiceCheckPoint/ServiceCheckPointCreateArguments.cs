using PayIn.Common;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceCheckPoint
{
	public partial class ServiceCheckPointCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.serviceCheckPoint.tag")]								public int?           TagId        { get; private set; }
		[Display(Name = "resources.serviceCheckPoint.name")]			[Required]		public string         Name         { get; private set; }
		[Display(Name = "resources.serviceCheckPoint.observations")]					public string         Observations { get; private set; }
		[Display(Name = "resources.serviceCheckPoint.longitude")]		[Required]		public decimal?       Longitude    { get; private set; }
		[Display(Name = "resources.serviceCheckPoint.latitude")]		[Required]		public decimal?       Latitude     { get; private set; }
		[Display(Name = "resources.serviceCheckPoint.supplier")]  		[Required]		public int            SupplierId   { get; private set; }
		[Display(Name = "resources.enumResources.checkPointType_")]		[Required]		public CheckPointType Type         { get; private set; }		
		[Display(Name = "resources.serviceCheckPoint.item")]     		[Required]	    public int            ItemId       { get; private set; }


		#region Constructors
		public ServiceCheckPointCreateArguments(string name, string observations, decimal longitude, decimal latitude, int supplierId, CheckPointType type, int? tagId, int itemId)
		{
			Name         = name;
			Observations = observations;
			Longitude    = longitude;
			Latitude	 = latitude;
			SupplierId   = supplierId;
			Type         = type;			
			TagId        = tagId;
			ItemId       = itemId;
		}
		#endregion Constructors

		
	}
}
