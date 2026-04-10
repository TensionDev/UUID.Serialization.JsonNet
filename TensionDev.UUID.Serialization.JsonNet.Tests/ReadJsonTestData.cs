using Newtonsoft.Json;
using System;
using Xunit;

namespace TensionDev.UUID.Serialization.JsonNet.Tests
{
    public class ReadJsonTestData : TheoryData<JsonToken, Object, Boolean>
    {
        public ReadJsonTestData()
        {
            Add(JsonToken.String, "00000000-0000-0000-0000-000000000000", false);
            Add(JsonToken.String, "ffffffff-ffff-ffff-ffff-ffffffffffff", false);
            Add(JsonToken.String, "164a714c-0c79-11ec-82a8-0242ac130003", false);
            Add(JsonToken.String, "550e8400-e29b-41d4-a716-446655440000", false);
            Add(JsonToken.String, "1bf6935b-49e6-54cf-a9c8-51fb21c41b46", false);

            Add(JsonToken.Boolean, "550e8400-e29b-41d4-a716-446655440000", true);
            Add(JsonToken.Bytes, "550e8400-e29b-41d4-a716-446655440000", true);
            Add(JsonToken.Date, "550e8400-e29b-41d4-a716-446655440000", true);
            Add(JsonToken.Float, "550e8400-e29b-41d4-a716-446655440000", true);
            Add(JsonToken.Integer, "550e8400-e29b-41d4-a716-446655440000", true);
            Add(JsonToken.Null, "550e8400-e29b-41d4-a716-446655440000", true);
            Add(JsonToken.Raw, "550e8400-e29b-41d4-a716-446655440000", true);
            Add(JsonToken.Undefined, "550e8400-e29b-41d4-a716-446655440000", true);

            Add(JsonToken.String, Int16.MaxValue, true);
            Add(JsonToken.String, Int32.MaxValue, true);
            Add(JsonToken.String, Int64.MaxValue, true);
            Add(JsonToken.String, Single.MaxValue, true);
            Add(JsonToken.String, Double.MaxValue, true);
            Add(JsonToken.String, DateTime.MinValue, true);
            Add(JsonToken.String, DateTime.MaxValue, true);
            Add(JsonToken.String, Guid.Empty, true);

            Add(JsonToken.Raw, new Object(), true);
        }
    }
}
