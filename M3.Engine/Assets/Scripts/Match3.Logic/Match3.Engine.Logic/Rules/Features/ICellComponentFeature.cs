using System.Collections.Generic;

namespace Match3.Features
{
    public interface ICellComponentFeature : IFeature
    {
        void InitState(IGame game);
    }
}