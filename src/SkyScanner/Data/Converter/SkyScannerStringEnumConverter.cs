// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SkyScanner.Data.Converter
{
    internal class SkyScannerStringEnumConverter : StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is Enum))
            {
                base.WriteJson(writer, value, serializer);
                return;
            }

            var enumType = value.GetType();
            var enumField = enumType.GetField(value.ToString());

            var attribute = enumField.GetCustomAttributes<SkyScannerEnumValueAttribute>().FirstOrDefault();
            if (attribute == null)
            {
                base.WriteJson(writer, value, serializer);
                return;
            }

            if (attribute.Value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteValue(attribute.Value);
        }
    }
}