using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeShare.Model
{
    public class File : Entity, IFile
    {
        public Content Content { get; set; }

        public Guid? ContentUid { get; set; }

        private string _data;
        public string Data
        {
            get => _data;
            set
            {
                _data = value;
                Updated = DateTime.Now;
            }
        }

        public string Name { get; set; }

        public string Extension { get; set; }

        public int Lines => Data.Split('\n').Length;

        public File() { }

        public File(string data, string name, string extension)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Extension = extension ?? throw new ArgumentNullException(nameof(extension));
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }

        public override string ToString() => $"{Name}{Extension}";
    }
}
