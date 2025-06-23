using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototype.Utilities_classes
{
    internal class Camera
    {
        public Matrix transformationmat;
        public Vector2 position;
        public void update(player player)
        {
            position = player.position;
            //clamp method keep the positions of the camera focus in this range, so I doesn't show the view beyond the background size
            position.X = MathHelper.Clamp(position.X, 960, 6720);
            position.Y = MathHelper.Clamp(position.Y, 540, 3780);
            //first translation moves the map opposite direction to the player, second translation kept player at center of the screen
            transformationmat = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) * Matrix.CreateTranslation(new Vector3(960,480, 0));
        }

    }  
}
