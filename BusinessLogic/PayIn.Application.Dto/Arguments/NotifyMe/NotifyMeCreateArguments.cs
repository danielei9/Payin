using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class NotifyMeCreateArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public NotifyMeType? Type { get; set; }

		#region Constructors
		public NotifyMeCreateArguments(int id, NotifyMeType? type)
		{
			Id = id;
			Type = type;
		}
		#endregion Constructors
	}
}
