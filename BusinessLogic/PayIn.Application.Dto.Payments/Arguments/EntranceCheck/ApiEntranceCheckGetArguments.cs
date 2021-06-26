using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class ApiEntranceCheckGetArguments : IArgumentsBase
    {
        public int Id { get; set; }

        #region Constructors
        public ApiEntranceCheckGetArguments(int id)
        {
            Id = id;
        }
        #endregion Constructors
    }
}