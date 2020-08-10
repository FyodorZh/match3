using System;

namespace Match3.Features
{
    public class MassComponentFeature : ICellObjectComponentFeature
    {
        public const string Name = "Mass"; 
        public static readonly MassComponentFeature Instance = new MassComponentFeature();

        public string FeatureId => Name;
        
        public IObjectComponent Construct(IObjectComponentData data)
        {
            if (!(data is IMassData massData))
                throw new InvalidOperationException();
            
            return Construct(massData);
        }

        public IMass Construct(IMassData data)
        {
            return new Mass(data);
        }
        
        public interface IMass : ICellObjectComponent
        {
            bool Locked { get; set; }
        }

        public interface IMassData : ICellObjectComponentData
        {
        }
        
        private class Mass : IMass
        {
            public string TypeId => Name;

            public Mass(IMassData data)
            {
            }

            public bool Locked { get; set; }
        }
    }
}