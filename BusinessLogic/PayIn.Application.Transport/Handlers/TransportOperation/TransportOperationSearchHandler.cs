using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Transport.Services;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using PayIn.Infrastructure.Transport.Services;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.Public.Handlers
{
	[XpLog("TransportOperation", "Search", RelatedId = "Uids")]
	[XpAnalytics("TransportOperation", "Search", Response = new[] { "Data[0].Code", "Data[0].Name", "Data[1].Code", "Data[1].Name" })]
	public class TransportOperationSearchHandler : TransportCardSearchInternalHandler,
		IQueryBaseHandler<TransportOperationSearchArguments, ServiceCardReadInfoResult>
	{
		#region Constructors
		public TransportOperationSearchHandler(
			ISessionData sessionData,
			EigeService eigeService,
			SigapuntService sigapuntService,
			EmtService emtService,
			FgvService fgvService,
			EmailService emailService,
			IEntityRepository<TransportOperation> transportOperationRepository,
			IEntityRepository<TransportTitle> transportTitleRepository
		)
			:base(sessionData, eigeService, sigapuntService, emtService, fgvService, emailService, transportOperationRepository, transportTitleRepository)
		{
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ServiceCardReadInfoResult>> ExecuteAsync(TransportOperationSearchArguments arguments)
		{
			return await base.ExecuteAsync(arguments);
		}
		#endregion ExecuteAsync
	}
}
