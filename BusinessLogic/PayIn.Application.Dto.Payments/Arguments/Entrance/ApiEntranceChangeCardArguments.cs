using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiEntranceChangeCardArguments : IArgumentsBase
	{
        [Required] public int Id { get; set; } // Entrance Id

		[Display(Name = "resources.entrance.changeCardEntrance_NewCardUid")]
		[Required] public string UidText { get; set; }

        #region Constructor
        public ApiEntranceChangeCardArguments(int id, string uidText)
		{
            Id = id;
            UidText = uidText;
        }
		#endregion Constructor
	}
}
