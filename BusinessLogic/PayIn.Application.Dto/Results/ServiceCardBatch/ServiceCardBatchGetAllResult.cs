using PayIn.Common;

namespace PayIn.Application.Dto.Results
{
	public class ServiceCardBatchGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string SystemCardName { get; set; }
		public ServiceCardBatchState State { get; set; }
        public string UidFormat { get; set; }
    }
}
