using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class MobileActivityGetByEventArguments : IArgumentsBase
    {
        public int Id { get; set; }

        #region Constructors
        public MobileActivityGetByEventArguments(int id)
        {
            Id = id;
        }
        #endregion Constructors
    }
}
