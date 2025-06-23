using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototype.GameState
{
    abstract class Staterecipe
    {
        public abstract void initialize();
        public abstract void load(ContentManager cm);
        public abstract void update(GameTime gametime, GameStates gameStates);
        public abstract void draw(GameTime gametime);
        public abstract void drawUI();
    }
}
