using System.Collections.Generic;
using Match3.Core;
using Match3.Features.Emitter;

namespace Match3.Features
{
    public sealed class Emitters : StatelessGameFeature
    {
        public override IEnumerable<ICellComponentFeature> DependsOnCellComponentFeatures { get; } = new ICellComponentFeature[]
        {
        };

        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {
        };

        public override IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; } = new IObjectComponentFeature[]
        {
        };

        public Emitters()
            : base("Emitters")
        {
        }

        protected override void Process(IGame game, DeltaTime dTime)
        {
            foreach (var grid in game.Board.Grids)
            {
                foreach (var cell in grid.AllCells)
                {
                    ICellObject obj = null;
                    foreach (var _obj in cell.Objects)
                    {
                        if (obj == null)
                        {
                            obj = _obj;
                        }
                        else
                        {
                            obj = null;
                            break;
                        }
                    }

                    if (obj != null)
                    {
                        var emitter = obj.TryGetComponent<IEmitterCellObjectComponent>();
                        if (emitter != null)
                        {
                            if (cell.IsActive && !cell.IsLocked)
                            {
                                var cellUnder = cell.Under();
                                Debug.Assert(cellUnder != null && cellUnder.IsActive);
                                if (!cellUnder.IsLocked)
                                {
                                    var newObject = emitter.Emit(game);
                                    if (newObject != null)
                                    {
                                        cell.Attach(newObject);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}