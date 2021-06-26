using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ExhibitorUpdateHandler :
		IServiceBaseHandler<ExhibitorUpdateArguments>
	{
		private readonly IEntityRepository<Exhibitor> Repository;

		#region Constructors
		public ExhibitorUpdateHandler(
			IEntityRepository<Exhibitor> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ExhibitorUpdateArguments arguments)
		{
            var exhibitor = (await Repository.GetAsync(arguments.OldId));

			exhibitor.Id = arguments.Id;

			exhibitor.Name = arguments.Name;
			exhibitor.Address = arguments.Address;
			exhibitor.Phone = arguments.Phone;
			exhibitor.Email = arguments.Email;
			exhibitor.InvitationCode = arguments.InvitationCode;
			exhibitor.Code = arguments.Code;
			exhibitor.Contact = arguments.Contact;
			exhibitor.PostalCode = arguments.PostalCode;
			exhibitor.City = arguments.City;
			exhibitor.Province = arguments.Province;
			exhibitor.Country = arguments.Country;
			exhibitor.Fax = arguments.Fax;
			exhibitor.Url = arguments.Url;
			exhibitor.ContactEmail = arguments.ContactEmail;
			exhibitor.Pavilion = arguments.Pavilion;
			exhibitor.Stand = arguments.Stand;

			exhibitor.PaymentConcessionId = arguments.PaymentConcessionId;

			return exhibitor;
		}
		#endregion ExecuteAsync
	}
}
