// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

namespace SkyScanner.Data
{
    public enum LocationSchema
    {
        /// <summary>
        /// For airports and cities the internationally recognized IATA schema can be used. For countries the ISO schema can be used. This combined IATA/ISO schema is the default schema for use with Skyscanner services.
        /// </summary>
        Iata, 
        /// <summary>
        /// GeoNameCodes from the GeoNames schema (www.geonames.org)
        /// </summary>
        GeoNameCode, 
        /// <summary>
        /// GeoNameIDs from the GeoNames schema (www.geonames.org)
        /// </summary>
        GeoNameId, 
        /// <summary>
        /// A unique ID for a location in Skyscanner. For example EDIN is the Skyscanner code for Edinburgh City and its Rnid is 2343.
        /// </summary>
        Rnid, 
        /// <summary>
        /// Skyscanner code. The response from Location Autosuggest provides these ids.
        /// </summary>
        Sky,
        /// <summary>
        /// Latitude and longitude of the place in the following form: "latitude,longitude". The nearest city with airport will be used.
        /// </summary>
        LatLong,
        /// <summary>
        /// IP of a user. The nearest city with airport will be used.
        /// </summary>
        Ip,

        Default
    }
}