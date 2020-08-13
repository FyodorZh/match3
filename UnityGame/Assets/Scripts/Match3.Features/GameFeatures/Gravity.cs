using System.Collections.Generic;
using Match3.Core;
using Match3.Math;

namespace Match3.Features
{
    public sealed class Gravity : GameFeature<Gravity.State>
    {
        public override IEnumerable<ICellComponentFeature> DependsOnCellComponentFeatures { get; } = new ICellComponentFeature[]
        {
        };

        public override IEnumerable<IObjectFeature> DependsOnObjectFeatures { get; } = new IObjectFeature[]
        {
        };

        public override IEnumerable<IObjectComponentFeature> DependsOnObjectComponentFeatures { get; } = new IObjectComponentFeature[]
        {
            MoveObjectComponentFeature.Instance,
            MassObjectComponentFeature.Instance,
        };

        public Gravity()
            : base("Gravity")
        {
        }

        protected override State ConstructState(IGame game)
        {
            return new State(game);
        }

        public class MoveAgent : ITrajectory
        {
            public readonly GridId GridId;
            public readonly ICell Destination;
            public readonly CellPosition FallFrom;
            public readonly CellPosition FallTo;

            public readonly MoveObjectComponentFeature.IMove FallingObjectMover;

            private readonly ReleasableLock _lock;

            private bool _isFinished;

            public FixedVector2 Direction;
            public Fixed Speed;

            public FixedVector2 Position { get; set; }
            public FixedVector2 Velocity => Direction * Speed;

            public bool IsFinished => _isFinished;

            public MoveAgent(GridId gridId, MoveObjectComponentFeature.IMove fallingObjectMover, CellPosition fallFrom, FixedVector2 fallOffset)
            {
                GridId = gridId;
                Destination = fallingObjectMover.Owner.Owner;
                FallFrom = fallFrom;
                FallTo = Destination.Position;
                FallingObjectMover = fallingObjectMover;

                Position = fallOffset;

                if (Position.X == 0)
                {
                    Direction = new FixedVector2(0, -1);
                    Speed = -FallingObjectMover.Velocity.Y;
                }
                else
                {
                    Direction = -fallOffset.Normalized;
                    Speed = -FallingObjectMover.Velocity.Y;
                }


                _lock = new ReleasableLock();
                FallingObjectMover.Owner.Owner.AddLock(_lock);
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

            public FixedVector2 VisualPos => Position + new FixedVector2(FallTo.X, FallTo.Y);
        }

        public class State
        {
            public readonly List<MoveAgent> AgentsToRegister = new List<MoveAgent>();

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
                            MassObjectComponentFeature.IMass massComponent = cell.FindObjectComponent<MassObjectComponentFeature.IMass>();

