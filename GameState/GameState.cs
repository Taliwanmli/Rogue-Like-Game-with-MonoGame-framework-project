using prototype.GameState.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace prototype.GameState
{
    internal class GameStates
    {
        public static Stack<Staterecipe> states = new Stack<Staterecipe>();
        public GameStates(Staterecipe temp)
        {
            states.Push(temp);
        }
        public void Push(Staterecipe temp)
        {
            states.Push(temp);
        }
        public void Pop()
        {
            states.Pop();
        }
        public Staterecipe peek()
        {
            return states.Peek();
        }
    }
}
