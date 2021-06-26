using System.Collections.Generic;

namespace PayIn.Presentation.Design
{
	public class HubGetD
	{
		public IEnumerable<HubGet_ItemD> Favorites { get; set; }
		public IEnumerable<HubGet_ItemD> Data { get; set; }
	}
	public class HubGet_ItemD
	{
		public bool IsFavorite { get; set; }
		public string Title { get; set; }
		public string Subtitle { get; set; }
		public decimal Balance { get; set; }
	}
}
