using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class ApiEntranceGetArguments : IArgumentsBase
    {
        public int Id { get; set; }

        #region Constructors
        public ApiEntranceGetArguments(int id)
        {
            Id = id;
        }
        #endregion Constructors
    }
}