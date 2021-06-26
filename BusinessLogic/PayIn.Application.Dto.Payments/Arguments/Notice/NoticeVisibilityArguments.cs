using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class NoticeVisibilityArguments : IArgumentsBase
	{
														public int Id { get; set; }
		[Display(Name = "resources.notice.visibility")]	public NoticeVisibility Visibility { get; set; }

		#region Constructors
		public NoticeVisibilityArguments(int id, NoticeVisibility visibility)
		{
			Id = id;
			Visibility = visibility;
		}
		#endregion Constructors
	}
}