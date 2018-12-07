using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdPartyTools;

namespace FileData.Helpers
{
    class VersionArgHandler : IArgHandler
    {
        public bool IsValidArg(string arg)
        {
            // Just doing some simple case sensitive string matching
            switch(arg)
            {
                case "-v":
                case "--v":
                case "/v":
                case "--version":
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
            return new FileDetails().Version(filePath);
        }
    }
}
