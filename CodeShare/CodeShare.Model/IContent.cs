using System;

namespace CodeShare.Model
{
    public interface IContent : IEntity
    {
        string Name { get; set; }

        int Views { get; set; }
    }
}
