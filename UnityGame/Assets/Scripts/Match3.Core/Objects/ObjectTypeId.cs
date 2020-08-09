using System;

namespace Match3
{
    public struct ObjectTypeId : IEquatable<ObjectTypeId>
    {
        public readonly string Id;

        public ObjectTypeId(string id)
        {
            Id = id;
        }

        public bool Equals(ObjectTypeId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is ObjectTypeId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public static bool operator ==(ObjectTypeId left, ObjectTypeId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ObjectTypeId left, ObjectTypeId right)
        {
            return !left.Equals(right);
        }
    }
}