using PayIn.Domain.Public;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceCategoryGetAllResult
	{
		public int Id							{ get; set; }
		public string Name						{ get; set; }
		public int ConcessionId					{ get; set; }
		public string ConcessionName			{ get; set; }
		public IEnumerable<ServiceCategoryGetAll_GroupResult> Groups { get; set; }
		public bool AllMembersInSomeGroup { get; set; }
		public bool AMemberInOnlyOneGroup { get; set; }
		public bool AskWhenEmit { get; set; }
		public int? DefaultGroupWhenEmitId { get; set; }

		#region Constructors
		public ServiceCategoryGetAllResult()
		{
			Groups = new List<ServiceCategoryGetAll_GroupResult>();
		}
		#endregion Constructors

	}
}