using System;
using System.Collections.Generic;

namespace Match3.Core
{
    public class Offsets2D : IEquatable<Offsets2D>
    {
        private CellOffset[] _offsets;
        public int Length { get; private set; }

        public int MaxX { get; private set;}
        public int MaxY { get; private set;}
        public int MinX { get; private set;}
        public int MinY { get; private set;}

        public Offsets2D(int[][] list)
        {
            var length = list.Length;
            var offsets = new CellOffset[length];
            for (int i = 0; i < length; ++i)
            {
                int x = list[i][0];
                int y = list[i][1];
                offsets[i] = new CellOffset(x, y);
            }

            Init(offsets);
            Normalize(true);
        }

        public Offsets2D(CellOffset[] offsets)
        {
            Init(offsets);
            Normalize(true);
        }

        public Offsets2D(params string[] lines)
        {
            int len = lines[0].Length;
            foreach (var line in lines)
            {
                if (line.Length != len)
                {
                    throw new InvalidOperationException();
                }
            }

            List<CellOffset> offsets = new List<CellOffset>();

            int y = 0;
            for (int i = lines.Length - 1; i >= 0; --i)
            {
                string line = lines[i];

                int x = 0;
                foreach (var ch in line)
                {
                    if (ch == '*')
                    {
                        offsets.Add(new CellOffset(x, y));
                    }
                    ++x;
                }
                ++y;
            }

            Init(offsets.ToArray());
            Normalize(false);
        }

        public void OffsetPivot(int dx, int dy)
        {
            MinX -= dx;
            MinY -= dy;

            MaxX -= dx;
            MaxY -= dy;

            for (int i = 0; i < Length; ++i)
            {
                _offsets[i] = new CellOffset(_offsets[i].X - dx, _offsets[i].Y - dy);
            }
        }

        public Offsets2D RotateRight()
        {
            CellOffset[] offsets = new CellOffset[Length];
            for (int i = 0; i < Length; ++i)
            {
                offsets[i] = new CellOffset(_offsets[i].Y, -_offsets[i].X);
            }
            return new Offsets2D(offsets);
        }

        public CellOffset OffsetAt(int id)
        {
            return _offsets[id];
        }

        private void Init(CellOffset[] offsets)
        {
            Length = offsets.Length;
            _offsets = offsets;

            for (int i = 0; i < Length; ++i)
            {
                int x = offsets[i].X;
                int y = offsets[i].Y;

                if (i == 0)
                {
                    MaxX = MinX = x;
                    MaxY = MinY = y;
                }
                else
                {
                    MaxX = Math.Max(MaxX, x);
                    MaxY = Math.Max(MaxY, y);

                    MinX = Math.Min(MinX, x);
                    MinY = Math.Min(MinY, y);
                }
            }
        }

        private void Normalize(bool deduplicate)
        {
            Array.Sort(_offsets, (left, right) =>
            {
                int cmp = left.Y.CompareTo(right.Y);
                if (cmp == 0)
                    cmp = left.X.CompareTo(right.X);
                return cmp;
            });

            if (deduplicate)
            {
                bool bFoundDuplicates = false;
                for (int i = 1; i < Length; ++i)
                {
                    if (_offsets[i - 1].Equals(_offsets[i]))
                    {
                        bFoundDuplicates = true;
                        break;
                    }
                }

                if (bFoundDuplicates)
                {
                    List<CellOffset> offsets = new List<CellOffset>();
                    var offset = _offsets[0];
                    offsets.Add(offset);
                    for (int i = 1; i < Length; ++i)
                    {
                        if (!offset.Equals(_offsets[i]))
                        {
                            offset = _offsets[i];
                            offsets.Add(offset);
                        }
                    }

                    _offsets = offsets.ToArray();
                }
            }
        }

        public bool Equals(Offsets2D other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Length != other.Length)
                return false;

            for (int i = 0; i < Length; ++i)
            {
                if (!_offsets[i].Equals(other._offsets[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Offsets2D)obj);
        }

        public override int GetHashCode()
        {
            return Length;
        }

        public static bool operator ==(Offsets2D left, Offsets2D right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Offsets2D left, Offsets2D right)
        {
            return !Equals(left, right);
        }
    }
}