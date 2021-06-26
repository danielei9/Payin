using PayIn.Application.Dto.Transport.Arguments.TransportCard;
using PayIn.Application.Transport.Scripts;
using PayIn.BusinessLogic.Common;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Application.Public.Handlers
{
	[XpLog("TransportCard", "GetRecharge", RelatedId = "CardNumber")]
	public class TransportCardGetRechargeHandler :
		IQueryBaseHandler<TransportOperationGetRechargeArguments, IMifareOperation>
	{
		private readonly ISessionData SessionData;
		private readonly IMifareClassicHsmService Hsm;

		#region Constructors
		public TransportCardGetRechargeHandler(
			ISessionData sessionData,
			IMifareClassicHsmService hsm
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (hsm == null) throw new ArgumentNullException("hsm");

			SessionData = sessionData;
			Hsm = hsm;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<IMifareOperation>> ExecuteAsync(TransportCardGetRechargeArguments arguments)
		{
			var script = new TransportCardGetRechargeScript(SessionData.Login, Hsm);
			var request = await script.GetRequestAsync();

			return new ResultBase<IMifareOperation> { Data = request };
		}
		#endregion ExecuteAsync
	}
}
