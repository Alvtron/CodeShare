using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeShare.Model
{
    public class CodeLanguage : Entity
    {
        public string Name { get; set; }

        public string Extension { get; set; }

        public string Description { get; set; }

        public string About { get; set; }

        public Syntax Syntax { get; set; } = new Syntax();

        public override string ToString()
        {
            return Extension;
        }
    }
}
