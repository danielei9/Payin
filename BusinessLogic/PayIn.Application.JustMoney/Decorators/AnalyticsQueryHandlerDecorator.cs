using PayIn.BusinessLogic.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;
using Xp.Domain;
using Xp.Infrastructure;

namespace PayIn.Application.JustMoney.Decorators
{
	public class AnalyticsQueryHandlerDecorator<TArguments, TResult> : IQueryBaseHandler<TArguments, TResult>
		where TArguments : IArgumentsBase
	{
		public IQueryBaseHandler<TArguments, TResult> Handler { get; private set; }
		private readonly ISessionData SessionData;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IAnalyticsService Analytics;
		private readonly XpAnalyticsAttribute Attribute;

		#region Contructors
		public AnalyticsQueryHandlerDecorator(
			IQueryBaseHandler<TArguments, TResult> handler,
			ISessionData sessionData,
			IUnitOfWork unitOfWork,
			IAnalyticsService analytics,
			XpAnalyticsAttribute attribute
		)
		{
			Handler = handler ?? throw new ArgumentNullException("handler");
			SessionData = sessionData ?? throw new ArgumentNullException("sessionData");
			UnitOfWork = unitOfWork ?? throw new ArgumentNullException("unitOfWork");
			Analytics = analytics ?? throw new ArgumentNullException("analytics");
			Attribute = attribute ?? throw new ArgumentNullException("attribute");
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<ResultBase<TResult>> ExecuteAsync(TArguments arguments)
		{
			var result = await Handler.ExecuteAsync(arguments);
			await TrackAsync(arguments, result);
			return result;
		}
		#endregion ExecuteAsync

		#region TrackAsync
		private async Task TrackAsync(object arguments, object result)
		{
			try
			{
				var parameters = new Dictionary<string, object>();
				if (Attribute.Arguments != null)
					foreach (var argument in Attribute.Arguments)
					{
						var value = arguments.GetPropertyValue_Path(argument);
						if (value != null)
							parameters.Add("arguments." + argument.ToLower(), value);
                    }
                if (Attribute.Response != null)
                    foreach (var argument in Attribute.Response)
                    {
                        var name = "result." + argument;
                        var path = argument;

                        var values = argument.SplitString(":", 2);
                        if (values.Count() > 1)
                        {
                            name = values[0];
                            path = values[1];
                        }

                        var value = result.GetPropertyValue_Path(path);
                        if (value != null)
                            parameters.Add(name.ToLower(), value);
                    }

				await Analytics.TrackEventAsync(Attribute.RelatedClass + Attribute.RelatedMethod, parameters);
			}
			catch (Exception) { }
		}
		#endregion TrackAsync
	}
}
