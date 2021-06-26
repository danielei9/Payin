using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class MobileEventGetSelectorOpenedArguments : IArgumentsBase
    {
        public string Filter { get; set; }

        #region Constructors
        public MobileEventGetSelectorOpenedArguments(string filter)
        {
            Filter = filter ?? "";
        }
        #endregion Constructors
    }
}