                            if (massComponent != null && !massComponent.IsLocked)
                            {
                                ICellObject objectToFall = massComponent.Owner;

                                var moveComponent = objectToFall.TryGetComponent<MoveObjectComponentFeature.IMove>();
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

                                            var freeMass = cellToCheck.FindObjectComponent<MassObjectComponentFeature.IMass>();
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
                                            var agent = new MoveAgent(grid.Id, moveComponent, cell.Position, currentPosition - finalPosition);
                                            AgentsToRegister.Add(agent);
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
                                if (!cellBlock.IsActive)
                                {
                                    block = true;
                                }
                                else
                                {
                                    var mass = cellBlock.FindObjectComponent<MassObjectComponentFeature.IMass>();
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

                                    {
                                        MoveObjectComponentFeature.IMove moveComponent = emptyCell.FindObjectComponent<MoveObjectComponentFeature.IMove>();
                                        if (moveComponent != null && moveComponent.IsMoving)
                                            break;
                                        EmitterObjectComponentFeature.IEmitter emitterComponent = emptyCell.FindObjectComponent<EmitterObjectComponentFeature.IEmitter>();
                                        if (emitterComponent != null)
                                            break;
                                    }

                                    if (emptyCell.IsActive && !emptyCell.IsLocked && emptyCell.FindObjectComponent<MassObjectComponentFeature.IMass>() == null)
                                    {
                                        MoveObjectComponentFeature.IMove sourceCellMove = null;

                                        bool foundMovement = false;
                                        {
                                            ICell cellToCheck = grid.GetCell(new CellPosition(x - 1, i + 1));
                                            if (cellToCheck != null)
                                            {
                                                var cellMove = cellToCheck.FindObjectComponent<MoveObjectComponentFeature.IMove>();
                                                if (cellMove != null)
                                                {
                                                    if (!cellMove.IsMoving)
                                                    {
                                                        if (!cellToCheck.IsLocked)
                                                        {
                                                            var mass = cellToCheck.FindObjectComponent<MassObjectComponentFeature.IMass>();
                                                            if (mass != null && !mass.IsLocked)
                                                            {
                                                                sourceCellMove = cellMove;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        foundMovement = true;
                                                    }
                                                }
                                            }
                                        }

                                        if (sourceCellMove == null)
                                        {
                                            ICell cellToCheck = grid.GetCell(new CellPosition(x + 1, i + 1));
                                            if (cellToCheck != null)
                                            {
                                                var cellMove = cellToCheck.FindObjectComponent<MoveObjectComponentFeature.IMove>();
                                                if (cellMove != null)
                                                {
                                                    if (!cellMove.IsMoving)
                                                    {
                                                        if (!cellToCheck.IsLocked)
                                                        {
                                                            var mass = cellToCheck.FindObjectComponent<MassObjectComponentFeature.IMass>();
                                                            if (mass != null && !mass.IsLocked)
                                                            {
                                                                sourceCellMove = cellMove;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        foundMovement = true;
                                                    }
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
                                                var agent = new MoveAgent(grid.Id, sourceCellMove, sourceCell.Position, currentPosition - finalPosition);
                                                AgentsToRegister.Add(agent);
                                            }
                                            else
                                            {
                                                Debug.Assert(false);
                                            }

                                            break;
                                        }
                                        else
                                        {
                                            if (foundMovement)
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            public void RegisterNewAgents()
            {
                foreach (var newAgent in AgentsToRegister)
                {
                    var agents = Agents[newAgent.GridId][newAgent.FallTo.X];

                    bool inserted = false;
                    for (int i = 0; i < agents.Count; ++i)
                    {
                        if (agents[i].FallTo.Y > newAgent.FallTo.Y)
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
                AgentsToRegister.Clear();
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
                            Fixed maxSpeedY;
                            if (prevAgent == null)
                            {
                                maxSpeedY = 1000; // just big
                            }
                            else
                            {
                                var selfPosY = agent.VisualPos.Y;
                                var obstaclePosY = prevAgent.VisualPos.Y;
                                var possibleDistanceY = selfPosY - obstaclePosY - 1;
                                if (possibleDistanceY < 0)
                                {
                                    possibleDistanceY = 0;
                                }

                                maxSpeedY = possibleDistanceY / dTimeSeconds;
                            }

                            if (agent.Direction.X == 0)
                            {
                                agent.Speed = FixedMath.Min(agent.Speed + speedUp, maxSpeedY);
                            }
                            else
                            {
                                var maxSpeed = maxSpeedY * FixedMath.Abs(agent.Direction.Y / agent.Direction.X);
                                var desiredSpeed = agent.Speed + (speedUp * 2); // wrong speedUp sqrt(2) ?
                                if (maxSpeed < desiredSpeed)
                                    agent.Speed = maxSpeed;
                                else
                                    agent.Speed = desiredSpeed;
                            }

                            agent.Position += agent.Velocity * dTimeSeconds;

                            if (agent.Position.Y <= Fixed.Eps)
                            {
                                agent.Position = new FixedVector2(0, 0);
                                agent.Finish();
                                finished = true;
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
            state.RegisterNewAgents();
            state.ProcessAgents(new Fixed(dTimeMs, 1000));
        }
    }
}