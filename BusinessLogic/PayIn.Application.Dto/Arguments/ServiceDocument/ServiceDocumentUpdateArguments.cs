using PayIn.Domain.Public;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceDocumentUpdateArguments : IUpdateArgumentsBase<ServiceDocument>
	{
		                                                                                          public int        Id    { get; set; }
		[Display(Name = "resources.serviceDocument.name")]  [Required(AllowEmptyStrings = false)] public string     Name  { get; set; }
		[Display(Name = "resources.serviceDocument.start")]                                       public XpDateTime Since { get; set; }
		[Display(Name = "resources.serviceDocument.end")]                                         public XpDateTime Until   { get; set; }

		#region Constructors
		public ServiceDocumentUpdateArguments(string name, XpDateTime since, XpDateTime until)
		{
			Since = since ?? XpDateTime.MinValue;
			Until = until ?? XpDateTime.MaxValue;
			Name = name ?? "";
		}
		#endregion Constructos
	}
}
