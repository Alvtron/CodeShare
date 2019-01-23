using System;

namespace CodeShare.Model
{
    public interface IClassifiable
    {
        string Name { get; set; }

        int Views { get; set; }

        DateTime? Created { get; set; }

        DateTime? Updated { get; set; }
    }
}
