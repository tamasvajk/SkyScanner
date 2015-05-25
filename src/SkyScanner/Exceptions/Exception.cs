// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace SkyScanner.Exceptions
{
    [Serializable]
    public class Exception : System.Exception
    {
        public Exception()
        {
        }

        public Exception(string message)
            : base(message)
        {
        }
        public Exception(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}