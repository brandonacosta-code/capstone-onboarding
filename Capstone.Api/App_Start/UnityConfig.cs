using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Capstone.Api.Unity;
using Capstone.Application.MediatR;
using Capstone.Core.Interfaces;
using Capstone.Infrastructure.Repositories;
using MediatR;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace Capstone.Api.App_Start
{
    public static class UnityConfig
    {
		public static UnityContainer RegisterComponents() 
		{
			var container = new UnityContainer();

			string connString = ConfigurationManager
				.ConnectionStrings["CapstoneDB"]
				.ConnectionString;

			//MediatR
			container.RegisterMediator(new HierarchicalLifetimeManager());
			container.RegisterMediatorHandlers(Assembly.GetAssembly(typeof(Ping)));
			container.RegisterType<IMediator, Mediator>(new ContainerControlledLifetimeManager());
			container.RegisterType<IMessageBus, MessageBus>(new ContainerControlledLifetimeManager());


			//Repositories
			container.RegisterType<IProductRepository, ProductRepository>(new ContainerControlledLifetimeManager(), new InjectionConstructor(connString));
			container.RegisterType<IOrderRepository, OrderRepository>(new ContainerControlledLifetimeManager(), new InjectionConstructor(connString));

			return container;
		}
	}
}
