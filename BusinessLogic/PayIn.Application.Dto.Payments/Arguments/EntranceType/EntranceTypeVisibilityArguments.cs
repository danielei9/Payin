using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EntranceTypeVisibilityArguments : IArgumentsBase
	{
														public int Id { get; set; }
		[Display(Name = "resources.entranceType.visibility")]  public EntranceTypeVisibility Visibility { get; set; }

		#region Constructors
		public EntranceTypeVisibilityArguments(int id, EntranceTypeVisibility visibility)
		{
			Id = id;
			Visibility = visibility;
		}
		#endregion Constructors
	}
}
