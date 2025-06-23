using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using prototype.Map;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace prototype
{
    internal class player
    {
        //player stats field
        public string name;
        private double health; 
        public double Health//a and m
        {
            get { return health; } set { health = value; }
        }

        private float attackspeedlimit;  //(attacking speed limit fields, smaller means faster speed)
        public float attacktimer;
        public float Attackspeedlimit
        {
            get { return attackspeedlimit; }
            set { attackspeedlimit = value; }
        }
        private bool speedlimitreach = false;
        public bool Speedlimitreach { get { return speedlimitreach; } set { speedlimitreach = value; } }

        // player motion fields
        public Vector2 position; 
        public Vector2 velocity;
        private float speed;
        public float Speed   //speed a and m
        {
            get { return speed; } set { speed = value; }
        }

        //appearance
        public static Texture2D hero; 
        public static Vector2 screenedge;
        public static Texture2D healthbar;

        //mechanics field
        public Rectangle playerhitbox;

        //others
        public bool isinaroom;
        public int roomnumberin;
        public int money = 100000;
        public SpriteFont moneyfont;

        Dictionary<string, string[]> characters = new Dictionary<string, string[]>()
        {
            // structure: health, damage,speed, apearance,method(boolean)
            {"hero", new string[] {"100","0.35","8"} }, //(health,attackspeed,speed)
            {"swordhero", new string[] {"250","0.4","10"} },
            {"gunhero", new string[] {"80","0.8","7"} },

        };
        public player(float x, float y,string charactername) 
        {
            position = new Vector2(x, y);
            this.name = charactername;
            this.health = Convert.ToDouble(characters[charactername][0]);
            this.attackspeedlimit = float.Parse(characters[charactername][1]);
            this.speed = float.Parse(characters[charactername][2]);

        }   

        public Vector2 positionupdate()//the position update logic for player
        {
            KeyboardState control = Keyboard.GetState();
            //move for W and S
            if (control.IsKeyDown(Keys.W) && !(position.Y < 0))
            {
                velocity.Y = -speed;
            }
            else if (control.IsKeyDown(Keys.S) && !(position.Y >= screenedge.Y - hero.Height))
            {
                velocity.Y = speed;
            }
            else
            {
                velocity.Y = 0;
            }
            //move for A and D
            if (control.IsKeyDown(Keys.A) && !(position.X <= 0))
            {
                velocity.X = -speed;
            }
            else if (control.IsKeyDown(Keys.D) && !(position.X >= screenedge.X - hero.Width))
            {
                velocity.X = speed;
            }
            else
            {
                velocity.X = 0;
            }
            return velocity;
        }
        public void Update(GameTime gametime,Mapgeneration map)//player update
        {
            position += positionupdate();
            playerhitbox = new Rectangle((int)position.X, (int)position.Y,hero.Width,hero.Height);  
            foreach(Rooms r in map.rooms)
            {
                if (playerhitbox.Intersects(r.roomspace)) // this loop here check whether player is in a room 
                {
                    roomnumberin = r.roomnumber;
                    isinaroom = true;
                    break;
                }
                else { isinaroom = false;}
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(hero, position, Color.White);
        }
        public void drawUI(SpriteBatch spritebatch)
        {
            spritebatch.Draw(healthbar, new Vector2(0, 0), new Rectangle(0, 0, (int)health * 3, 15), Color.White);  //draw the health bar player
            string moneyText = $"Money: {money}"; 
            spritebatch.DrawString(moneyfont, moneyText, new Vector2(50,50), Color.Gold, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
        }
    }
}
