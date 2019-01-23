using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace CodeShare.Model
{
    public interface IComment
    {
        Guid Uid { get; set; }
        User User { get; set; }
        Guid? UserUid { get; set; }
        string Text { get; set; }
    }
}
