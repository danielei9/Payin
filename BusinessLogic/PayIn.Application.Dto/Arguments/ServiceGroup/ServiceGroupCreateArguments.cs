using PayIn.Domain.Public;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceGroupCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.serviceGroup.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name				{ get; set; }
		public int	ServiceCategoryId	{ get; set; }

		#region Constructors
		public ServiceGroupCreateArguments(string name, int serviceCategoryId)
		{
			Name = name;
			ServiceCategoryId = serviceCategoryId;
		}
		#endregion Constructors
	}
}
