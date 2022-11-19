using Devpack.Cache.Services.MemoryCache;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Devpack.Cache.Tests
{
    public class CacheServiceTests
    {
        private readonly Mock<IMemoryCacheService> _memoryCacheServiceMock;

        public CacheServiceTests()
        {
            _memoryCacheServiceMock = new Mock<IMemoryCacheService>();
        }

        [Fact (DisplayName = "Deve gerar obter o dado do cache quando o mesmo existir em memória - Sync Method.")]
        [Trait("Category", "Services")]
        public void GetFromMemory_WhenCache()
        {
            // Arrange
            var cacheKey = Guid.NewGuid().ToString();
            var expectedCacheValue = Guid.NewGuid();

            _memoryCacheServiceMock.Setup(m => m.Get(It.IsAny<string>(), out expectedCacheValue)).Returns(true);

            var service = new CacheService(_memoryCacheServiceMock.Object);

            // Act
            var result = service.GetFromMemory(cacheKey, () => Guid.NewGuid());

            // Asserts
            result.Should().Be(expectedCacheValue);
            _memoryCacheServiceMock.Verify(m => m.Save(cacheKey, expectedCacheValue), Times.Never);
        }

        [Fact(DisplayName = "Deve gerar obter o dado do delegate quando o mesmo não existir em memória. - Sync Method.")]
        [Trait("Category", "Services")]
        public void GetFromMemory_WhenNoCache()
        {
            // Arrange
            var cacheKey = Guid.NewGuid().ToString();
            var expectedResult = Guid.NewGuid();

            var service = new CacheService(_memoryCacheServiceMock.Object);

            // Act
            var result = service.GetFromMemory(cacheKey, () => expectedResult);

            // Asserts
            result.Should().Be(expectedResult);
            _memoryCacheServiceMock.Verify(m => m.Save(cacheKey, expectedResult), Times.Once);
        }

        [Fact(DisplayName = "Deve gerar obter o dado do cache quando o mesmo existir em memória - Async Method.")]
        [Trait("Category", "Services")]
        public async Task GetFromMemoryAsync_WhenCache()
        {
            // Arrange
            var cacheKey = Guid.NewGuid().ToString();
            var expectedCacheValue = Guid.NewGuid();

            _memoryCacheServiceMock.Setup(m => m.Get(It.IsAny<string>(), out expectedCacheValue)).Returns(true);

            var service = new CacheService(_memoryCacheServiceMock.Object);

            // Act
            var result = await service.GetFromMemoryAsync(cacheKey, () => Task.FromResult(Guid.NewGuid()));

            // Asserts
            result.Should().Be(expectedCacheValue);
            _memoryCacheServiceMock.Verify(m => m.Save(cacheKey, expectedCacheValue), Times.Never);
        }

        [Fact(DisplayName = "Deve gerar obter o dado do delegate quando o mesmo não existir em memória. - Async Method.")]
        [Trait("Category", "Services")]
        public async Task GetFromMemoryAsync_WhenNoCache()
        {
            // Arrange
            var cacheKey = Guid.NewGuid().ToString();
            var expectedResult = Guid.NewGuid();

            var service = new CacheService(_memoryCacheServiceMock.Object);

            // Act
            var result = await service.GetFromMemoryAsync(cacheKey, () => Task.FromResult(expectedResult));

            // Asserts
            result.Should().Be(expectedResult);
            _memoryCacheServiceMock.Verify(m => m.Save(cacheKey, expectedResult), Times.Once);
        }
    }
}