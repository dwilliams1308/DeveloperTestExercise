using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileData.Helpers
{
    public interface IArgHandler
    {
        bool IsValidArg(string arg);

        string RunArgAction(string filePath);
    }
}
