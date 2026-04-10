using Moq;
using Newtonsoft.Json;
using System;
using Xunit;

namespace TensionDev.UUID.Serialization.JsonNet.Tests
{
    public class UuidJsonNetConverterTests : IDisposable
    {
        private bool disposedValue;

        private readonly UuidJsonNetConverter _converter;

        public UuidJsonNetConverterTests()
        {
            _converter = new UuidJsonNetConverter();
        }

        [Theory]
        [ClassData(typeof(ReadJsonTestData))]
        public void TestReadJson(JsonToken jsonToken, object value, bool throwsException)
        {
            // Arrange
            var readerMock = new Mock<JsonReader>(MockBehavior.Strict);
            readerMock.SetupGet(r => r.TokenType).Returns(jsonToken);
            readerMock.SetupGet(r => r.Value).Returns(value);

            // existingValue must be non-nullable; obtain an instance via Parse to satisfy parameter constraints.
            Uuid existingValue = Uuid.Max;
            var serializer = new JsonSerializer();

            if (throwsException)
            {
                // Act and Assert
                Assert.Throws<JsonSerializationException>(() => _converter.ReadJson(readerMock.Object, typeof(Uuid), existingValue, hasExistingValue: false, serializer: serializer));
            }
            else
            {
                // Act
                Uuid result = _converter.ReadJson(readerMock.Object, typeof(Uuid), existingValue, hasExistingValue: false, serializer: serializer);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(value.ToString(), result.ToString());
            }
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
        [InlineData("164a714c-0c79-11ec-82a8-0242ac130003")]
        [InlineData("550e8400-e29b-41d4-a716-446655440000")]
        [InlineData("1bf6935b-49e6-54cf-a9c8-51fb21c41b46")]
        public void TestWriteJson(string validUuidString)
        {
            var writerMock = new Mock<JsonWriter>(MockBehavior.Strict);
            object? capturedValue = null;
            // Setup to accept any object and capture it
            writerMock.Setup(w => w.WriteValue(It.IsAny<string?>()))
                      .Callback<string?>(v => capturedValue = v)
                      .Verifiable();

            var serializerMock = new Mock<JsonSerializer>(MockBehavior.Loose);

            TensionDev.UUID.Uuid value = TensionDev.UUID.Uuid.Parse(validUuidString);

            // Act
            _converter.WriteJson(writerMock.Object, value, serializerMock.Object);

            // Assert
            writerMock.Verify(w => w.WriteValue(It.IsAny<string?>()), Times.Once, "WriteValue should be called exactly once.");
            // Ensure the captured value matches the value.ToString() result
            var expected = value.ToString();
            Assert.NotNull(capturedValue);
            Assert.Equal(expected, capturedValue?.ToString());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TestUuidJsonNetConverter()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}