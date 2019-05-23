using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public interface IFile
    {
        string Data { get; set; }
        string Name { get; set; }
        string Extension { get; set; }
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}
