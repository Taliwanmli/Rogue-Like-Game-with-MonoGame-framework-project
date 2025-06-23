using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace prototype
{
    internal class mousedetection
    {
        MouseState mousenow;
        MouseState mousebefore;
        public bool leftbuttonpressed = false;
        public Vector2 leftclickedposition;
        public Vector2 mouseactivityupdate()
        {
            mousenow = Mouse.GetState();
            if (mousenow.LeftButton == ButtonState.Pressed)
            {
                leftbuttonpressed = true;
                leftclickedposition = new Vector2(mousenow.X, mousenow.Y);
            }
            else
            {
                leftbuttonpressed = false;
            }
            return leftclickedposition;
        }
        public Vector2 mouseactivityupdate(bool fullpress)
        {
            mousenow = Mouse.GetState();
            if (mousenow.LeftButton == ButtonState.Released && mousebefore.LeftButton == ButtonState.Pressed)
            {
                leftbuttonpressed = true;
                leftclickedposition = new Vector2(mousenow.X, mousenow.Y);
            }
            else
            {
                leftbuttonpressed = false;
            }
            mousebefore = mousenow;
            return leftclickedposition;
        }
    }
}
