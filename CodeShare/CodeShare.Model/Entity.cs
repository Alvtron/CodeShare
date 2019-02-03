using System;

namespace CodeShare.Model
{
    public abstract class Entity : ObservableObject, IEntity, IEquatable<Entity>
    {
        public Guid Uid { get; set; } = Guid.NewGuid();

        public DateTime? Created { get; set; } = DateTime.Now;

        private DateTime? _updated = DateTime.Now;
        public DateTime? Updated
        {
            get => _updated;
            set => SetField(ref _updated, value);
        }

        public bool Equals(Entity other)
        {
            return Uid.Equals(other?.Uid);
        }

        public override int GetHashCode()
        {
            return Uid.GetHashCode();
        }
    }
}
