using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace prototype.Map
{
    internal class chest
    {
        //information for a chest
        Vector2 chestposition;
        public static Texture2D chesttexture;
        public static SpriteFont chestfont;
        public int chestvalue;
        public Rectangle chestspace;
        public int roomnumberin;
        public string itemname;
        public chest(Vector2 chestposition,int roomnumber, string itemname)
        {
            this.chestposition = chestposition;
            Random r = new Random();
            chestvalue = r.Next(10, 20); //randomly generate chest value
            chestspace = new Rectangle((int)chestposition.X, (int)chestposition.Y, 80, 80);
            roomnumberin = roomnumber;
            this.itemname = itemname;
        }
        public void draw(SpriteBatch sb)
        {
            sb.Draw(chesttexture,chestposition,Color.White);
            sb.DrawString(chestfont, chestvalue.ToString(), new Vector2(chestposition.X - 100, chestposition.Y - 130), Color.Cyan, 0f, Vector2.Zero, 3, SpriteEffects.None, 0f);
            sb.DrawString(chestfont, itemname, new Vector2(chestposition.X-100,chestposition.Y-80), Color.Red, 0f, Vector2.Zero, 3, SpriteEffects.None, 0f);
        }
    }
}
