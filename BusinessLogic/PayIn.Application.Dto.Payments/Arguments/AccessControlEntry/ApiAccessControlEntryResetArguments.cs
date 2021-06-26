using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class ApiAccessControlEntryResetArguments : IArgumentsBase
	{
		public int AccessControlId { get; set; }

		#region Constructors

		public ApiAccessControlEntryResetArguments(int accessControlId)
		{
			AccessControlId = accessControlId;
		}

		#endregion
	}
}
