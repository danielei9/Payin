using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceCategoryUpdateArguments : IUpdateArgumentsBase<ServiceCategory>
	{
												public int							Id						{ get; set; }
		[Display(ResourceType=typeof(ServiceCategoryResources), Name = "Name")]
		[Required(AllowEmptyStrings = false)]	public string						Name					{ get; set; }
		[Display(Name = "resources.serviceCategory.allMembersInSomeGroup")]
												public bool							AllMembersInSomeGroup	{ get; set; }
		[Display(Name = "resources.serviceCategory.aMemberInOnlyOneGroup")]
												public bool							AMemberInOnlyOneGroup	{ get; set; }
		[Display(Name = "resources.serviceCategory.askWhenEmit")]
												public bool							AskWhenEmit				{ get; set; }
		[Display(Name = "resources.serviceCategory.defaultGroupWhenEmit")]
												public int?							DefaultGroupWhenEmitId	{ get; set; }

		[Display(Name = "resources.serviceCategory.defaultGroupWhenEmit")]
												public string						DefaultGroupWhenEmitName { get; set; }

		#region Constructors
		public ServiceCategoryUpdateArguments(int id, string name, bool allMembersInSomeGroup, bool aMemberInOnlyOneGroup, bool askWhenEmit, int? defaultGroupWhenEmitId, string defaultGroupWhenEmitName)
		{
			Id = id;
			Name = name;
			AllMembersInSomeGroup = allMembersInSomeGroup;
			AMemberInOnlyOneGroup = aMemberInOnlyOneGroup;
			AskWhenEmit = askWhenEmit;
			DefaultGroupWhenEmitId = defaultGroupWhenEmitId;
			DefaultGroupWhenEmitName = defaultGroupWhenEmitName;
		}
		#endregion Constructos
	}
}
