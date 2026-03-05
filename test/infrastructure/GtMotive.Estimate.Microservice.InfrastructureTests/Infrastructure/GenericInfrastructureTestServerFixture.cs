using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure
{
    public sealed class GenericInfrastructureTestServerFixture : IDisposable
    {
        public GenericInfrastructureTestServerFixture()
        {
            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseEnvironment("IntegrationTest")
                .UseDefaultServiceProvider(options => { options.ValidateScopes = true; })
                .ConfigureAppConfiguration((context, builder) =>
                {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
                    builder.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["MongoDb:ConnectionString"] = "mongodb://localhost:27017",
                        ["MongoDb:MongoDbDatabaseName"] = "GtMotiveEstimate_Test"
                    });
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.

                    builder.AddEnvironmentVariables();
                })
                .UseStartup<Startup>();

            Server = new TestServer(hostBuilder);
        }

        public TestServer Server { get; }

        /// <inheritdoc />
        public void Dispose()
        {
            Server?.Dispose();
        }
    }
}
