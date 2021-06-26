using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ApiEntranceValidationResult
    {
        public int Id { get; set; }
        public long Code { get; set; }
        public string UserName { get; set; }
        public string VatNumber { get; set; }
        public string Login { get; set; }
        public EntranceState State { get; set; }
        public int EntraceTypeId { get; set; }
        public int TicketLineId { get; set; }
    }
}