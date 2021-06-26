namespace PayIn.Application.Dto.Results.SystemCard
{
	public class SystemCardGetAllResult
	{
       	public int Id { get; set; }
		public string Name { get; set; }
		public string Owner { get; set; }
		public string Profile { get; set; }
		public string CardType { get; set; }
		public string NumberGenerationType { get; set; }
		public int MembersCount { get; set; }
	}
}
