// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace SkyScanner.Settings
{
    /// <summary>
    /// Settings for the flight query
    /// </summary>
    public class FlightQuerySettings
    {
        /// <summary>
        /// Initializes a new instance of the FlightQuerySettings with the specified parameters
        /// </summary>
        /// <param name="requestSettings"></param>
        /// <param name="responseSettings"></param>
        public FlightQuerySettings(FlightRequestSettings requestSettings, FlightResponseSettings responseSettings)
        {
            if (requestSettings == null)
            {
                throw new ArgumentNullException(nameof(requestSettings));
            }
            if (responseSettings == null)
            {
                throw new ArgumentNullException(nameof(responseSettings));
            }

            if (!requestSettings.InboundDate.HasValue &&
                (responseSettings.InboundDepartureEndTime.HasValue ||
                 responseSettings.InboundDepartureStartTime.HasValue ||
                 responseSettings.InboundDepartureTime.HasValue))
            {
                throw new ArgumentException("A one-way flight is queried, but response settings assume a return flight");
            }

            FlightRequest = requestSettings;
            FlightResponse = responseSettings;
        }
        
        public FlightRequestSettings FlightRequest { get; }

        public FlightResponseSettings FlightResponse { get; }
    }
}