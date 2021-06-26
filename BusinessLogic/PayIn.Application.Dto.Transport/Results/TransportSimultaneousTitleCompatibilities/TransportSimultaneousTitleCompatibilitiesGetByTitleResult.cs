namespace PayIn.Application.Dto.Transport.Results.TransportSimultaneousTitleCompatibilities
{
	public class TransportSimultaneousTitleCompatibilitiesGetByTitleResult
	{
		public int Id { get; set; }
        public int IdSimultaneous { get; set; }
        public int Code { get; set; }
		public string Name { get; set; }
		public int? OwnerCode { get; set; }
		public string OwnerName { get; set; }
		public string EnvironmentAlias { get; set; }
	}
}
