// Copyright (c) 2015 Tamas Vajk. All Rights Reserved. Licensed under the MIT License. See License.txt in the project root for license information.

using System.Configuration;
using SkyScanner.Services;

namespace SkyScanner.Test
{
    public abstract class TestBase
    {
        protected Scanner Scanner;
        public virtual void Setup()
        {
            Scanner = new Scanner(ConfigurationManager.AppSettings["apiKey"]);
        }
    }
}
