using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class NoticeGetSelectorArguments : IArgumentsBase
    {
        public string Filter { get; set; }
        public int Id { get; set; }

        #region Constructors
        public NoticeGetSelectorArguments(string filter, int id)
        {
            Filter = filter ?? "";
            Id = id;
        }
        #endregion Constructors
    }
}