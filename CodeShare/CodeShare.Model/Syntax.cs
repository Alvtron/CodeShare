using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeShare.Model
{
    [ComplexType]
    public class Syntax
    {
        public string Delimiter { get; set; } = "";
        public string Keywords { get; set; } = "";
        public string Comments { get; set; } = "";

        public Syntax() { }

        public Syntax(string delimiter, string keywords, string comments)
        {
            Delimiter = delimiter ?? "";
            Keywords = keywords ?? "";
            Comments = comments ?? "";
        }
    }
}
