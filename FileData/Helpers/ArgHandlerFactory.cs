using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FileData.FileDataProcessor;

namespace FileData.Helpers
{
    public class ArgHandlerFactory : IArgHandlerFactory
    {
        public IArgHandler Create(FileActions actionType)
        {
            switch(actionType)
            {
                case FileActions.Size:
                    {
                        return new SizeArgHandler();
                    }
                case FileActions.Version:
                    {
                        return new VersionArgHandler();
                    }
                default:
                    {
                        throw new ArgumentException("File action type is unknown");
                    }
            }
        }
    }
}
