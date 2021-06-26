using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceDocumentCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.serviceDocument.name")]       [Required(AllowEmptyStrings = false)]                              public string     Name         { get; set; }
		[Display(Name = "resources.serviceDocument.since")]                                                                         public XpDateTime Since        { get; set; }
		[Display(Name = "resources.serviceDocument.until")]                                                                         public XpDateTime Until        { get; set; }
		[Display(Name = "resources.serviceDocument.url")]        [DataSubType(DataSubType.Pdf)]                                     public byte[]     Url          { get; set; }
		[Display(Name = "resources.serviceDocument.systemCard")]                                                                    public int        SystemCardId { get; set; }

		#region Constructors
		public ServiceDocumentCreateArguments(string name, XpDateTime since, XpDateTime until, byte[] url, int systemCardId)
		{
			Name = name;
			Since = since ?? XpDateTime.MinValue;
			Until = until ?? XpDateTime.MaxValue;
			Url = url;
			SystemCardId = systemCardId;
		}
		#endregion Constructors
	}
}
