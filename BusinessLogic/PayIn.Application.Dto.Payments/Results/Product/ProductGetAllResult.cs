using System.Collections.Generic;
using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class ProductGetAllResult
	{
		public enum TypeEnum
		{
			Product     = 1,
			Family      = 2
		}

		public int Id							             { get; set; }
		public string Name						             { get; set; }
		public string Description				             { get; set; }
		public string PhotoUrl					             { get; set; }
		public decimal? Price					             { get; set; }
		public int? SuperFamilyId				             { get; set; }
		public TypeEnum Type					             { get; set; }
		public string TreeId					             { get; set; }
		public int TreeLevel					             { get; set; }
		public bool IsVisible					             { get; set; }
        public string Code						             { get; set; }
		public ProductVisibility Visibility		             { get; set; }
		public int GroupsCount								 { get; set; }
	}
}
