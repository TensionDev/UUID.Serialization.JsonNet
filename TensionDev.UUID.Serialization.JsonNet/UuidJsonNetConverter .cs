using Newtonsoft.Json;
using System;

namespace TensionDev.UUID.Serialization.JsonNet
{
    /// <summary>
    /// <see cref="JsonConverter"/> for the <see cref="Uuid"/> type that handles serialization and deserialization
    /// </summary>
    public class UuidJsonNetConverter : JsonConverter<Uuid>
    {
        /// <summary>
        /// Reads a JSON value and converts it to a new instance of the Uuid type.
        /// </summary>
        /// <param name="reader">The JsonReader used to read the JSON value to be converted.</param>
        /// <param name="objectType">The type of the object to deserialize. This parameter is not used.</param>
        /// <param name="existingValue">The existing <see cref="Uuid"/> value to be updated, if applicable. This parameter is not used.</param>
        /// <param name="hasExistingValue">A value indicating whether an existing value is available to be updated.</param>
        /// <param name="serializer">The <see cref="JsonSerializer"/> used for deserialization.</param>
        /// <returns>A Uuid instance parsed from the JSON value read by the reader.</returns>
        /// <exception cref="JsonSerializationException">Thrown when the JSON token is not a string or if parsing fails.</exception>"
        public override Uuid ReadJson(JsonReader reader, Type objectType, Uuid existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String && reader.Value is string s)
            {
                return Uuid.Parse(s);
            }
            throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing UUID.");
        }

        /// <summary>
        /// Writes the specified <see cref="Uuid"/> to JSON by writing its string representation
        /// to the provided <paramref name="writer"/>.
        /// The converter serializes the Uuid using its canonical textual form returned by
        /// <see cref="Uuid.ToString()"/>.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> used to write the JSON value.</param>
        /// <param name="value">The <see cref="Uuid"/> instance to serialize.</param>
        /// <param name="serializer">The <see cref="JsonSerializer"/> performing the serialization.</param>
        public override void WriteJson(JsonWriter writer, Uuid value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
