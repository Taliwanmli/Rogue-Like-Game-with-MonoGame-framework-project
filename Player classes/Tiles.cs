using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototype.Player_classes
{
    public enum tiletype
    {
        floor,
        wall
    }
    internal class Tiles
    {
        tiletype tiletype;
        Rectangle tilespace;
        public Tiles(tiletype tiletype,  Rectangle rectangle)
        {
            this.tiletype = tiletype;
            tilespace = rectangle;  
        }
    }
}
