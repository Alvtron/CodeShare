using System;

namespace CodeShare.Model
{
    public interface ITimeRecord
    {
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}