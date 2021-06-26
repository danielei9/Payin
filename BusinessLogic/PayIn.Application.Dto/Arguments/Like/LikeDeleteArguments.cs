using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class LikeDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public LikeType? Type { get; set; }

		#region Constructors
		public LikeDeleteArguments(int id, LikeType? type)
		{
			Id = id;
			Type = type;
		}
		#endregion Constructors
	}
}
