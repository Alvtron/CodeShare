using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeShare.Model
{
    public class CodeFile : File
    {
        public virtual Code Code { get; set; }
        public Guid? CodeUid { get; set; }
        public virtual CodeLanguage CodeLanguage { get; set; }
        public Guid? CodeLanguageUid { get; set; }

        public CodeFile() { }

        public CodeFile(CodeLanguage codeLanguage, string data, string name, string extension)
            : base(data, name, extension)
        {
            CodeLanguageUid = codeLanguage.Uid;
        }
    }
}
