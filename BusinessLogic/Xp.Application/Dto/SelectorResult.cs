using Newtonsoft.Json;

namespace Xp.Application.Dto
{
	public partial class SelectorResult
	{
		public int Id { get; set; }
		public string Value { get; set; }
        [JsonIgnore]
        public int? Order { get; set; }
	}
}
