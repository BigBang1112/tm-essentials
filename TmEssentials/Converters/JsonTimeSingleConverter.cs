#if NET5_0_OR_GREATER
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TmEssentials.Converters;

/// <summary>
/// Converts a <see cref="TimeSingle"/> to and from JSON using System.Text.Json.
/// </summary>
public sealed class JsonTimeSingleConverter : JsonConverter<TimeSingle>
{
    /// <summary>
    /// Reads and converts the JSON to type <see cref="TimeSingle"/>.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override TimeSingle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Debug.Assert(typeToConvert == typeof(TimeSingle));
        return new TimeSingle(reader.GetSingle());
    }

    /// <summary>
    /// Writes a <see cref="TimeSingle"/> as JSON.
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, TimeSingle value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.TotalSeconds);
    }
}
#endif