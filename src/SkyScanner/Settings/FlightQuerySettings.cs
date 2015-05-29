// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace SkyScanner.Settings
{
    /// <summary>
    /// Settings for the flight query
    /// </summary>
    public class FlightQuerySettings
    {
        private readonly FlightRequestSettings _requestSettings;
        private readonly FlightResponseSettings _responseSettings;

        /// <summary>
        /// Initializes a new instance of the FlightQuerySettings with the specified parameters
        /// </summary>
        /// <param name="requestSettings"></param>
        /// <param name="responseSettings"></param>
        public FlightQuerySettings(FlightRequestSettings requestSettings, FlightResponseSettings responseSettings)
        {
            if (requestSettings == null)
            {
                throw new ArgumentNullException("requestSettings");
            }
            if (responseSettings == null)
            {
                throw new ArgumentNullException("responseSettings");
            }

            if (!requestSettings.InboundDate.HasValue &&
                (responseSettings.InboundDepartureEndTime.HasValue ||
                 responseSettings.InboundDepartureStartTime.HasValue ||
                 responseSettings.InboundDepartureTime.HasValue))
            {
                throw new ArgumentException("A one-way flight is queried, but response settings assume a return flight");
            }

            _requestSettings = requestSettings;
            _responseSettings = responseSettings;
        }
        
        public FlightRequestSettings FlightRequest
        {
            get { return _requestSettings; }
        }

        public FlightResponseSettings FlightResponse
        {
            get { return _responseSettings; }
        }
    }
}