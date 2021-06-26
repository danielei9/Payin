using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class ApiAccessControlEntryCreateArguments : IArgumentsBase
	{
		public int AccessControlId { get; set; }
		public int EntranceId { get; set; }

		[Display(Name = "resources.accessControl.entrance.in")]
		public bool EntryIn { get; set; }

		[Display(Name = "resources.accessControl.entrance.out")]
		public bool EntryOut { get; set; }

		[Display(Name = "resources.accessControl.entrance.people")]
		public int People { get; set; }

		#region Constructors

		public ApiAccessControlEntryCreateArguments(int entranceId, bool entryIn, bool entryOut, int people)
		{
			EntranceId = entranceId;
			EntryIn = entryIn;
			EntryOut = entryOut;
			People = people;
		}

		#endregion
	}
}
