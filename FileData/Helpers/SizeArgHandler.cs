using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdPartyTools;

namespace FileData.Helpers
{
    class SizeArgHandler : IArgHandler
    {
        public bool IsValidArg(string arg)
        {
            // Just doing some simple case sensitive string matching
            switch (arg)
            {
                case "-s":
                case "--s":
                case "/s":
                case "--size":
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        public string RunArgAction(string filePath)
        {
            return new FileDetails().Size(filePath).ToString();
        }
    }
}
