using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShare.Model
{
    public interface IPassword
    {
        int Iterations { get; set; }
        byte[] Salt { get; set; }
        string Hash { get; set; }
    }
}
