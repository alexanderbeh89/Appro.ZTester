using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Services;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.TestBlocks;
using Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.TestBlocks.Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.TestBlocks;

namespace Appro.ZTester.QDOS._14514ButtonMicFunctionalTester.Common
{
    public class ServiceFactory
    {
        public static IRunTest GetIRunTestInstance(String testName)
        {
            if (testName == "UpVolume")
            {
                return new UpVolume();
            }
            if (testName == "StartTest")
            {
                return new StartTest();
            }

            return null;
        }
    }
}
