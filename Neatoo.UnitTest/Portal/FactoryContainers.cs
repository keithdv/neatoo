using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neatoo.Portal;
using Neatoo.Portal.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Neatoo.UnitTest.Portal.AuthorizationAllCombinationTests;

namespace Neatoo.UnitTest.Portal
{
    public class ServerServiceProvider
    {
        public IServiceProvider serverProvider { get; set; }
    }


    internal class DoRemoteRequestTest : IDoRemoteRequest
    {
        private readonly INeatooJsonSerializer NeatooJsonSerializer;
        private readonly IServiceProvider serviceProvider;

        public DoRemoteRequestTest(INeatooJsonSerializer neatooJsonSerializer, IServiceProvider serviceProvider)
        {
            NeatooJsonSerializer = neatooJsonSerializer;
            this.serviceProvider = serviceProvider;
        }

        public async Task<T?> ForDelegate<T>(Type delegateType, object[]? parameters)
        {
            // Mimic all the steps of a Remote call except the actual http call

            var remoteRequest = NeatooJsonSerializer.ToRemoteRequest(delegateType, parameters);

            // Mimic real life - use standard ASP.NET Core JSON serialization
            var json = JsonSerializer.Serialize(remoteRequest); //NeatooJsonSerializer.Serialize(remoteRequest);
            var remoteRequestOnServer = JsonSerializer.Deserialize<RemoteRequestDto>(json); NeatooJsonSerializer.Deserialize<RemoteRequestDto>(json);

            // Use the Server's container
            var remoteResponseOnServer = await serviceProvider.GetRequiredService<ServerServiceProvider>()
                                                                .serverProvider
                                                                .GetRequiredService<ServerHandlePortalRequest>()(remoteRequestOnServer);

            json = JsonSerializer.Serialize(remoteResponseOnServer); // NeatooJsonSerializer.Serialize(remoteResponseOnServer);
            var result = JsonSerializer.Deserialize<RemoteResponseDto>(json); // NeatooJsonSerializer.Deserialize<RemoteResponseDto>(json);

            return NeatooJsonSerializer.DeserializeRemoteResponse<T>(result);
        }
    }

    internal static class FactoryContainers
    {
        private static object lockContainer = new object();
        static IServiceProvider serverContainer = null!;
        static IServiceProvider clientContainer = null!;

        public static (IServiceScope server, IServiceScope client) Scopes()
        {
            lock (lockContainer)
            {
                if (serverContainer == null)
                {
                    var serverCollection = new ServiceCollection();
                    var clientCollection = new ServiceCollection();

                    serverCollection.AddNeatooServices(NeatooHost.Local, Assembly.GetExecutingAssembly());
                    serverCollection.AddTransient<Objects.IDisposableDependency, Objects.DisposableDependency>();
                    serverCollection.AddScoped<Objects.DisposableDependencyList>();
                    //serverCollection.AddScoped<AuthorizationClassTests.IAuthorizationClass, AuthorizationClassTests.AuthorizationClass>();
                    //serverCollection.AddScoped<AuthorizationConcreteClassTests.AuthorizationConcreteClass>();
                    serverCollection.AddScoped<AuthorizationAllCombinations>();
                    serverCollection.AddScoped<ReadAuthTests.ReadAuth>();
                    serverCollection.AddScoped<ReadAuthTests.ReadAuthTask>();
                    serverCollection.AddScoped<ReadRemoteAuthTests.ReadRemoteAuth>();
                    serverCollection.AddScoped<ReadRemoteAuthTests.ReadRemoteAuthTask>();

                    clientCollection.AddNeatooServices(NeatooHost.Remote, Assembly.GetExecutingAssembly());
                    clientCollection.AddScoped<ServerServiceProvider>();
                    clientCollection.AddScoped<Objects.DisposableDependencyList>();
                    //clientCollection.AddScoped<AuthorizationClassTests.IAuthorizationClass, AuthorizationClassTests.AuthorizationClass>();
                    clientCollection.AddScoped<AuthorizationAllCombinations>();
                    clientCollection.AddScoped<ReadAuthTests.ReadAuth>();
                    clientCollection.AddScoped<ReadAuthTests.ReadAuthTask>();
                    clientCollection.AddScoped<ReadRemoteAuthTests.ReadRemoteAuth>();
                    clientCollection.AddScoped<ReadRemoteAuthTests.ReadRemoteAuthTask>();

                    clientCollection.AddScoped<IDoRemoteRequest, DoRemoteRequestTest>();
                    serverContainer = serverCollection.BuildServiceProvider();
                    clientContainer = clientCollection.BuildServiceProvider();
                }
            }

            var serverScope = serverContainer.CreateScope();
            var clientScope = clientContainer.CreateScope();

            clientScope.GetRequiredService<ServerServiceProvider>().serverProvider = serverScope.ServiceProvider;

            return (serverScope, clientScope);
        }
    }
}
