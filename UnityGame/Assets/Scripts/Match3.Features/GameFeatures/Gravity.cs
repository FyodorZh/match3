﻿using System.Collections.Generic;
using Match3.Core;
using Match3.Math;

namespace Match3.Features
{
    public sealed class Gravity : GameFeature<Gravity.State>
    {
        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {
        };
        
        public override IEnumerable<IComponentFeature> DependsOnComponentFeatures { get; } = new IComponentFeature[]
        {
            MoveComponentFeature.Instance, 
            MassComponentFeature.Instance,
        };

        public Gravity() 
            : base("Gravity")
        {
        }

        protected override State ConstructState(IGame game)
        {
            return new State();
        }

        protected override void Process(IGame game, State state, int dTimeMs)
        {
            foreach (var grid in game.Board.Grids)
            {
                for (int x = 0; x < grid.Width; ++x)
                {
                    for (int y = 1; y < grid.Height; ++y)
                    {
                        ICell cell = grid.GetCell(new CellPosition(x, y));
                        MassComponentFeature.IMass massComponent = cell.FindComponent<MassComponentFeature.IMass>();
                        if (massComponent != null && !massComponent.IsLocked)
                        {
                            var moveComponent = massComponent.Owner.TryGetComponent<MoveComponentFeature.IMove>();
                            if (moveComponent != null && !moveComponent.IsMoving)
                            {
                                int k = y - 1;
                                ICell freeCell = null;
                                while (k >= 0)
                                {
                                    ICell cellToCheck = grid.GetCell(new CellPosition(x, k));
                                    if (cellToCheck.IsActive)
                                    {
                                        if (cellToCheck.IsLocked)
                                        {
                                            break;
                                        }
                                        
                                        var freeMass = cellToCheck.FindComponent<MassComponentFeature.IMass>();
                                        if (freeMass != null)
                                        {
                                            break;
                                        }

                                        freeCell = cellToCheck;
                                    }
                                    --k;
                                }

                                var objectToFall = massComponent.Owner;
                                if (freeCell != null && freeCell.CanAttach(objectToFall))
                                {
                                    if (freeCell.Attach(objectToFall))
                                    {
                                        Fixed distanceToFall = cell.Position.Y - freeCell.Position.Y;
                                        Fixed initialSpeed = moveComponent.Velocity.Y;
                                        var trajectory = new FallTrajectory(distanceToFall, initialSpeed);
                                        
                                        freeCell.AddLock(objectToFall);
                                        
                                        moveComponent.SetTrajectory(trajectory, null, () => freeCell.RemoveLock(objectToFall));

                                        //Debug.Log("FALL " + cell.Position + " -> " + freeCell.Position);
                                    }
                                    else
                                    {
                                        Debug.Assert(false);
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        
        public class State
        {
            
        }
     
        private class FallTrajectory : Trajectory
        {
            private Fixed _height;
            private Fixed _velocity;
            
            public FallTrajectory(Fixed height, Fixed velocity)
            {
                _height = height;
                _velocity = velocity;
                Position = new FixedVector2(0, height);
                Velocity = new FixedVector2(0, velocity);
            }
            
            protected override bool OnUpdate(Fixed timeSeconds)
            {
                _velocity += timeSeconds * new Fixed(15, 100); // 0.1
                _height -= _velocity;
                
                Velocity = new FixedVector2(0, _velocity);
                if (_height <= 0)
                {
                    Position = new FixedVector2(0, 0);
                    return false;
                }
                else
                {
                    Position = new FixedVector2(0, _height);
                    return true;
                }
            }
        }
    }
}