using System;

namespace Match3.Core
{
    public class Pattern2D
    {
        public readonly int Length;

        public readonly int MaxX;
        public readonly int MaxY;
        public readonly int MinX;
        public readonly int MinY;

        public readonly int[] OffsetsX;
        public readonly int[] OffsetsY;

        public Pattern2D(int[][] list)
        {
            Length = list.Length;
            OffsetsX = new int[Length];
            OffsetsY = new int[Length];

            for (int i = 0; i < Length; ++i)
            {
                int x = list[i][0];
                int y = list[i][1];

                MaxX = Math.Max(MaxX, x);
                MaxY = Math.Max(MaxY, y);

                MinX = Math.Min(MinX, x);
                MinY = Math.Min(MinY, y);

                OffsetsX[i] = x;
                OffsetsY[i] = y;
            }
        }
    }
}