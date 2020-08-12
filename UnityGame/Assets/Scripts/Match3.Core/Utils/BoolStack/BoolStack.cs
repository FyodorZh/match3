using System.Collections.Generic;
using Match3.Core;

namespace Match3.Utils
{
    /// <summary>
    /// Returns true if any active agent returns true
    /// </summary>
    public class BoolStack
    {
        private readonly List<IBoolAgent> _agents = new List<IBoolAgent>();
        
        public bool Value
        {
            get
            {
                int count = _agents.Count;
                if (count > 0)
                {
                    for (int i = count - 1; i >= 0; --i)
                    {
                        var agent = _agents[i];
                        if (!agent.IsValid)
                        {
                            _agents[i] = _agents[count - 1];
                            _agents.RemoveAt(count - 1);
                            count -= 1;
                        }
                        else
                        {
                            if (agent.Value)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }
        
        public void AddAgent(IBoolAgent agent)
        {
            Debug.Assert(!_agents.Contains(agent));
            _agents.Add(agent);
        }

        public void RemoveAgent(IBoolAgent agent)
        {
            _agents.Remove(agent);
        }
        
        public static implicit operator bool(BoolStack stack)
        {
            return stack.Value;
        }
    }
}