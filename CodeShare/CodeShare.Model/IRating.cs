using System;

namespace CodeShare.Model
{
    public interface IRating
    {
        User User { get; set; }
        Guid? UserUid { get; set; }
        bool Value { get; set; }
        DateTime? Date { get; set; }
    }
}
