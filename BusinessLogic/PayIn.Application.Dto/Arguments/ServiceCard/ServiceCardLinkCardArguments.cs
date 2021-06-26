using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
    public class ServiceCardLinkCardArguments : IArgumentsBase
	{
		[Display(Name = "resources.serviceCard.uid")]	public int		Id			{ get; set; }
		[Display(Name = "resources.serviceCard.uid")]	public string	UidText		{ get; set; }

		#region Constructors
		public ServiceCardLinkCardArguments(int id, string uidText)
		{
			Id = id;
			UidText = uidText;
		}
		#endregion Constructors
	}
}
