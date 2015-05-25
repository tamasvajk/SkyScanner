// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace SkyScanner.Exceptions
{
    [Serializable]
    public class ServerErrorException : Exception
    {
        public ServerErrorException(string message)
            : base(message)
        {
        }
    }
}