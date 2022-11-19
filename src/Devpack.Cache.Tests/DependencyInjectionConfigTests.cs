using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Devpack.Cache.Tests
{
    public class DependencyInjectionConfigTests
    {
        [Fact(DisplayName = "Deve injectar corretamente os serviços quando o método for chamado.")]
        [Trait("Category", "Services")]
        public void AddCacheServices()
        {
            var serviceColection = new ServiceCollection();
            serviceColection.AddCacheServices();

            var provider = serviceColection.BuildServiceProvider();
            var cacheService = provider.GetService<ICache>();

            cacheService.Should().NotBeNull();
        }
    }
}