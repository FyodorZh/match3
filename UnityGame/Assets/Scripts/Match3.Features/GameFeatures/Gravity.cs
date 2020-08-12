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

            public FixedVector2 Position { get; set; }
            public FixedVector2 Velocity { get; set; }

            public bool IsFinished => _isFinished;
            
            public MoveAgent(FallRequestInfo info)
            {
                DstY = info.FallingObjectMover.Owner.Owner.Position.Y;
                FallingObjectMover = info.FallingObjectMover;

                Position = info.FallOffset;
                Velocity = FallingObjectMover.Velocity;

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
                Fixed gravity = new Fixed(600, 100);

                Fixed speedUp = gravity * dTimeSeconds;
                
                foreach (var gridList in Agents.Values)
                {
                    foreach (var column in gridList)
                    {
                        bool finished = false;
                        
                        MoveAgent prevAgent = null;
                        foreach (var agent in column)
                        {
                            Fixed possibleDistance = agent.Position.Y;
                            if (prevAgent != null)
                            {
                                var obstaclePos = agent.VisualPos.Y;
                                var selfPos = prevAgent.VisualPos.Y;
                                possibleDistance = FixedMath.Min(possibleDistance, obstaclePos - selfPos - 1);
                                if (possibleDistance <= 0)
                                {
                                    Debug.Warning("Strange!!!");
                                }
                            }

                            if (possibleDistance <= 0)
                            {
                                agent.Position = new FixedVector2(0, 0);
                                agent.Finish();
                                finished = true;
                            }
                            else
                            {
                                Fixed maxVelocity = possibleDistance / dTimeSeconds;

                                agent.Velocity = new FixedVector2(0, FixedMath.Min(agent.Velocity.Y + speedUp, maxVelocity));
                                agent.Position -= agent.Velocity * dTimeSeconds;
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
            state.ConstructNewAgents();
            state.ProcessAgents(new Fixed(dTimeMs, 1000));
        }
    }
}