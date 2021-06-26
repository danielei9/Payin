using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiEntranceGetAllArguments : IArgumentsBase
	{
        public int Id       { get; set; }
        
        #region Constructors
        public ApiEntranceGetAllArguments  (int id)
		{
            Id = id;
        }
		#endregion Constructors
	}
}
