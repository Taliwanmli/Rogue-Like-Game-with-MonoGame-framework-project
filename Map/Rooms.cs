using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototype.Map
{
    internal class Rooms
    {
        //room position and size
        Texture2D roomfloortexture;
        Texture2D rooftexture;
        Texture2D doorhorizontaltexture;
        Texture2D doorverticaltexture;
        Texture2D walltexture;
        public Vector2 roompostition;
        public Rectangle roomspace;
        public int roomnumber;
        //room structure
        Rectangle horizontalwall1;
        Rectangle horizontalwall3;
        Rectangle verticalwall2;
        Rectangle verticalwall4;
        public Rectangle door;
        public Rectangle[] horizontalwalls;
        public Rectangle[] verticalwalls;
        public Rectangle[] walls;
        int doorside;
        public Rooms(int width, int height, Vector2 roomposition, ContentManager cm, int roomnumber)
        {
            this.roompostition = roomposition;
            roomspace = new Rectangle((int)roomposition.X, (int)roomposition.Y, width, height);
            //load textures
            roomfloortexture = cm.Load<Texture2D>("floortexture");
            doorhorizontaltexture = cm.Load<Texture2D>("doorhorizontal");
            doorverticaltexture = cm.Load<Texture2D>("doorvertical");
            walltexture = cm.Load<Texture2D>("walltexture");
            rooftexture = cm.Load<Texture2D>("rooftexture");
            //randomly select which side is the door in the room
            Random r = new Random();
            doorside = r.Next(1, 5);
            //generate door and walls
            wallgenerate();
            doorgenerate();
            this.roomnumber = roomnumber;
        }
        public void wallgenerate()//generate walls aroudn the room space, 2 vertical and 2 horizontal
        {
            horizontalwalls = new Rectangle[2];
            verticalwalls = new Rectangle[2];
            walls = new Rectangle[4];
            //horizontal walls  //ababa
            horizontalwall1 = new Rectangle((int)roompostition.X, (int)roompostition.Y - 50, roomspace.Width, 50);
            horizontalwalls[0] = horizontalwall1;
            walls[0] = horizontalwall1;
            horizontalwall3 = new Rectangle((int)roompostition.X, (int)(roompostition.Y + roomspace.Height), roomspace.Width, 50);
            horizontalwalls[1] = horizontalwall3;
            walls[1] = horizontalwall3;
            //vertical walls
            verticalwall4 = new Rectangle((int)(roompostition.X - 50), (int)(roompostition.Y-50),50,roomspace.Height+100);
            verticalwalls[0] = verticalwall4;
            walls[2] = verticalwall4;
            verticalwall2 = new Rectangle((int)(roompostition.X + roomspace.Width), (int)(roompostition.Y-50), 50, roomspace.Height +100);
            verticalwalls[1] = verticalwall2;
            walls[3] = verticalwall2;
        }
        public void doorgenerate()//randomly place the door on one of the wall
        {
            switch (doorside)
            {
                case 1://door on wall 1
                    door =  new Rectangle((int)(roompostition.X + (roomspace.Width / 2) - 100), (int)(roompostition.Y - 50), 200, 50);
                    break;
                case 2://door on wall 2
                    door = new Rectangle((int)(roompostition.X + roomspace.Width), (int)(roompostition.Y + (roomspace.Height / 2) - 100), 50, 200);
                    break;
                case 3: // door on wall3
                    door = new Rectangle((int)(roompostition.X + (roomspace.Width / 2) - 100), (int)(roompostition.Y + roomspace.Height), 200, 50);
                    break;
                case 4: //door on wall4
                    door = new Rectangle((int)(roompostition.X - 50),(int)(roompostition.Y+(roomspace.Height/2)-100),50,200);
                    break;
            }
        }
        public void draw(SpriteBatch sb, bool isplayerinroom)
        {
            if(isplayerinroom)
            {
                sb.Draw(roomfloortexture, roompostition, roomspace, Color.White);//draw the floor
            }
            else
            {
                sb.Draw(rooftexture, roompostition, roomspace, Color.White); // only draw roof when player isn't in room
            }
            foreach (Rectangle r in walls)//draw the walls
            {
                sb.Draw(walltexture, new Vector2(r.X, r.Y), r, Color.White);
            }
            switch (doorside)//draw the doors
            {
                case 1://this is for the horizontal wall texture
                case 3:
                    sb.Draw(doorhorizontaltexture, new Vector2(door.X, door.Y), Color.White);
                    break;
                case 2://this is for the vertical texture
                case 4:
                    sb.Draw(doorverticaltexture, new Vector2(door.X, door.Y), Color.White);            
                    break;
            }
            
                
        }

    }

}
