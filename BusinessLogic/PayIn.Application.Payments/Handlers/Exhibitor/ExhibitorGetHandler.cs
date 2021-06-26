using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ExhibitorGetHandler :
        IQueryBaseHandler<ExhibitorGetArguments, ExhibitorGetResult>
    {
		private readonly IEntityRepository<Exhibitor> Repository;

		#region Constructors
		public ExhibitorGetHandler(
			IEntityRepository<Exhibitor> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ExhibitorGetResult>> ExecuteAsync(ExhibitorGetArguments arguments)
		{
            var exhibitor = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .Select(x => new ExhibitorGetResult
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    ConcessionId = x.PaymentConcessionId,
                    concessionName = x.PaymentConcession.Concession.Name,
                    Email= x.Email,
                    Phone = x.Phone,

					InvitationCode = x.InvitationCode,
					State = x.State,

					Code = x.Code,
					Contact = x.Contact,
					PostalCode = x.PostalCode,
					City = x.City,
					Province = x.Province,
					Country = x.Country,
					Fax = x.Fax,
					Url = x.Url,
					ContactEmail = x.ContactEmail,
					Pavilion = x.Pavilion,
					Stand = x.Stand

		});

            return new ResultBase<ExhibitorGetResult> { Data = exhibitor };
        }
		#endregion ExecuteAsync
	}
}
