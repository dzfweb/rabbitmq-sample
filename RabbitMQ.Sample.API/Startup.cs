using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac;
using MassTransit;
using Autofac.Extensions.DependencyInjection;
using MassTransit.Util;

namespace RabbitMQ.Sample.API
{
    public class Startup
    {
        private IContainer container;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            var builder = new ContainerBuilder();
            builder.Register(c =>
            {
                return Bus.Factory.CreateUsingRabbitMq(sbc =>
                    sbc.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    })
                );
            })
            .As<IBusControl>()
            .As<IBus>()
            .As<IPublishEndpoint>()
            .SingleInstance();
            builder.Populate(services);
            container = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime lifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            var bus = container.Resolve<IBusControl>();
            var busHandle = TaskUtil.Await(() => bus.StartAsync());

            lifetime.ApplicationStopping.Register(() => busHandle.Stop());

        }
    }
}
