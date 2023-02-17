using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookRCv001.Interfaces
{
    public interface IDisplayProgressTarget
    {
        public double ProgressValue { get; set; }
    }
}
