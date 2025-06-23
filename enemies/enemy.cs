using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using prototype.enemies.enemy_treversal_algorithm;
using prototype.GameState.States;
using System;
using System.Collections.Generic;

namespace prototype.enemies
{
    internal class enemy
    {
        public Texture2D enemytexture;

        // enemy motion fields
        public Vector2 position;
        public Vector2 velocity;
        double speed;
        double speedcontrol = 0;
        Stack<Vector2> directions = new Stack<Vector2>();
        //enemy stats
        public int health;
        public double damage;
        Astar_algorithm astaralgorithm = new Astar_algorithm();
        public int coindrop;
        //enemy hitbox
        private Rectangle enmeyhitbox;
        public Rectangle Flyingpurplehitbox
        {
            get { return enmeyhitbox; }
            set { enmeyhitbox = value; }
        }
        //for traversal
        double timefromlasttraversal = 0;
        double traversecontrol;
        public bool ancientice = false;
        double ancienticetimer;
        //special enemy
        public bool specialenemy;
        string charactername;

        public enemy(int x, int y, string enemyname)//get all the stats for an enemy
        {
            position = new Vector2(x, y);
            health = Convert.ToInt32(enemydictionary.enemystats[enemyname][0]);
            damage = Convert.ToDouble(enemydictionary.enemystats[enemyname][1]);
            speed = double.Parse(enemydictionary.enemystats[enemyname][2]);
            coindrop = Convert.ToInt16(enemydictionary.enemystats[enemyname][5]);
            specialenemy = (enemydictionary.enemystats[enemyname][4].Length == 4);
            string temp_name = enemydictionary.enemystats[enemyname][3];
            switch (temp_name)
            {
                case "enemy":
                    enemytexture = enemydictionary.flyingpurple;
                    enmeyhitbox = new Rectangle((int)position.X, (int)position.Y, enemytexture.Width, enemytexture.Height);
                    break;
                case "purplemushroom":
                    enemytexture = enemydictionary.purplemushroom;
                    enmeyhitbox = new Rectangle((int)position.X, (int)position.Y, enemytexture.Width, enemytexture.Height);
                    break;
                case "wizard":
                    enemytexture = enemydictionary.wizard;
                    enmeyhitbox = new Rectangle((int)position.X, (int)position.Y, enemytexture.Width, enemytexture.Height);
                    break;
                case "nianmonster":
                    enemytexture = enemydictionary.nianmonster;
                    enmeyhitbox = new Rectangle((int)position.X, (int)position.Y, enemytexture.Width, enemytexture.Height);
                    break;
                case "forest1":
                    charactername = "forestattack";
                    enemytexture = enemydictionary.forest1;
                    enmeyhitbox = new Rectangle((int)position.X, (int)position.Y, enemytexture.Width, enemytexture.Height);
                    break;
                case "forest2":
                    charactername = "forestattack";
                    enemytexture = enemydictionary.forest2;
                    enmeyhitbox = new Rectangle((int)position.X, (int)position.Y, enemytexture.Width, enemytexture.Height);
                    break;
                case "town1":
                    charactername = "townattack";
                    enemytexture = enemydictionary.town1;
                    enmeyhitbox = new Rectangle((int)position.X, (int)position.Y, enemytexture.Width, enemytexture.Height);
                    break;
                case "town2":
                    charactername = "townattack";
                    enemytexture = enemydictionary.town2;
                    enmeyhitbox = new Rectangle((int)position.X, (int)position.Y, enemytexture.Width, enemytexture.Height);
                    break;
                case "cave1":
                    charactername = "caveattack";
                    enemytexture = enemydictionary.cave1;
                    enmeyhitbox = new Rectangle((int)position.X, (int)position.Y, enemytexture.Width, enemytexture.Height);
                    break;
                case "cave2":
                    charactername = "caveattack";
                    enemytexture = enemydictionary.cave2;
                    enmeyhitbox = new Rectangle((int)position.X, (int)position.Y, enemytexture.Width, enemytexture.Height);
                    break;
            }
        }
        public bool getdirections(Gridsystem grid,Vector2 playerposition,Rectangle playerhitbox,bool isitinroom, List<attack> attacks,ContentManager cm)
        {
            playerposition = new Vector2(playerposition.X + 50, playerposition.Y + 50);
            double modulusdistancetoplayer = Math.Sqrt(Math.Pow(playerposition.X-position.X, 2) + Math.Pow(playerposition.Y- position.Y, 2));
            if(modulusdistancetoplayer > 2000) //this is how progrma adjust the frequency of calling path finding method base on distance between player and enemy
            {
                return true;
            }
            else if(modulusdistancetoplayer > 1000 && modulusdistancetoplayer < 2000) { traversecontrol = 2; }
            else if(modulusdistancetoplayer > 500 && modulusdistancetoplayer < 1000) { traversecontrol = 1.5; }
            else if( modulusdistancetoplayer < 500) { traversecontrol = 1; }
            if(timefromlasttraversal >= traversecontrol && !enmeyhitbox.Intersects(playerhitbox) && !isitinroom)
            {// these conditions are, traverse cool down finish, enemy isn't already on player, player is not in room
                if (specialenemy) { attacks.Add(new attack(true, position, playerposition, new Vector2(7680, 4320), cm, charactername)); } //allow special enemy to throw attacks
                timefromlasttraversal = 0;
                directions = new Stack<Vector2>();
                directions = astaralgorithm.traversal(position, playerposition, grid.Getemptygrid());
                if (astaralgorithm.spawnfail == true) { return false; }
            }
            return true;
        }
        public void enemymotionupdate(GameTime gametime)//update enemy's velocity
        {
            if(timefromlasttraversal < 4) //just to make sure some enemy arene't met during the whole game play, so this time won't get constantly updated, takes up too much memory space
            {
                timefromlasttraversal += gametime.ElapsedGameTime.TotalSeconds;
            }
            speedcontrol += gametime.ElapsedGameTime.TotalSeconds;
            if (ancientice) //this is the timer for a item ancient ice
            {
                ancienticetimer += gametime.ElapsedGameTime.TotalSeconds;
                if(ancienticetimer > 8)
                {
                    ancientice = false;
                }
            }
            if(speedcontrol >= speed)
            {
                if(directions != null)
                {
                    if (directions.Count != 0 && !ancientice)
                    {
                        position = directions.Pop();
                        speedcontrol = 0;
                        enmeyhitbox = new Rectangle((int)position.X, (int)position.Y, enemytexture.Width, enemytexture.Height);
                    }
                }
            }
        }

        public void Update()//update enemy's position
        {
            
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(enemytexture, position, Color.White);
        }
        
    }
}
