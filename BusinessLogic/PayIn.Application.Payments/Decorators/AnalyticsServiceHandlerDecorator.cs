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

namespace PayIn.Application.Payments.Decorators
{
	public class AnalyticsServiceHandlerDecorator<TArguments> : IServiceBaseHandler<TArguments>
		where TArguments : IArgumentsBase
	{
		public IServiceBaseHandler<TArguments> Handler { get; private set; }
		private readonly ISessionData SessionData;
		private readonly IUnitOfWork UnitOfWork;
		private readonly IAnalyticsService Analytics;
		private readonly XpAnalyticsAttribute Attribute;

		#region Contructors
		public AnalyticsServiceHandlerDecorator(
			IServiceBaseHandler<TArguments> handler,
			ISessionData sessionData,
			IUnitOfWork unitOfWork,
			IAnalyticsService analytics,
			XpAnalyticsAttribute attribute
		)
		{
			if (handler == null) throw new ArgumentNullException("handler");
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (analytics == null) throw new ArgumentNullException("analytics");
			if (attribute == null) throw new ArgumentNullException("attribute");

			Handler = handler;
			SessionData = sessionData;
			UnitOfWork = unitOfWork;
			Analytics = analytics;
			Attribute = attribute;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TArguments arguments)
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
					foreach(var argument in Attribute.Arguments)
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
