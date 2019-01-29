using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CodeShare.Model
{
    public class File : Entity, IFile
    {
        private string _data;
        public string Data
        {
            get => _data;
            set => SetField(ref _data, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        private string _extension;
        public string Extension
        {
            get => _extension;
            set => SetField(ref _extension, value);
        }

        [NotMapped, JsonIgnore]
        public string FullName => $"{Name}{Extension}";
        [NotMapped, JsonIgnore]
        public int Lines => Data == null ? 0 : Data.Split('\n').Length;

        public File() { }

        public File(string data, string name, string extension)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Extension = extension ?? throw new ArgumentNullException(nameof(extension));
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }

        public override string ToString() => FullName;
    }
}
