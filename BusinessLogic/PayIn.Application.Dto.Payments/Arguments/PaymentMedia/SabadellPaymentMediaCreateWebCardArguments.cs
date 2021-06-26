using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class SabadellPaymentMediaCreateWebCardArguments : IArgumentsBase
	{
		public string Ds_SignatureVersion { get; set; }
		public string Ds_MerchantParameters { get; set; }
		public string Ds_Signature { get; set; }

		#region Constructors
		public SabadellPaymentMediaCreateWebCardArguments(string ds_SignatureVersion, string ds_MerchantParameters, string ds_Signature)
		{
			this.Ds_SignatureVersion = ds_SignatureVersion;
			this.Ds_MerchantParameters = ds_MerchantParameters;
			this.Ds_Signature = ds_Signature;
		}
		#endregion Constructors
	}
}
