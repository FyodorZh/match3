using System.Collections.Generic;
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
            return new State(game);
        }
        
        public readonly struct FallRequestInfo
        {
            public readonly IGrid Grid;
            public readonly MoveComponentFeature.IMove FallingObjectMover;
            public readonly CellPosition FallFrom;
            public readonly FixedVector2 FallOffset;
            public readonly ReleasableLock Lock; 

            public FallRequestInfo(IGrid grid, MoveComponentFeature.IMove fallingObjectMover, CellPosition fallFrom, FixedVector2 fallOffset)
            {
                Grid = grid;
                FallingObjectMover = fallingObjectMover;
                FallFrom = fallFrom;
                FallOffset = fallOffset;

                Lock = new ReleasableLock();
                fallingObjectMover.Owner.Owner.AddLock(Lock);
            }
        }
        
        public class MoveAgent : ITrajectory
        {
            public readonly int DstY;
            
            public readonly MoveComponentFeature.IMove FallingObjectMover;

            private bool _isFinished;
            private ReleasableLock _lock;

            public FixedVector2 Direction;
            public Fixed Speed;

            public FixedVector2 Position { get; set; }
            public FixedVector2 Velocity => Direction * Speed;

            public bool IsFinished => _isFinished;
            
            public MoveAgent(FallRequestInfo info)
            {
                DstY = info.FallingObjectMover.Owner.Owner.Position.Y;
                FallingObjectMover = info.FallingObjectMover;

                Position = info.FallOffset;

                if (Position.X == 0)
                {
                    Direction = new FixedVector2(0, -1);
                    Speed = -FallingObjectMover.Velocity.Y;
                }
                else
                {
                    Direction = info.FallOffset.Normalized;
                    Speed = -FallingObjectMover.Velocity.Y;
                }

                _lock = info.Lock;
                
                FallingObjectMover.SetTrajectory(this);
            }

            public void Finish()
            {
                _isFinished = true;
                _lock.Release();
            }
            
            public bool Update(Fixed timeSeconds)
            {
                return !_isFinished;
            }

            public FixedVector2 VisualPos
            {
                get
                {
                    var cell = FallingObjectMover?.Owner?.Owner;

                    return cell != null ? (Position + new FixedVector2(cell.Position.X, cell.Position.Y)) : new FixedVector2(0, 0);
                }
            }
        }
        
        public class State
        {
            public readonly List<FallRequestInfo> FallRequests = new List<FallRequestInfo>();
            
            public readonly Dictionary<GridId, List<MoveAgent>[]> Agents = new Dictionary<GridId, List<MoveAgent>[]>();

            public State(IGame game)
            {
                foreach (var grid in game.Board.Grids)
                {
                    var list = new List<MoveAgent>[grid.Width];
                    for (int i = 0; i < grid.Width; ++i)
                    {
                        list[i] = new List<MoveAgent>();
                    }
                    Agents.Add(grid.Id, list);
                }
            }

            public void FindNewObjectsToFall(IGame game)
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
                                ICellObject objectToFall = massComponent.Owner;

                                var moveComponent = objectToFall.TryGetComponent<MoveComponentFeature.IMove>();
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

                                            if (!cellToCheck.CanAttach(objectToFall))
                                            {
                                                break;
                                            }

                                            freeCell = cellToCheck;
                                        }
                                        else
                                        {
                                            break;
                                        }

                                        --k;
                                    }

                                    if (freeCell != null)
                                    {
                                        var currentPosition = moveComponent.VisualPosition();
                                        if (freeCell.Attach(objectToFall))
                                        {
                                            FixedVector2 finalPosition = new FixedVector2(freeCell.Position.X, freeCell.Position.Y);
                                            var fallRequest = new FallRequestInfo(grid, moveComponent, cell.Position, currentPosition - finalPosition);
                                            FallRequests.Add(fallRequest);
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

            public void FindNewObjectsToSideFall(IGame game)
            {
                foreach (var grid in game.Board.Grids)
                {
                    for (int x = 0; x < grid.Width; ++x)
                    {
                        for (int y = grid.Height; y >= 0; --y)
                        {
                            bool block = false;
                            if (y == grid.Height)
                            {
                                block = true;
                            }
                            else
                            {
                                ICell cellBlock = grid.GetCell(new CellPosition(x, y));
                                if (!cellBlock.IsActive || cellBlock.IsLocked)
                                {
                                    block = true;
                                }
                                else
                                {
                                    var mass = cellBlock.FindComponent<MassComponentFeature.IMass>();
                                    if (mass != null && mass.IsLocked)
                                    {
                                        block = true;
                                    }
                                }
                            }

                            if (block)
                            {
                                for (int i = y - 1; i >= 0; --i)
                                {
                                    ICell emptyCell = grid.GetCell(new CellPosition(x, i));
                                    if (emptyCell.IsActive && !emptyCell.IsLocked && emptyCell.FindComponent<MassComponentFeature.IMass>() == null)
                                    {
                                        MoveComponentFeature.IMove sourceCellMove = null;

                                        ICell cellLeft = grid.GetCell(new CellPosition(x - 1, i + 1));
                                        if (cellLeft != null && !cellLeft.IsLocked)
                                        {
                                            var mass = cellLeft.FindComponent<MassComponentFeature.IMass>();
                                            if (mass != null && !mass.IsLocked)
                                            {
                                                sourceCellMove = cellLeft.FindComponent<MoveComponentFeature.IMove>();
                                            }
                                        }

                                        if (sourceCellMove == null)
                                        {
                                            ICell cellRight = grid.GetCell(new CellPosition(x + 1, i + 1));
                                            if (cellRight != null && !cellRight.IsLocked)
                                            {
                                                var mass = cellRight.FindComponent<MassComponentFeature.IMass>();
                                                if (mass != null && !mass.IsLocked)
                                                {
                                                    sourceCellMove = cellRight.FindComponent<MoveComponentFeature.IMove>();
                                                }
                                            }
                                        }

                                        if (sourceCellMove != null)
                                        {
                                            var sourceCell = sourceCellMove.Owner.Owner;
                                            var currentPosition = sourceCellMove.VisualPosition();
                                            if (emptyCell.Attach(sourceCellMove.Owner))
                                            {
                                                FixedVector2 finalPosition = new FixedVector2(emptyCell.Position.X, emptyCell.Position.Y);
                                                var fallRequest = new FallRequestInfo(grid, sourceCellMove, sourceCell.Position, currentPosition - finalPosition);
                                                FallRequests.Add(fallRequest);
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
            }

            public void ConstructNewAgents()
            {
                foreach (var fallRequest in FallRequests)
                {
                    var newAgent = new MoveAgent(fallRequest);

                    var agents = Agents[fallRequest.Grid.Id][fallRequest.FallFrom.X];

                    bool inserted = false;
                    for (int i = 0; i < agents.Count; ++i)
                    {
                        if (agents[i].DstY > newAgent.DstY)
                        {
                            agents.Insert(i, newAgent);
                            inserted = true;
                            break;
                        }
                    }
                    if (!inserted)
                    {
                        agents.Add(newAgent);
                    }
                }
                FallRequests.Clear();
            }

            public void ProcessAgents(Fixed dTimeSeconds)
            {
                Fixed gravity = new Fixed(900, 100);

                Fixed speedUp = gravity * dTimeSeconds;
                
                foreach (var gridList in Agents.Values)
                {
                    foreach (var column in gridList)
                    {
                        bool finished = false;
                        
                        MoveAgent prevAgent = null;
                        foreach (var agent in column)
                        {
                            Fixed possibleDistanceY = agent.Position.Y;
                            if (prevAgent != null)
                            {
                                var obstaclePosY = agent.VisualPos.Y;
                                var selfPosY = prevAgent.VisualPos.Y;
                                possibleDistanceY = FixedMath.Min(possibleDistanceY, obstaclePosY - selfPosY - 1);
                                if (possibleDistanceY <= 0)
                                {
                                    Debug.Warning("Strange!!!");
                                }
                            }

                            if (possibleDistanceY <= 0)
                            {
                                agent.Position = new FixedVector2(0, 0);
                                agent.Finish();
                                finished = true;
                            }
                            else
                            {
                                Fixed maxVelocityY = possibleDistanceY / dTimeSeconds;

                                Fixed velocity;
                                if (agent.Position.X == 0)
                                {
                                    velocity = 
                                }

                                FixedVector2 desiredVelocity = agent.Velocity;
                                if (desiredVelocity.X == 0)
                                {
                                    desiredVelocity.Y -= speedUp;
                                    if (desiredVelocity.Y > maxVelocityY)
                                        desiredVelocity.Y = maxVelocityY;
                                }
                                else
                                {
                                    if (desiredVelocity.Y > maxVelocityY)
                                        desiredVelocity *= (maxVelocityY / desiredVelocity.Y);
                                }

                                agent.Velocity = desiredVelocity;
                                
                                agent.Position += agent.Velocity * dTimeSeconds;
                                if (agent.Position.Y <= 0)
                                {
                                    agent.Position = new FixedVector2(0, 0);
                                    agent.Finish();
                                    finished = true;
                                }
                            }

                            prevAgent = agent;
                        }

                        if (finished)
                        {
                            column.RemoveAll(a => a.IsFinished);
                        }
                    }
                }
            }
        }
        
        protected override void Process(IGame game, State state, int dTimeMs)
        {
            state.FindNewObjectsToFall(game);
            state.FindNewObjectsToSideFall(game);
            state.ConstructNewAgents();
            state.ProcessAgents(new Fixed(dTimeMs, 1000));
        }
    }
}