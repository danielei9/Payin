using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public partial class MobileMainGetAllV4Result
	{
		public string Name { get; set; }
		public string IconUrl { get; set; }
		public string BackgroundUrl { get; set; }
		public string Color { get; set; }
		public int? PaymentConcessionId { get; set; }
		public int? SystemCardId { get; set; }
		public TimeSpan? SystemCardSynchronizationInterval { get; set; }
		public string SystemCardPhotoUrl { get; set; }
		public int? EventId { get; set; }
        public IEnumerable<MobileMainGetAllV4Result_Row> Layout { get; set; }
	}
}
