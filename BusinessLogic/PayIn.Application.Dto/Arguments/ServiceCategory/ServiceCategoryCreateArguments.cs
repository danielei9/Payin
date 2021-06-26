using PayIn.Domain.Public;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCategoryCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.serviceCategory.name")]	[Required(AllowEmptyStrings = false)]	public string		Name { get; set; }
		[Display(Name = "resources.serviceCategory.allMembersInSomeGroup")]							public bool			AllMembersInSomeGroup { get; set; }
		[Display(Name = "resources.serviceCategory.aMemberInOnlyOneGroup")]							public bool			AMemberInOnlyOneGroup { get; set; }
		[Display(Name = "resources.serviceCategory.askWhenEmit")]									public bool			AskWhenEmit { get; set; }
		[Display(Name = "resources.serviceCategory.defaultGroupWhenEmit")]							public int?			DefaultGroupWhenEmitId { get; set; }
		//[Display(Name = "resources.serviceCategory.defaultGroupWhenEmit")]							public ServiceGroup DefaultGroupWhenEmit { get; set; }
		//[Display(Name = "resources.serviceCategory.defaultGroupWhenEmit")]							public string		DefaultGroupWhenEmitName { get; set; }

		//public int? DefaultGroupWhenEmitId { get; set; }


		#region Constructors
		public ServiceCategoryCreateArguments(string name, bool allMembersInSomeGroup, bool aMemberInOnlyOneGroup, bool askWhenEmit, int? defaultGroupWhenEmitId) //, ServiceGroup defaultGroupWhenEmit, string defaultGroupWhenEmitName) //, int serviceConcessionId)
		{
			Name = name;
			//ServiceConcessionId = serviceConcessionId;
			AllMembersInSomeGroup = allMembersInSomeGroup;
			AMemberInOnlyOneGroup = aMemberInOnlyOneGroup;
			AskWhenEmit = askWhenEmit;
			DefaultGroupWhenEmitId = defaultGroupWhenEmitId;
			//DefaultGroupWhenEmit = defaultGroupWhenEmit;
			//DefaultGroupWhenEmitName = defaultGroupWhenEmitName;
		}
		#endregion Constructors
	}
}
