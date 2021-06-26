using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceDocumentGetResult
	{
		public int	      Id       { get; set; }
		public XpDateTime Since    { get; set; }
		public XpDateTime Until    { get; set; }
		public string     Name     { get; set; }
		public string     Url      { get; set; }
	}
}