using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class MobileTicketAnswerFormsHandler : IServiceBaseHandler<TicketAnswerFormsArguments>
    {
        [Dependency] public IUnitOfWork UnitOfWork { get; set; }
        [Dependency] public IEntityRepository<Ticket> Repository { get; set; }
        [Dependency] public IEntityRepository<ControlForm> ControlFormRepository { get; set; }
        [Dependency] public IEntityRepository<ControlFormValue> ControlFormValueRepository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(TicketAnswerFormsArguments arguments)
		{
            var formsIds = arguments.Forms
                   .Select(y => y.Id);
            var formsDb = (await ControlFormRepository.GetAsync("Arguments.Options"))
                .Where(x =>
					x.State == ControlFormState.Active &&
					formsIds.Contains(x.Id)
                )
                .ToList();

            var answers = new Dictionary<int, ICollection<ControlFormValue>>(); // int=FormId
            foreach(var formDb in formsDb)
            {
                foreach (var argumentDb in formDb.Arguments
					.Where(x => x.State == ControlFormArgumentState.Active)
				)
                {
					var answer = arguments.Forms
						.Where(x => x.Id == formDb.Id)
						.SelectMany(x => x.Arguments
							.Where(y => y.Id == argumentDb.Id)
						)
						.FirstOrDefault();


					// Si es requerido, y no tiene valor, retornar error
					if ((argumentDb.MinOptions > 0) && (argumentDb.Type != ControlFormArgumentType.MultiOption))
                    {
						switch (argumentDb.Type)
						{
							case ControlFormArgumentType.Double:
							case ControlFormArgumentType.Int:
								if (answer.ValueNumeric == null)
									throw new ApplicationException("No se ha contestado a la pregunta {0}".FormatString(argumentDb.Name));
								break;

							case ControlFormArgumentType.Bool:
								if (answer.ValueBool == null)
									throw new ApplicationException("No se ha contestado a la pregunta {0}".FormatString(argumentDb.Name));
								break;

							case ControlFormArgumentType.Datetime:
							case ControlFormArgumentType.Date:
							case ControlFormArgumentType.Time:
							case ControlFormArgumentType.Duration:
								if (answer.ValueDateTime == null)
									throw new ApplicationException("No se ha contestado a la pregunta {0}".FormatString(argumentDb.Name));
								break;

							case ControlFormArgumentType.File:
							case ControlFormArgumentType.Image:
							default: // String
								if (answer.ValueString == null || answer.ValueString == "")
									throw new ApplicationException("No se ha contestado a la pregunta {0}".FormatString(argumentDb.Name));
								break;
						}
					}


					if (argumentDb.Type == ControlFormArgumentType.MultiOption)
					{
						var numOptions = 
							answer == null ? 0 :
							answer.Options == null ? 0 :
							answer.Options
                                .Where(x => x.ValueSelected)
                                .Count();

						if (numOptions < argumentDb.MinOptions)
							throw new ApplicationException(
                                "Se han de seleccionar al menos {1} y se han seleccionado {0} en la pregunta '{2}'"
                                .FormatString(
                                    numOptions,
                                    argumentDb.MinOptions,
                                    argumentDb.Name
                                )
                            );
						if ((argumentDb.MaxOptions != null) && (numOptions > argumentDb.MaxOptions))
							throw new ApplicationException(
                                "Se han de seleccionar como máximo {1} y se han seleccionado {0} en la pregunta {2}"
                                .FormatString(
                                        numOptions,
                                        argumentDb.MaxOptions,
                                    argumentDb.Name
                                )
                            );

					} else {
	
						if (answer != null && (answer.ValueBool != null || answer.ValueDateTime != null || answer.ValueNumeric != null || answer.ValueString != null))
						{
							switch (argumentDb.Type)
							{
								case ControlFormArgumentType.Datetime:
								case ControlFormArgumentType.Date:
								case ControlFormArgumentType.Time:
								case ControlFormArgumentType.Duration:
									if ((answer.ValueBool != null || answer.ValueNumeric != null || answer.ValueString != null) && (answer.ValueDateTime == null))
										throw new ApplicationException("El valor especificado en '{0}' no es válido".FormatString(argumentDb.Name));

									answer.ValueBool = null;
									answer.ValueNumeric = null;
									answer.ValueString = null;
									break;

								case ControlFormArgumentType.Double:
								case ControlFormArgumentType.Int:
									if ((answer.ValueBool != null || answer.ValueDateTime != null || answer.ValueString != null) && (answer.ValueNumeric == null))
										throw new ApplicationException("El valor especificado en '{0}' no es válido".FormatString(argumentDb.Name));

									answer.ValueBool = null;
									answer.ValueDateTime = null;
									answer.ValueString = null;
									break;

								case ControlFormArgumentType.Bool:
									if ((answer.ValueDateTime != null || answer.ValueNumeric != null || answer.ValueString != null) && (answer.ValueBool== null))
										throw new ApplicationException("El valor especificado en '{0}' no es válido".FormatString(argumentDb.Name));

									answer.ValueDateTime = null;
									answer.ValueNumeric = null;
									answer.ValueString = null;
									break;

								case ControlFormArgumentType.File:
								case ControlFormArgumentType.Image:
								default: //string
									if ((answer.ValueBool != null || answer.ValueNumeric != null || answer.ValueDateTime != null) && (answer.ValueString == null))
										throw new ApplicationException("El valor especificado en '{0}' no es válido".FormatString(argumentDb.Name));

									answer.ValueBool = null;
									answer.ValueNumeric = null;
									answer.ValueDateTime = null;
									break;
							}
						}

					}
                    
					// Create values
					var value = new ControlFormValue
					{
						ArgumentId = answer.Id,
						Observations = "",
						Target = ControlFormArgumentTarget.BuyEntrance,
						ValueNumeric = answer.ValueNumeric,
						ValueBool = answer.ValueBool,
						ValueDateTime = answer.ValueDateTime,
						ValueString = answer.ValueString
					};
					if (answers.ContainsKey(formDb.Id))
						answers[formDb.Id].Add(value);
					else
						answers.Add(formDb.Id, new List<ControlFormValue> { value });
						await ControlFormValueRepository.AddAsync(value);

						// Assign options
						if ((argumentDb.Type == ControlFormArgumentType.MultiOption) && (answer.Options != null))
						{
							var argumentsFormOptions = answer.Options
								.Where(x => x.ValueSelected)
								.Select(y => y.Id);

							value.ValueOptions = argumentDb.Options
								.Where(x => argumentsFormOptions.Contains(x.Id))
								.ToList();
							value.ValueNumeric = value.ValueOptions
								.Select(x => x.Value)
								.Sum();
						}
					
                }
            }
            await UnitOfWork.SaveAsync();

            // Asignar valores a las entradas
            var ticket = (await Repository.GetAsync(arguments.Id, "Lines.Entrances.EntranceType.EntranceTypeForms", "Lines.Entrances.FormValues"));
            if (ticket == null)
                throw new ArgumentNullException("id");

            foreach (var entrance in ticket.Lines.SelectMany(x => x.Entrances))
                foreach (var form in entrance.EntranceType.EntranceTypeForms)
                    foreach (var value in answers[form.FormId])
                        entrance.FormValues.Add(new EntranceFormValue
                        {
                            FormValueId = value.Id
                        });

            return ticket;
		}
        #endregion ExecuteAsync
    }
}
