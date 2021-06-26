using PayIn.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceDocument : Entity
	{
		                                      public ServiceDocumentState      State { get; set; }
		                                      public DateTime                  Since { get; set; }
		                                      public DateTime                  Until { get; set; }
		[Required(AllowEmptyStrings = false)] public string                    Name { get; set; }
		[Required(AllowEmptyStrings = false)] public string                    Url { get; set; }
		                                      public bool                      IsVisible { get; set; }
		                                      public ServiceDocumentVisibility Visibility { get; set; }

		#region SystemCard
		public int SystemCardId { get; set; }
		[ForeignKey(nameof(ServiceDocument.SystemCardId))]
		public SystemCard SystemCard { get; set; }
		#endregion SystemCard
	}
}
