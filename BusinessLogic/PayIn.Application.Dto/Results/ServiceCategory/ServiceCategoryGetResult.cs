using PayIn.Domain.Public;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceCategoryGetResult
	{

		public int Id									{ get; set; }
		public string Name								{ get; set; }
		public int ConcessionId							{ get; set; }
		public string ConcessionName					{ get; set; }
		public IEnumerable<Group> Groups { get; set; }
		public bool AllMembersInSomeGroup { get; set; }
		public bool AMemberInOnlyOneGroup { get; set; }
		public bool AskWhenEmit { get; set; }
		public int? DefaultGroupWhenEmitId { get; set; }
		public string DefaultGroupWhenEmitName { get; set; }

		#region ServiceZoneResult
		public partial class Group
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public bool AllProduct { get; set; }
			public bool AllEntranceType { get; set; }
		}
		#endregion ServiceZoneResult

		#region Constructors
		public ServiceCategoryGetResult()
		{
			Groups = new List<Group>();
		}
		#endregion Constructors
	}
}