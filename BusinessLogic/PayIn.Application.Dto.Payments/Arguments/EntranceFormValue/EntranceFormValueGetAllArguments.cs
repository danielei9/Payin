using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EntranceFormValueGetAllArguments : IArgumentsBase
	{
		public int EntranceId { get; set; }

		#region Constructors
		public EntranceFormValueGetAllArguments(int entranceId)
		{
			EntranceId = entranceId;
		}
		#endregion Constructors
	}
}

