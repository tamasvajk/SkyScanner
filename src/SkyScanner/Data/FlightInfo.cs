// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using SkyScanner.Data.Base;

namespace SkyScanner.Data
{
    public class FlightInfo
    {
        public int FlightNumber { get; internal set; }
        internal int CarrierId { get; set; }
        internal IContainerResponse ContainerResponse { get; set; }
        public Carrier Carrier {
            get { return ContainerResponse.Carriers.FirstOrDefault(carrier => carrier.Id == CarrierId); }
        }
    }
}