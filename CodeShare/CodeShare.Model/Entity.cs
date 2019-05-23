using System;

namespace CodeShare.Model
{
    public abstract class Entity : ObservableObject, IEntity, IEquatable<Entity>, IComparable
    {
        public Guid Uid { get; set; } = Guid.NewGuid();

        public DateTime Created { get; set; } = DateTime.Now;

        private DateTime _updated = DateTime.Now;
        public DateTime Updated
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

        public int CompareTo(object obj)
        {
            if (obj is IEntity entity)
            {
                return Uid.CompareTo(entity.Uid);
            } 
            else if (obj is ITimeRecord timeRecord)
            {
                return Created.CompareTo(timeRecord.Created);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
