using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class TranslationDeleteArguments : IArgumentsBase
	{
		public int Id			{ get; set; }

		#region Constructors
		public TranslationDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
