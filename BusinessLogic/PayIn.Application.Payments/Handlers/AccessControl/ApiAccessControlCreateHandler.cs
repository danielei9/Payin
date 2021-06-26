using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ApiAccessControlCreateHandler : IServiceBaseHandler<ApiAccessControlCreateArguments>
	{
		[Dependency] public IEntityRepository<AccessControl> Repository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync

		public async Task<dynamic> ExecuteAsync(ApiAccessControlCreateArguments arguments)
		{
			var paymentConcession = (await PaymentConcessionRepository.GetAsync())
				.Where(x =>
					x.Concession.Supplier.Login == SessionData.Login &&
					x.Concession.State == ConcessionState.Active
				)
				.FirstOrDefault();

			var access = new AccessControl()
			{
				Name = arguments.Name,
				Schedule = arguments.Schedule ?? "",
				CurrentCapacity = 0,
				MaxCapacity = arguments.MaxCapacity,
				MapUrl = arguments.Map,
				PaymentConcession = paymentConcession,
			};

            await Repository.AddAsync(access);

			return access;
		}

		#endregion
	}
}
