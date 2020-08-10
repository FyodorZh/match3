using System;
using Match3.Math;

namespace Match3.Features
{
    public class MoveComponentFeature : ICellObjectComponentFeature
    {
        public const string Name = "Move"; 
        public static readonly MoveComponentFeature Instance = new MoveComponentFeature();

        public string FeatureId => Name;
        
        public IObjectComponent Construct(IObjectComponentData data)
        {
            if (!(data is IMoveData MoveData))
                throw new InvalidOperationException();
            
            return Construct(MoveData);
        }

        public IMove Construct(IMoveData data)
        {
            return new Move(data);
        }
        
        public interface IMove : ICellObjectComponent
        {
            ITrajectory Trajectory { get; set; }
        }

        public interface IMoveData : ICellObjectComponentData
        {
        }
        
        private class Move : IMove
        {
            public string TypeId => Name;

            public Move(IMoveData data)
            {
            }

            public ITrajectory Trajectory { get; set; }
        }
    }
}