// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace SkyScanner.Data.Base
{
    internal interface IContainerResponse
    {
        List<Place> Places { get; }
        List<LegSegment> Segments { get; }
        List<Carrier> Carriers { get; }
    }
}