using Newtonsoft.Json;

namespace Xp.Application.Dto
{
	public partial class SelectorLongResult
	{
		public long Id { get; set; }
		public string Value { get; set; }
        [JsonIgnore]
        public int? Order { get; set; }
	}
}
