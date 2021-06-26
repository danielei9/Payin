namespace PayIn.Application.Dto.Results.Main
{
	public partial class MainMobileGetVersionResult
	{
        public string PublicServerVersion { get; set; }
        public string PublicDbVersion { get; set; }
        public string InternalDbVersion { get; set; }
    }
}
