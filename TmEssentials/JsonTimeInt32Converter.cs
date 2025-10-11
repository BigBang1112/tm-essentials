#if NET5_0_OR_GREATER
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TmEssentials;

/// <summary>
/// Converts a <see cref="TimeInt32"/> to and from JSON using System.Text.Json.
/// </summary>
public sealed class JsonTimeInt32Converter : JsonConverter<TimeInt32>
{
    /// <summary>
    /// Reads and converts the JSON to type <see cref="TimeInt32"/>.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override TimeInt32 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(TimeInt32));
        return new TimeInt32(reader.GetInt32());
    }

    /// <summary>
    /// Writes a <see cref="TimeInt32"/> as JSON.
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, TimeInt32 value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.TotalMilliseconds);
    }
}
#endif