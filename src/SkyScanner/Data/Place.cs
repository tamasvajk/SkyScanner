// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using Newtonsoft.Json;
using SkyScanner.Data.Base;

namespace SkyScanner.Data
{
    public class Place
    {
        internal int Id { get; set; }
        internal int ParentId { get; set; }
        public string Code { get; internal set; }
        public string Name { get; internal set; }
        public PlaceType Type { get; internal set; }
        [JsonIgnore]
        internal IContainerResponse ContainerResponse { get; set; }
        [JsonIgnore]
        public Place ParentPlace
        {
            get { return ContainerResponse.Places.FirstOrDefault(place => place.Id == ParentId); }
        }
    }
}