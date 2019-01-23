using System;

namespace CodeShare.Model
{
    public interface IContentImage
    {
        Content Content { get; set; }
        Guid ContentUid { get; set; }
    }
}