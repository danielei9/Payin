using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Internal.Decorators;
using PayIn.Application.Internal.Handlers;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Internal.Infrastructure;
using PayIn.Infrastructure.Internal;
using PayIn.Infrastructure.Internal.Adapters;
using PayIn.Infrastructure.Internal.Db;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Unity.WebApi;
using Xp.Application.Attributes;
using Xp.Application.Decorators;
using Xp.Application.Dto;
using Xp.Application.Handlers;
using Xp.Common.Dto.Arguments;
using Xp.Domain;
using Xp.Infrastructure;
using Xp.Infrastructure.Repositories;
using Xp.Infrastructure.Services;

namespace PayIn.Common.DI.Internal
{
	public static class DIConfig
	{
		#region Container
		private static IUnityContainer _Container;
		private static IUnityContainer Container
		{
			get
			{
				if (_Container == null)
					_Container = CreateContainer();

				return _Container;
			}
		}
		#endregion Container

		#region GetAssignsFromInterface
		private static IEnumerable<KeyValuePair<Type, Type>> GetAssignsFromInterface(Type type)
		{
			var assemblies = new List<Assembly>() {
				typeof(UserUpdatePinHandler).Assembly, // PayIn.Application.Internal
				typeof(UserUpdatePinArguments).Assembly, // PayIn.Application.Dto
				typeof(InternalUnitOfWork).Assembly // PayIn.Repository
			};

			var types = assemblies
				.SelectMany(x => x.DefinedTypes
					.Where(t => 
						!t.IsAbstract && 
						!t.IsGenericType && 
						t.IsNested != true
					)
					.Select(t => t.AsType())
				)
				.ToList()
			;

			if (type.IsGenericTypeDefinition)
			{
				var result = types
					.SelectMany(t => t.GetTypeInfo().ImplementedInterfaces
						.Select(t2 => new { Type = t, Interfaz = t2 })
						.Where(i => i.Interfaz.IsConstructedGenericType && i.Interfaz.GetGenericTypeDefinition() == type)
					)
					.Select(x => new KeyValuePair<Type, Type>(x.Interfaz, x.Type))
					.ToList()
				;
				return result;
			}
			else
			{
				var result = types
					.SelectMany(t => t.GetTypeInfo().ImplementedInterfaces
						.Where(i => i == type)
						.Select(t2 => new { Type = t, Interfaz = t2 })
					)
					.Select(x => new KeyValuePair<Type, Type>(x.Interfaz, x.Type))
					.ToList()
				;
				return result;
			}
		}
		#endregion GetAssignsFromInterface

