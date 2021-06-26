using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class EntranceGenerateMailArguments : IArgumentsBase
	{
        public int Id       { get; set; }
        
        #region Constructors
        public EntranceGenerateMailArguments(int id)
		{
            Id = id;
        }
		#endregion Constructors
	}
}
