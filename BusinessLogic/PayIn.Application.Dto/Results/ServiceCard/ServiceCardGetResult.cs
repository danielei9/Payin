using System.Collections.Generic;
using PayIn.Common;

namespace PayIn.Application.Dto.Results
{
	public class ServiceCardGetResult
	{
		public enum ResultType
		{
			NotEmitted = 0,
			Principal = 1,
			Secondary = 2,
			Anonymous = 3,
			Linked = 4
		}

		public enum RelationType
		{
			IAmVinculatedUser = 0,
			IAmCardOwner = 1,
			IAmSystemCardManager = 2
		}

		public int Id { get; set; }
		public int PaymentConcessionId { get; set; }
		public string SystemCardName { get; set; }
		public ServiceCardState State { get; set; }
		public long Uid { get; set; }
		public string CardId { get; set; }
		public string UserPhoto { get; set; }
		public string UserName { get; set; }
		public string UserSurname { get; set; }
        public string Alias { get; set; }
        public ResultType CardType { get; set; }
		public IEnumerable<ServiceCardGetResult_PurseValue> PurseValues { get; set; }
		public IEnumerable<ServiceCardGetResult_PendingRecharge> PendingRecharges { get; set; }
		public IEnumerable<ServiceCardGetResult_Group> Groups { get; set; }
		public IEnumerable<ServiceCardGetResult_Entrance> Entrances { get; set; }
		public IEnumerable<ServiceCardGetResult_Operation> Operations { get; set; }
		public RelationType Relation { get; set; }
		public bool CanSellBalance { get; set; }
		public bool CanGiveBalance { get; set; }
		public bool CanBuyBalance { get; set; }
		public bool CanSellEntrance { get; set; }
		public bool CanGiveEntrance { get; set; }
		public bool CanBuyEntrance { get; set; }
		public bool InBlackList { get; set; }
	}
}
