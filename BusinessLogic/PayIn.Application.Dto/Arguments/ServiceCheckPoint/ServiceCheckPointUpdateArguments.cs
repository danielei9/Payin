using PayIn.Common;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceCheckPoint
{
	public partial class ServiceCheckPointUpdateArguments : IArgumentsBase
	{
																				   	  [Required]  public int            Id            { get; private set; }
	    [Display(Name = "resources.serviceCheckPoint.name")]	     							  public string Name { get; private set; }
		[Display(Name = "resources.serviceCheckPoint.observations")]                              public string Observations { get; private set; }
		[Display(Name = "resources.serviceCheckPoint.tag")]                     			      public int?	        TagId		  { get; private set; }
		[Display(Name = "resources.serviceCheckPoint.longitude")]                           	  public decimal? Longitude { get; private set; }
		[Display(Name = "resources.serviceCheckPoint.latitude")]                                  public decimal? Latitude { get; private set; }
		[Display(Name = "resources.enumResources.checkPointType_")]		              [Required]  public CheckPointType Type { get; private set; }		

		  
		#region Constructors
				   public ServiceCheckPointUpdateArguments(int id, string name, string observations, int? tagId, decimal? longitude, decimal? latitude, CheckPointType type) 
		{
			Id           = id;
			Name         = name;
			Observations = observations;
			Longitude    = longitude;
			Latitude     = latitude;
			TagId		 = tagId;
			Type		 = type;
		}
		#endregion Constructors
	}
}
