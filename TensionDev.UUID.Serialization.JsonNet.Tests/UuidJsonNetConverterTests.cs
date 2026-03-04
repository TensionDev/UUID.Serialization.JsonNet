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

        [Fact]
        public void TestReadJson()
        {
            // Arrange
            const string validUuidString = "00000000-0000-0000-0000-000000000000";
            var readerMock = new Mock<JsonReader>(MockBehavior.Strict);
            readerMock.SetupGet(r => r.TokenType).Returns(JsonToken.String);
            readerMock.SetupGet(r => r.Value).Returns((object)validUuidString);

            var converter = new UuidJsonNetConverter();

            // existingValue must be non-nullable; obtain an instance via Parse to satisfy parameter constraints.
            Uuid existingValue = Uuid.Parse(validUuidString);
            var serializer = new JsonSerializer();

            // Act
            Uuid result = converter.ReadJson(readerMock.Object, typeof(Uuid), existingValue, hasExistingValue: false, serializer: serializer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(validUuidString, result.ToString());
        }

        [Fact]
        public void TestWriteJson()
        {
            var writerMock = new Mock<JsonWriter>(MockBehavior.Strict);
            object? capturedValue = null;
            // Setup to accept any object and capture it
            writerMock.Setup(w => w.WriteValue(It.IsAny<string?>()))
                      .Callback<string?>(v => capturedValue = v)
                      .Verifiable();

            var serializerMock = new Mock<JsonSerializer>(MockBehavior.Loose);

            // Attempt to create an instance of the Uuid type.
            // This will succeed for structs and for classes with a public parameterless constructor.
            // If it fails, mark test inconclusive because we cannot safely fabricate the dependency.
            TensionDev.UUID.Uuid value;
            try
            {
                var instance = Activator.CreateInstance(typeof(TensionDev.UUID.Uuid));
                if (instance == null)
                {
                    // This can happen if the type is a non-instantiable class (abstract) or Activator returns null.
                    Assert.Fail("Could not create an instance of TensionDev.UUID.Uuid (Activator.CreateInstance returned null). Provide a parameterless constructor or update the test environment.");
                    return;
                }
                
                value = (TensionDev.UUID.Uuid)instance;
            }
            catch (Exception ex)
            {
                // xUnit does not provide Assert.Inconclusive; use Assert.Fail to report the diagnostic and stop the test.
                Assert.Fail("Could not create an instance of TensionDev.UUID.Uuid: " + ex.Message);
                return;
            }

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