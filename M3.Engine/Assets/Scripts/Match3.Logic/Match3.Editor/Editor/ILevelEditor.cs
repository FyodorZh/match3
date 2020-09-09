using System;

namespace Match3.Editor
{
    public interface ILevelEditor
    {
        IGameObserver View { get; }

        event Action LevelReloaded;

        bool LoadLevel(ILevelData data);
        ILevelData SaveLevel();
    }
}