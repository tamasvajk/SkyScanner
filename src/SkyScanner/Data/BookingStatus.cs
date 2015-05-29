// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

namespace SkyScanner.Data
{
    /// <summary>
    /// The status of the booking price
    /// </summary>
    public enum BookingStatus
    {
        /// <summary>
        /// Live price
        /// </summary>
        Current,
        /// <summary>
        /// Awaiting live price
        /// </summary>
        Pending,
        /// <summary>
        /// A non-live price
        /// </summary>
        Estimated,
        /// <summary>
        /// The flight is now fully booked
        /// </summary>
        NotAvailable,
        /// <summary>
        /// Obtaining a live or estimated price failed
        /// </summary>
        Failed
    }
}