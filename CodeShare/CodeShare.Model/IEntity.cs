using System;
using System.Collections.Generic;

namespace CodeShare.Model
{
    public interface IEntity : ITimeRecord, IComparable
    {
        Guid Uid { get; set; }
    }
}
