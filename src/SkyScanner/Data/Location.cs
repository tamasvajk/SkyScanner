// Copyright (c) 2015-2016 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

namespace SkyScanner.Data
{
    /// <summary>
    /// Location supported by Skyscanner
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Create a location object with a given ID. If the ID of a location is known, it doesn't have to be queried
        /// from the server.
        /// </summary>
        /// <param name="id">ID of the location</param>
        /// <returns></returns>
        public static Location FromString(string id)
        {
            return new Location { PlaceId = id };
        }
        /// <summary>
        /// ID of the location
        /// </summary>
        public string PlaceId { get; internal set; }
        /// <summary>
        /// Name of the location (locale dependent)
        /// </summary>
        public string PlaceName { get; internal set; }
        /// <summary>
        /// Country ID where the location is
        /// </summary>
        public string CountryId { get; internal set; }
        /// <summary>
        /// Region ID where the location is - empty?
        /// </summary>
        public string RegionId { get; internal set; }
        /// <summary>
        /// City ID where the location is
        /// </summary>
        public string CityId { get; internal set; }
        /// <summary>
        /// Country name where the location is (locale dependent)
        /// </summary>
        public string CountryName { get; internal set; }

        public override string ToString()
        {
            return PlaceId;
        }
    }
}