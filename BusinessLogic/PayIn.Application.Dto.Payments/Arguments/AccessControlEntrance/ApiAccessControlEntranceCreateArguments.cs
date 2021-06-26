using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class ApiAccessControlEntranceCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.accessControl.entrance.accessControlId")]
		public int AccessControlId { get; set; }

		[Display(Name = "resources.accessControl.entrance.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		#region Constructors

		public ApiAccessControlEntranceCreateArguments(int accessControlId, string name)
		{
			AccessControlId = accessControlId;
			Name = name;
		}

		#endregion
	}
}
