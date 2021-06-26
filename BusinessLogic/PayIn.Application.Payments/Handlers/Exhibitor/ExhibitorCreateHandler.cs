using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ExhibitorCreateHandler :
		IServiceBaseHandler<ExhibitorCreateArguments>
	{
		private readonly IEntityRepository<Exhibitor> Repository;
        private readonly IEntityRepository<Event> EventRepository;

        #region Constructors
        public ExhibitorCreateHandler(
			IEntityRepository<Exhibitor> repository,
            IEntityRepository<Event> eventRepository
        )
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
            if (eventRepository == null) throw new ArgumentNullException("repository");
            EventRepository = eventRepository;
        }
        #endregion Constructors

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(ExhibitorCreateArguments arguments)
        {
            var eventAssing = (await EventRepository.GetAsync(arguments.EventId));
            if (eventAssing == null)
                throw new ArgumentNullException("EventId");

			var exhibitor = new Exhibitor(
				name: arguments.Name,
				address: arguments.Address,
				phone: arguments.Phone,
				email: arguments.Email,
				invitationCode: "",
				code: arguments.Code,
				contact: arguments.Contact,
				postalCode: arguments.PostalCode,
				city: arguments.City,
				province: arguments.Province,
				country: arguments.Country,
				fax: arguments.Fax,
				url: arguments.Url,
				contactEmail: arguments.ContactEmail,
				pavilion: arguments.Pavilion,
				stand: arguments.Stand
			)
			{
				PaymentConcessionId = arguments.PaymentConcessionId,
			};
			await Repository.AddAsync(exhibitor);

			eventAssing.Exhibitors.Add(exhibitor);

            return exhibitor;
		}
		#endregion ExecuteAsync
	}
}
