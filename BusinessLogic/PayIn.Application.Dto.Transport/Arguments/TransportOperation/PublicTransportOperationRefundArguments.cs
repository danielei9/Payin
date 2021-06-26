using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class PublicTransportOperationRefundArguments : IArgumentsBase
	{
        [JsonIgnore]
		                                     public int Id { get; set; }
		[Required(AllowEmptyStrings = true)] public string AuthorizationCode { get; set; }
		[Required(AllowEmptyStrings = true)] public string CommerceCode { get; set; }

		#region Constructors
		public PublicTransportOperationRefundArguments(string authorizationCode, string commerceCode)
		{
            AuthorizationCode = authorizationCode ?? "";
			CommerceCode = commerceCode ?? "";
		}
		#endregion Constructors
	}
}
