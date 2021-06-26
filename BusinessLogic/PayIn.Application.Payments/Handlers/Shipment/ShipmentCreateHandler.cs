using PayIn.Application.Dto.Payments.Arguments.Shipment;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Common.Resources;
using PayIn.Common;
using Xp.Common.Exceptions;

namespace PayIn.Application.Public.Handlers
{
	public class ShipmentCreateHandler :
		IServiceBaseHandler<ShipmentCreateArguments>
	{
		private readonly ISessionData SessionData;
		private readonly IEntityRepository<Shipment> Repository;	
	    private readonly IEntityRepository<PaymentConcession> RepositoryPaymentConcession;

	
		

		#region Constructors
		public ShipmentCreateHandler(
			ISessionData sessionData,
		    IEntityRepository<PaymentConcession> repositoryPaymentConcession,
			IEntityRepository<Shipment> repository
		)
		{
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryPaymentConcession == null) throw new ArgumentNullException("repositoryPaymentConcession");


			SessionData = sessionData;
			Repository = repository;
			RepositoryPaymentConcession = repositoryPaymentConcession;

		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ShipmentCreateArguments>.ExecuteAsync(ShipmentCreateArguments arguments)
		{
			var now = DateTime.Now.AddMinutes(-3).ToUTC();		
			
			var concessions = await RepositoryPaymentConcession.GetAsync("Concession.Supplier");
			var concession = concessions
				.Where(x => x.Concession.Supplier.Login == SessionData.Login).FirstOrDefault();

			if (arguments.Since == null )
				throw new Exception(ShipmentResources.NullSinceException);

			if (arguments.Until == null)
				throw new Exception(ShipmentResources.NullUntilException);
			
			if(arguments.Since.Value >= arguments.Until.Value)
				throw new Exception(ShipmentResources.UntilPreviousThanSinceException);


			if(arguments.Since.Value.ToUTC() < now)
			{
				throw new Exception(ShipmentResources.SinceInvalidDateException);
			}
			if (arguments.Until.Value.ToUTC() <= now)
			{
				throw new Exception(ShipmentResources.UntilInvalidDateException);
			}
			
			var shipment = new Shipment
			{
				Name = arguments.Name,
				Amount = arguments.Amount,
				Since = arguments.Since.Value.ToUTC(),
				Until = arguments.Until.Value.ToUTC(),
				Concession = concession,
				State = ShipmentState.Active
			};
			await Repository.AddAsync(shipment);			

			return shipment;
		}
		#endregion ExecuteAsync
	}
}

