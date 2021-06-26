using PayIn.Application.Dto.Payments.Arguments.TicketTemplate;
using PayIn.Application.Dto.Payments.Results.TicketTemplate;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class TicketTemplateUpdateHandler :
		IServiceBaseHandler<TicketTemplateUpdateArguments>
	{
		private readonly IEntityRepository<TicketTemplate>Repository;

		#region Constructor
		public TicketTemplateUpdateHandler(
			IEntityRepository<TicketTemplate> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructor

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TicketTemplateUpdateArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			if (item == null)
				throw new ArgumentNullException("template");

			item.Name = arguments.Name;
			item.RegEx = arguments.RegEx;
			item.PreviousTextPosition = arguments.PreviousTextPosition;
			item.BackTextPosition = arguments.BackTextPosition;
			item.DateFormat = arguments.DateFormat;
			item.DecimalCharDelimiter = arguments.DecimalCharDelimiter;
			item.ReferencePosition = arguments.ReferencePosition ?? null;
			item.TitlePosition = arguments.TitlePosition ?? null;
			item.DatePosition = arguments.DatePosition ?? null;
			item.AmountPosition = arguments.AmountPosition;
			item.WorkerPosition = arguments.WorkerPosition ?? null;
			item.IsGeneric = arguments.IsGeneric ? true : false; 

			return item;
		}
		#endregion ExecuteAsync
	}
}
