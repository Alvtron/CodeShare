using System;

namespace CodeShare.Model
{
    public abstract class Entity : ObservableObject, IEntity, IComparable<Entity>
    {
        public Guid Uid { get; set; } = Guid.NewGuid();

        public DateTime? Created { get; set; } = DateTime.Now;

        private DateTime? _updated = DateTime.Now;
        public DateTime? Updated
        {
            get => _updated;
            set => SetField(ref _updated, value);
        }

        public int CompareTo(Entity entity) =>
            Created.HasValue && entity.Created.HasValue
            ? Created.Value.CompareTo(entity.Created.Value)
            : 0;

    }
}
