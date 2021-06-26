using PayIn.Application.Dto.Arguments.ControlPlanning;
using PayIn.Common.Resources;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Common.Exceptions;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPlanningCreateTemplateHandler :
		IServiceBaseHandler<ControlPlanningCreateTemplateArguments>
	{
		private readonly IEntityRepository<ControlPlanning>     Repository;
		private readonly IEntityRepository<ControlTemplate>     RepositoryTemplate;
		private readonly IEntityRepository<ControlPlanningItem> RepositoryPlanningItem;

		#region Constructors
		public ControlPlanningCreateTemplateHandler(
			IEntityRepository<ControlPlanning>     repository,
			IEntityRepository<ControlPlanningItem> repositoryPlanningItem,
			IEntityRepository<ControlTemplate>     repositoryTemplate
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (repositoryPlanningItem == null) throw new ArgumentNullException("repositoryPlanningItem");
			if (repositoryTemplate == null) throw new ArgumentNullException("repositoryTemplate");

			Repository = repository;
			RepositoryPlanningItem = repositoryPlanningItem;
			RepositoryTemplate = repositoryTemplate;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(ControlPlanningCreateTemplateArguments arguments)
		{
			if (arguments.DateSince.Value > arguments.DateUntil.Value)
				throw new XpException(ControlPlanningResources.SincePreviousUntilException);
			if (arguments.DateSince.Value.AddMonths(6) < DateTime.Now)
				throw new XpException(ControlPlanningResources.OldSinceException);
			if (DateTime.Now.AddMonths(12) < arguments.DateUntil)
				throw new XpException(ControlPlanningResources.NewUntilException);

			var template = await RepositoryTemplate.GetAsync(arguments.TemplateId, "TemplateItems", "Checks.FormAssignTemplates.Form.Arguments");

			var date = arguments.DateSince.Value.Date;
			var formAssigns = new List<ControlFormAssign>();
			while (date <= arguments.DateUntil)
			{
				if ((template != null) && (
					((date.DayOfWeek == DayOfWeek.Monday) && !template.Monday) ||
					((date.DayOfWeek == DayOfWeek.Tuesday) && !template.Tuesday) ||
					((date.DayOfWeek == DayOfWeek.Wednesday) && !template.Wednesday) ||
					((date.DayOfWeek == DayOfWeek.Thursday) && !template.Thursday) ||
					((date.DayOfWeek == DayOfWeek.Friday) && !template.Friday) ||
					((date.DayOfWeek == DayOfWeek.Saturday) && !template.Saturday) ||
					((date.DayOfWeek == DayOfWeek.Sunday) && !template.Sunday)
				))
				{
					date = date.AddDays(1);
					continue;
				}

				var item = (await Repository.GetAsync("Items", "Checks"))
					.Where(x => 
						(x.WorkerId   == arguments.WorkerId) &&
						(x.ItemId     == template.ItemId) &&
						(x.Date.Year  == date.Year) &&
						(x.Date.Month == date.Month) &&
						(x.Date.Day   == date.Day)
					).
					FirstOrDefault();
				if (item == null)
				{
					item = new ControlPlanning
					{
						Date = date,
						ItemId = template.ItemId,
						WorkerId = arguments.WorkerId
					};
					await Repository.AddAsync(item);
				}
				item.CheckDuration = template.CheckDuration;

				// Copiar items
				foreach (var templateItem in template.TemplateItems)
				{
					var planningItem = CopyItem(templateItem, item.Date);

					item.Items.Add(planningItem);
					item.Checks.Add(planningItem.Since);
					item.Checks.Add(planningItem.Until);

					formAssigns.AddRange(planningItem.Since.FormAssigns);
					formAssigns.AddRange(planningItem.Until.FormAssigns);
				}
				var templateChecks = template.TemplateItems.SelectMany(y => new[] { y.SinceId, y.UntilId });

				// Copiar checks
				foreach (var templateCheck in template.Checks.Where(x => !templateChecks.Contains(x.Id)))
				{
					var check = CopyCheck(templateCheck, item.Date);

					item.Checks.Add(check);

					formAssigns.AddRange(check.FormAssigns);
				}

				date = date.AddDays(1);
			}

			return formAssigns;
		}
		#endregion ExecuteAsync

		#region CopyItem
		public ControlPlanningItem CopyItem(ControlTemplateItem itemTemplate, DateTime date)
		{
			var planningSince = CopyCheck(itemTemplate.Since, date);
			var planningUntil = CopyCheck(itemTemplate.Until, itemTemplate.Until.Time < itemTemplate.Since.Time ? date.AddDays(1) : date);

			var planningItem = new ControlPlanningItem
			{
				Since = planningSince,
				Until = planningUntil
			};

			return planningItem;
		}
		#endregion CopyItem

		#region CopyCheck
		public ControlPlanningCheck CopyCheck(ControlTemplateCheck checkTemplate, DateTime date)
		{
			var localDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Local);
			var time = (XpTime)checkTemplate.Time;
			var localDateTime = date.Add(time.Value.Value);

			var check = new ControlPlanningCheck
			{
				Date = localDateTime.ToUTC(),
				CheckPointId = checkTemplate.CheckPointId
			};

			foreach (var formTemplate in checkTemplate.FormAssignTemplates)
				check.FormAssigns.Add(CopyForm(formTemplate));

			return check;
		}
		#endregion CopyCheck

		#region CopyForm
		public ControlFormAssign CopyForm(ControlFormAssignTemplate formTemplate)
		{
			var form = new ControlFormAssign
			{
				Form = formTemplate.Form
			};

			foreach (var argument in formTemplate.Form.Arguments)
				form.Values.Add(CopyArgument(argument));

			return form;
		}
		#endregion CopyForm

		#region CopyArgument
		public ControlFormValue CopyArgument(ControlFormArgument argument)
		{
			var value = new ControlFormValue
			{
				ArgumentId = argument.Id,
				Observations = argument.Observations,
				Target = argument.Target,
			};
			return value;
		}
		#endregion CopyArgument
	}
}
