// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SkyScanner.Services.Helpers
{
    internal class SkyScannerContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            var property = member as PropertyInfo;
            if (!prop.Readable && property != null)
            {
                var hasPrivateGetter = property.GetGetMethod(true) != null;
                prop.Readable = hasPrivateGetter;
            }
            if (!prop.Writable && property != null)
            {
                var hasPrivateSetter = property.GetSetMethod(true) != null;
                prop.Writable = hasPrivateSetter;
            }
            return prop;
        }
        
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var l = objectType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(prop => !prop.GetCustomAttributes<JsonIgnoreAttribute>().Any())
                .Cast<MemberInfo>()
                .ToList();
            return l;
        }
    }
}