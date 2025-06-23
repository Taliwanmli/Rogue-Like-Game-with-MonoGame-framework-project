using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using prototype.GameState;
using prototype.GameState.States;

namespace prototype.Utilities_classes
{
    internal class Button
    {
        public string name;
        public int buttonindex;
        Texture2D buttontexture;
        Vector2 position;
        Rectangle clickbox;
        //classes used
        //MouseState mousestate;
        mousedetection mouse = new mousedetection();
        // name, position, clickbox rectangle
        public Button(string name, Vector2 position, Rectangle clickbox)
        {
            this.name = name;
            this.position = position;
            this.clickbox = clickbox;
        }
        public Button(string name, Vector2 position, Rectangle clickbox,int buttonindex)
        {
            this.name = name;
            this.position = position;
            this.clickbox = clickbox;
            this.buttonindex = buttonindex;
        }

        public void load(ContentManager cm) 
        {
            buttontexture = cm.Load<Texture2D>(name);
        }
        public bool update()
        {   
            mouse.mouseactivityupdate(true);//check mouse click and check if it was click on the button
            if(new Rectangle ((int)mouse.leftclickedposition.X,(int)mouse.leftclickedposition.Y, 1,1).Intersects(clickbox)&& mouse.leftbuttonpressed)
            {
                return true;    
            }
            return false;   
            
        }
        public void draw(SpriteBatch spritebatch)
        {   
            spritebatch.Draw(buttontexture, position, Color.White);
        }
    }
}