using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public class ServiceCardMyCardsGetResult
	{
        public enum ResultType
        {
            NotEmitted = 0,
            Principal = 1,
            Secundaria = 2,
            Anonymous = 3
        }

		public int Id { get; set; }
		public string SystemCardName { get; set; }
        public string CardId { get; set; }
		public string UserPhoto { get; set; }
		public string UserName { get; set; }
		public string UserSurname { get; set; }
        public string Alias { get; set; }
        public ResultType CardType { get; set; }
		public IEnumerable<ServiceCardGetResult_PurseValue> PurseValues { get; set; }
		public IEnumerable<ServiceCardGetResult_Group> Groups { get; set; }
		public IEnumerable<ServiceCardGetResult_Entrance> Entrances { get; set; }
		public IEnumerable<ServiceCardGetResult_Operation> Operations { get; set; }
	}
}
