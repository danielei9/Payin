using System;
using Xp.Common;
using PayIn.Common;

namespace PayIn.Application.Dto.Results.Account
{
	public partial class AccountGetCurrentResult
	{
        public string Name { get; set; }        
        public string Mobile { get; set; }        
        public SexType Sex { get; set; }       
        public string TaxNumber { get; set; }       
        public string TaxName { get; set; }        
        public string TaxAddress { get; set; }       
        public XpDate Birthday { get; set; }
		public string PhotoUrl { get; set; }
		
	}
}
