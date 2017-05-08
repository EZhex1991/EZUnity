using System;
using Newtonsoft.Json;
using UnityEngine;

public class Vector2Converter : JsonConverter
{
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		var vec = (Vector2)value;
		writer.WriteStartObject();
		writer.WritePropertyName("x");
		writer.WriteValue(vec.x);
		writer.WritePropertyName("y");
		writer.WriteValue(vec.y);
		writer.WriteEndObject();
	}

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Vector2);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
	}

	public override bool CanRead
	{
		get { return false; }
	}
}