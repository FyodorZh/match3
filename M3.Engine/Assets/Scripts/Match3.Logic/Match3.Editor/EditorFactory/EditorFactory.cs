using Match3.Editor;

namespace Match3
{
    public static class EditorFactory
    {
        public static ILevelEditor Construct(IGameRules rules)
        {
            return new LevelEditor(rules);
        }
    }
}