		#region CreateContainer
		private static IUnityContainer CreateContainer()
		{
			var container = new UnityContainer();

			container

			// Container
			.RegisterInstance<IUnityContainer>(container)
			.RegisterType<IUnitOfWork, InternalUnitOfWork>(new PerResolveLifetimeManager())
			.RegisterType<IInternalContext, InternalContextAdapter>(new PerResolveLifetimeManager())
			.RegisterType<ISessionData, SessionData>()
			.RegisterType<IBlobRepository, AzureBlobRepository>()
			.RegisterType<IPaymentGatewayAdapter, SabadellGatewayAdapter>()
			.RegisterType<ILogService, ApplicationInsightsLogService>()
			;

			foreach (var item in GetAssignsFromInterface(typeof(IEntityRepository<>)))
				container.RegisterType(item.Key, item.Value);
			foreach (var item in GetAssignsFromInterface(typeof(IQueueRepository<>)))
				container.RegisterType(item.Key, item.Value);

			#region Gets
			{
				var types = GetAssignsFromInterface(typeof(IQueryBaseHandler<,>));
				foreach (var item in types)
				{
					var interfaz = item.Key;
					var clase = item.Value;
					var needSave = false;

					container.RegisterType(interfaz, clase, "GetHandlers");
					var handlerName = "GetHandlers";

					if (needSave)
					{
						container.RegisterType(
							interfaz,
							typeof(ContextQueryHandlerDecorator<,>).MakeGenericType(interfaz.GenericTypeArguments),
							"ContextHandler",
							new InjectionConstructor(
								new ResolvedParameter(interfaz, handlerName),
								new ResolvedParameter<IUnitOfWork>()
							));
						handlerName = "ContextHandler";
					}

					{
						var attribute = clase.GetCustomAttribute<XpLogAttribute>();
						if (attribute != null)
						{
							container.RegisterType(
								interfaz,
								typeof(LogQueryHandlerDecorator<,>).MakeGenericType(interfaz.GenericTypeArguments),
								"LogHandler",
								new InjectionConstructor(
									new ResolvedParameter(interfaz, handlerName),
									new ResolvedParameter<ISessionData>(),
									new ResolvedParameter<IUnitOfWork>(),
									new InjectionParameter(attribute.RelatedClass),
									new InjectionParameter(attribute.RelatedMethod),
									new InjectionParameter(attribute.RelatedId)
								));
							handlerName = "LogHandler";

							needSave = true;
						}
					}

					container.RegisterType(
						interfaz,
						typeof(ErrorQueryHandlerDecorator<,>).MakeGenericType(interfaz.GenericTypeArguments),
						new InjectionConstructor(
							new ResolvedParameter(interfaz, handlerName)
						));
				}
				var precessedArguments = types
					.Select(x => x.Key.GetGenericArguments())
					.Select(x => new KeyValuePair<string, string>(x[0].FullName, x[1].FullName));

				// Implicit GetId
				var getIdArguments = GetAssignsFromInterface(typeof(IGetIdArgumentsBase<,>))
					.Where(x => !precessedArguments.Select(y => y.Key).Contains(x.Value.FullName));
				foreach (var item in getIdArguments)
				{
					var classArguments = item.Value;
					var classResult = item.Key.GetGenericArguments()[0];
					var classEntity = item.Key.GetGenericArguments()[1];

					var interfaz = typeof(IQueryBaseHandler<,>).MakeGenericType(classArguments, classResult);
					var clase = typeof(GetIdImplicitHandler<,,>).MakeGenericType(classArguments, classResult, classEntity);

					container.RegisterType(interfaz, clase, "GetHandlers");
					container.RegisterType(
						interfaz,
						typeof(ErrorQueryHandlerDecorator<,>).MakeGenericType(interfaz.GenericTypeArguments),
						new InjectionConstructor(
							new ResolvedParameter(interfaz, "GetHandlers")
						));
				}
			}
			#endregion Gets

			#region Services
			{
				var types = GetAssignsFromInterface(typeof(IServiceBaseHandler<>));
				foreach (var item in types)
				{
					var interfaz = item.Key;
					var clase = item.Value;

					container.RegisterType(interfaz, clase, "CommandHandlers");
					var handlerName = "CommandHandlers";

					container.RegisterType(
						interfaz,
						typeof(ContextServiceHandlerDecorator<>).MakeGenericType(interfaz.GenericTypeArguments),
						"ContextHandler",
						new InjectionConstructor(
							new ResolvedParameter(interfaz, handlerName),
							new ResolvedParameter<IUnitOfWork>()
						));
					handlerName = "ContextHandler";

					{
						var attribute = clase.GetCustomAttribute<XpLogAttribute>();
						if (attribute != null)
						{
							container.RegisterType(
								interfaz,
								typeof(LogServiceHandlerDecorator<>).MakeGenericType(interfaz.GenericTypeArguments),
								"LogHandler",
								new InjectionConstructor(
									new ResolvedParameter(interfaz, handlerName),
									new ResolvedParameter<ISessionData>(),
									new ResolvedParameter<IUnitOfWork>(),
									new InjectionParameter(attribute.RelatedClass),
									new InjectionParameter(attribute.RelatedMethod),
									new InjectionParameter(attribute.RelatedId)
								));
							handlerName = "LogHandler";
						}
					}

					container.RegisterType(
						interfaz,
						typeof(ErrorServiceHandlerDecorator<>).MakeGenericType(interfaz.GenericTypeArguments),
						new InjectionConstructor(
							new ResolvedParameter(interfaz, handlerName)
						));
				}
				var precessedArguments = types
					.SelectMany(x => x.Key.GetGenericArguments()
						.Select(y => y.FullName)
					);

				// Implicit Create
				var createArguments = GetAssignsFromInterface(typeof(ICreateArgumentsBase<>))
					.Where(x => !precessedArguments.Contains(x.Value.FullName));
				foreach (var item in createArguments)
				{
					var interfaz = typeof(IServiceBaseHandler<>).MakeGenericType(item.Value);
					var clase = typeof(CreateImplicitHandler<,>).MakeGenericType(item.Value, item.Key.GetGenericArguments().FirstOrDefault());

					container.RegisterType(interfaz, clase, "CommandHandlers");
					container.RegisterType(
						interfaz,
						typeof(ContextServiceHandlerDecorator<>).MakeGenericType(interfaz.GenericTypeArguments),
						"ServiceHandler",
						new InjectionConstructor(
							new ResolvedParameter(interfaz, "CommandHandlers"),
							new ResolvedParameter<IUnitOfWork>()
						));
					container.RegisterType(
						interfaz,
						typeof(ErrorServiceHandlerDecorator<>).MakeGenericType(interfaz.GenericTypeArguments),
						new InjectionConstructor(
							new ResolvedParameter(interfaz, "ServiceHandler")
						));
				}

				// Implicit Update
				var updateArguments = GetAssignsFromInterface(typeof(IUpdateArgumentsBase<>))
					.Where(x => !precessedArguments.Contains(x.Value.FullName));
				foreach (var item in updateArguments)
				{
					var interfaz = typeof(IServiceBaseHandler<>).MakeGenericType(item.Value);
					var clase = typeof(UpdateImplicitHandler<,>).MakeGenericType(item.Value, item.Key.GetGenericArguments().FirstOrDefault());

					container.RegisterType(interfaz, clase, "CommandHandlers");
					container.RegisterType(
						interfaz,
						typeof(ContextServiceHandlerDecorator<>).MakeGenericType(interfaz.GenericTypeArguments),
						"ServiceHandler",
						new InjectionConstructor(
							new ResolvedParameter(interfaz, "CommandHandlers"),
							new ResolvedParameter<IUnitOfWork>()
						));
					container.RegisterType(
						interfaz,
						typeof(ErrorServiceHandlerDecorator<>).MakeGenericType(interfaz.GenericTypeArguments),
						new InjectionConstructor(
							new ResolvedParameter(interfaz, "ServiceHandler")
						));
				}

				// Implicit Delete
				var deleteArguments = GetAssignsFromInterface(typeof(IDeleteArgumentsBase<>))
					.Where(x => !precessedArguments.Contains(x.Value.FullName));
				foreach (var item in deleteArguments)
				{
					var interfaz = typeof(IServiceBaseHandler<>).MakeGenericType(item.Value);
					var clase = typeof(DeleteImplicitHandler<,>).MakeGenericType(item.Value, item.Key.GetGenericArguments().FirstOrDefault());

					container.RegisterType(interfaz, clase, "CommandHandlers");
					container.RegisterType(
						interfaz,
						typeof(ContextServiceHandlerDecorator<>).MakeGenericType(interfaz.GenericTypeArguments),
						"ServiceHandler",
						new InjectionConstructor(
							new ResolvedParameter(interfaz, "CommandHandlers"),
							new ResolvedParameter<IUnitOfWork>()
						));
					container.RegisterType(
						interfaz,
						typeof(ErrorServiceHandlerDecorator<>).MakeGenericType(interfaz.GenericTypeArguments),
						new InjectionConstructor(
							new ResolvedParameter(interfaz, "ServiceHandler")
						));
				}
			}
			#endregion Services

			return container;
		}
		#endregion CreateContainer

		#region Resolve
		public static T Resolve<T>()
		{
			try
			{
				var result = Container.Resolve<T>();
				return result;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(string.Format("DI Server: {0}", ex.Message));
				throw;
			}
		}
		#endregion Resolve

		#region InitializeWebApi
		public static void InitializeWebApi(HttpConfiguration config)
		{
			config.DependencyResolver = new UnityDependencyResolver(Container);
		}
		#endregion InitializeWebApi

		#region Initialize
		public static void Initialize()
		{
			_Container = CreateContainer();
		}
		#endregion Initialize
	}
}
