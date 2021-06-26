using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class ApiEntranceCheckGetAllArguments : IArgumentsBase
    {
        public int Id { get; set; }

        #region Constructors
        public ApiEntranceCheckGetAllArguments(int id)
        {
            Id = id;
        }
        #endregion Constructors
    }
}
