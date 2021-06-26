using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileNoticeGetArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public LanguageEnum? Language { get; set; } = LanguageEnum.Spanish;

		#region Constructors
		public MobileNoticeGetArguments(int id, LanguageEnum? language)
		{
			Id = id;
			Language = language ?? LanguageEnum.Spanish;
		}
		#endregion Constructors
	}
}