using PayIn.Common;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceTag
{
	public partial class ServiceTagUpdateArguments : IUpdateArgumentsBase<PayIn.Domain.Public.ServiceTag>
	{
		                                                                         public int            Id        { get; private set; }
	    [Display(Name = "resources.enumResources.checkPointType_")]   [Required] public CheckPointType Type { get; private set; }
		[Display(Name = "resources.serviceTag.reference")]	          [Required] public string         Reference { get; private set; }

		#region Constructors
		public ServiceTagUpdateArguments(CheckPointType type, string reference, int id)
		{
			Id = id;		
			Type = type;
			Reference = reference;
		}
		#endregion Constructors
	}
}