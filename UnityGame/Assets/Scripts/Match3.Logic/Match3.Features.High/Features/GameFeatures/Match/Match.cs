using System;
using System.Collections.Generic;
using Match3.Utils;

namespace Match3.Features
{
    public sealed partial class Match : StatelessGameFeature
    {
        public override IEnumerable<ICellComponentFeature> DependsOnCellComponentFeatures { get; } = new ICellComponentFeature[]
        {
        };

        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {
        };

        public override IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; } = new IObjectComponentFeature[]
        {
            ColorObjectComponentFeature.Instance,
        };

        public Match()
            : base("Match")
        {
        }


        public interface IBonusFactory : IFactory<ICellObjectData, int>
        {

        }

        private readonly List<MatchInfo> _patterns = new List<MatchInfo>();
        private readonly HashSet<Offsets2D> _patternSet = new HashSet<Offsets2D>();

        public void RegisterPatterns(Offsets2D colorMatch, IBonusFactory bonusFactory, Offsets2D bonusPoints = null)
        {
            if (_patternSet.Contains(colorMatch))
            {
                throw new InvalidOperationException();
            }

            for (int i = 0; i < 4; ++i)
            {
                if (_patternSet.Add(colorMatch))
                {
                    _patterns.Add(new MatchInfo(colorMatch, bonusFactory, bonusPoints));
                }

                colorMatch = colorMatch.RotateRight();

                if (bonusPoints != null)
                {
                    bonusPoints = bonusPoints.RotateRight();
                    bonusPoints.OffsetPivot(colorMatch.MinX, colorMatch.MinY);
                }

                colorMatch.OffsetPivot(colorMatch.MinX, colorMatch.MinY);
            }
        }

        private class MatchInfo
        {
            public readonly Offsets2D MatchPattern;
            public readonly IBonusFactory BonusFactory;
            public readonly Offsets2D BonusPlacement;

            public MatchInfo(Offsets2D matchPattern, IBonusFactory bonusFactory = null, Offsets2D bonusPlacement = null)
            {
                MatchPattern = matchPattern;
                BonusFactory = bonusFactory;
                BonusPlacement = bonusPlacement;

                Debug.Assert(bonusFactory != null || bonusPlacement == null);
            }
        }
    }
}