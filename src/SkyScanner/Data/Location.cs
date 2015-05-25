// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

namespace SkyScanner.Data
{
    public class Location
    {
        public static Location FromString(string id)
        {
            return new Location { PlaceId = id };
        }
        public string PlaceId { get; internal set; }
        public string PlaceName { get; internal set; }
        public string CountryId { get; internal set; }
        public string RegionId { get; internal set; }
        public string CityId { get; internal set; }
        public string CountryName { get; internal set; }

        public override string ToString()
        {
            return PlaceId;
        }
    }
}