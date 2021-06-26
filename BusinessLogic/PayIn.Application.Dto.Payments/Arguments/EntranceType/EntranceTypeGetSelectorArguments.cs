using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class EntranceTypeGetSelectorArguments : IArgumentsBase
    {
        public string Filter { get; set; }

        #region Constructors
        public EntranceTypeGetSelectorArguments(string filter)
        {
            Filter = filter ?? "";
        }
        #endregion Constructors
    }
}