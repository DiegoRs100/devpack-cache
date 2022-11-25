using Devpack.Cache.Services.MemoryCache;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using Xunit;

namespace Devpack.Cache.Tests
{
    public class MemoryCacheServiceTests
    {
        private readonly Mock<IMemoryCache> _memoryCacheMock;

        public MemoryCacheServiceTests()
        {
            _memoryCacheMock = new Mock<IMemoryCache>();
        }

        [Fact(DisplayName = "Deve retornar um valor do cache quando o mesmo existir com a chave informada.")]
        [Trait("Category", "Services")]
        public void Get_WhenExists()
        {
            // Arrange
            var cacheKey = Guid.NewGuid().ToString();
            object? expectedCacheValue = Guid.NewGuid();

            _memoryCacheMock.Setup(m => m.TryGetValue(It.IsAny<string>(), out expectedCacheValue)).Returns(true);

            var service = new MemoryCacheService(_memoryCacheMock.Object);

            // Act
            var result = service.Get(cacheKey, out expectedCacheValue);

            // Asserts
            result.Should().BeTrue();
            _memoryCacheMock.Verify(m => m.TryGetValue(cacheKey, out expectedCacheValue), Times.Once);
        }

        [Fact(DisplayName = "Não deve retornar um valor do cache quando o mesmo não existir com a chave informada.")]
        [Trait("Category", "Services")]
        public void Get_WhenNotExists()
        {
            // Arrange
            var cacheKey = Guid.NewGuid().ToString();
            var service = new MemoryCacheService(_memoryCacheMock.Object);

            // Act
            var result = service.Get(cacheKey, out int expectedCacheValue);

            // Asserts
            result.Should().BeFalse();
            expectedCacheValue.Should().Be(0);
        }

        [Fact(DisplayName = "Deve salvar o dado em memória quando uma chave for passada.")]
        [Trait("Category", "Services")]
        public void Save_UsingDefaultLifetime()
        {
            var cacheKey = Guid.NewGuid().ToString();
            var cachedData = Guid.NewGuid();

            var service = new MemoryCacheService(_memoryCacheMock.Object);

            service.Invoking(s => s.Save(cacheKey, cachedData)).Should()
                .Throw<NullReferenceException>();

            _memoryCacheMock.Verify(m => m.CreateEntry(cacheKey), Times.Once);
        }

        [Fact(DisplayName = "Deve salvar o dado em memória quando uma chave e um tempo de expiração do cache forem passados.")]
        [Trait("Category", "Services")]
        public void Save_UsingCustomLifetime()
        {
            var cacheKey = Guid.NewGuid().ToString();
            var cachedData = Guid.NewGuid();
            var cacheLifetime = TimeSpan.FromMinutes(5);

            var service = new MemoryCacheService(_memoryCacheMock.Object);

            service.Invoking(s => s.Save(cacheKey, cachedData, cacheLifetime)).Should()
                .Throw<NullReferenceException>();

            _memoryCacheMock.Verify(m => m.CreateEntry(cacheKey), Times.Once);
        }
    }
}