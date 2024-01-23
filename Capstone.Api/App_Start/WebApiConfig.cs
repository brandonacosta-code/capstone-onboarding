using System.Net.Http.Formatting;
using System;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using Capstone.Api.Unity;
using Capstone.Application.MediatR;
using MediatR;
using Unity;
using Unity.Lifetime;
using Newtonsoft.Json;
using Capstone.Infrastructure.Repositories;

namespace Capstone.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();

            var container = new UnityContainer();

            container.RegisterMediator(new HierarchicalLifetimeManager());
            container.RegisterMediatorHandlers(Assembly.GetAssembly(typeof(Ping)));
            container.RegisterType<IMediator, Mediator>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageBus, MessageBus>(new ContainerControlledLifetimeManager());
            container.RegisterType<IProductRepository, ProductRepository>(new ContainerControlledLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);


            config.Formatters.Add(new BrowserJsonFormatter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

    public class BrowserJsonFormatter : JsonMediaTypeFormatter
    {
        public BrowserJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            this.SerializerSettings.Formatting = Formatting.Indented;
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
}
