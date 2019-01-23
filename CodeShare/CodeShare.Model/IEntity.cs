using System;
using System.Collections.Generic;

namespace CodeShare.Model
{
    public interface IEntity
    {
        Guid Uid { get; set; }
        DateTime? Created { get; set; }
        DateTime? Updated { get; set; }
    }
}
