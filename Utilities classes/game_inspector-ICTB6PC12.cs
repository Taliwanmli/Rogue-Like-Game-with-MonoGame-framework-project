using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using prototype.enemies;
using prototype.GameState;
using prototype.GameState.States;
using prototype.Map;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prototype
{
    internal class game_inspector
    {
        public List<Vector2> attackeffectpositionlist = new List<Vector2>();  //to draw fireball disappearing effect
        //timers
        double temptimer_damage;
        double temptimer_enemyspawn;
        double temptimer_specialenemyspawn;
        double temptimer_difficulties;
        //esssiential game field
        public static SpriteFont totalgametimerfont;
        public static TimeSpan totalgametimer;
        public Vector2 playerpositionrecord;
        public Vector2 playercurrentpositionrecord;
        //map
        Mapgeneration map;
        string mapname;
        //field for generating enemy 
        public Queue<Vector2> enemygenration = new Queue<Vector2>();
        Random spawnenemyrandom = new Random();
        bool oneminpass = false;
        int difficulties = 2;
        double adjustdifficulties;
        //player record
        int enemykilled = 0;
        int damagecaused = 0;
        gameend gameend;
        public game_inspector(Mapgeneration map, string mapname)//initial spawn of enemy
        {
            this.map = map;
            for (int i = 0; i < 25; i++) // push 25 position into a queue that is not intersecting any walls, and enemy will spawn at these positions
            {
                Rectangle temprectangle = new Rectangle(spawnenemyrandom.Next(0, 7200), spawnenemyrandom.Next(0, 4000), 50, 50);
                while (isitintersectingwalls(temprectangle, "rooms"))
                {
                    temprectangle = new Rectangle(spawnenemyrandom.Next(0, 7200), spawnenemyrandom.Next(0, 4000), 50, 50);
                }
                enemygenration.Enqueue(new Vector2(temprectangle.X, temprectangle.Y));
                
            }

            this.mapname = mapname;
        }
        public void checkhealth(List<enemy> enemies,player player,SpriteBatch sb,ContentManager cm) //remove 0 hp enemy
        {
            if(player.Health <= 0)
            {
                gameend = new gameend(totalgametimer, enemykilled, damagecaused, sb);
                gameend.load(cm);
                GameStates.states.Push(gameend);
            }
            for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i].health <= 0) 
                {
                    player.money += enemies[i].coindrop;
                    enemykilled++;  //record enemy killed
                    enemies.RemoveAt(i);
                }
            }
        }

        public void removeuselessattack(List<attack> attacks,SpriteBatch spriteBatch,string charactername) //remove attack when it reach screen edge
        {
            for(int i =0; i < attacks.Count; i++)
            {
                if (attacks[i].screenedgereached)
                {
                    attackeffectpositionlist.Add(attacks[i].position);
                    attacks.RemoveAt(i);
                }
            }
        }

        public void attackhitcontrol(List<enemy> enemies,List<attack> attacks, player player, GameTime gametime)   //collision detection, check for any damage
        {
            foreach (enemy e in enemies)
            {
                for(int i =0; i<attacks.Count; i++)//check attack hit enemy
                {
                    if (attacks[i].fireballhitbox.Intersects(e.Flyingpurplehitbox) && !attacks[i].enemyattack)
                    {
                        e.health -= attacks[i].Attackdamage;
                        damagecaused += attacks[i].Attackdamage; //record player's damage
                        attackeffectpositionlist.Add(e.position);
                        attacks.RemoveAt(i);
                    }
                    if(i< attacks.Count&&attacks[i].fireballhitbox.Intersects(player.playerhitbox) && attacks[i].enemyattack)
                    {
                        player.Health -= attacks[i].Attackdamage;
                        attackeffectpositionlist.Add(e.position);
                        attacks.RemoveAt(i);
                    }
                }
                if(e.Flyingpurplehitbox.Intersects(player.playerhitbox)&& temptimer_damage >= 0.1)//check enemy hit hero(also limit the speed of attack, not decreasing health every frame)
                {
                    player.Health -= e.damage;
                    temptimer_damage = 0;
                }
            }
            temptimer_damage += gametime.ElapsedGameTime.TotalSeconds;
        }

        public void attackspeedcontrol(GameTime gameTime,player player,List<attack> fireballs, attack fireball)  //control attacking speed 
        {
            if (player.attacktimer >= player.Attackspeedlimit) { player.Speedlimitreach = false; }
            if (player.Speedlimitreach == false)
            {
                fireballs.Add(fireball);
                player.Speedlimitreach = true;
                player.attacktimer = 0;
            }
            else { player.attacktimer += (float)gameTime.ElapsedGameTime.TotalSeconds; }
        }

        public void draw(SpriteBatch spriteBatch,GameTime gametime)   //draw the attack hit effect
        {
            for (int i = 0; i < attackeffectpositionlist.Count; i++) 
            {
                spriteBatch.Draw(attack.fireballeffect, attackeffectpositionlist[i], Color.White);
                attackeffectpositionlist.RemoveAt(i); 
            }
        }

        public void gametimerupdate(GameTime gametime,Vector2 position) //update all the timers, and adjust the difficulties
        {
            temptimer_enemyspawn += gametime.ElapsedGameTime.TotalSeconds;
            temptimer_difficulties += gametime.ElapsedGameTime.TotalSeconds;
            temptimer_specialenemyspawn += gametime.ElapsedGameTime.TotalSeconds;  
            totalgametimer += gametime.ElapsedGameTime;
            adjustdifficulties += gametime.ElapsedGameTime.TotalSeconds;
            playercurrentpositionrecord = position;

            //start to spawn special enemy after 1min
            if (totalgametimer.TotalMinutes >= 1) //after 1 mins, special enemy starts to spawn
            {
                if (temptimer_specialenemyspawn >= 4) //new position for enemy generation gets push into the queue every 4 seconds
                {
                    temptimer_specialenemyspawn = 0;
                    for (int j = 0; j < difficulties +2; j++) //spawn random amount of enemies and not within any rooms.
                    {
                        Rectangle temprectangle = new Rectangle(spawnenemyrandom.Next(0, 7200), spawnenemyrandom.Next(0, 4000), 50, 50);
                        while (isitintersectingwalls(temprectangle, "rooms"))
                        {
                            temprectangle = new Rectangle(spawnenemyrandom.Next(0, 7200), spawnenemyrandom.Next(0, 4000), 80, 80);
                        }
                        enemygenration.Enqueue(new Vector2(temprectangle.X, temprectangle.Y));
                    }
                }
            }
        }


        public void enemyspawn(List<enemy> enemies) //spawn enemies according to time and difficulties
        {
            if(totalgametimer.TotalMinutes > 1)// when game pass 1 minitue
            {
                if(adjustdifficulties>= 30) // every 30 seconds increase the difficulties
                {
                    updateenemystats();
                    adjustdifficulties = 0;
                    difficulties++;
                }
                if(temptimer_enemyspawn >= 5)//spawn enemy 5 seconds
                {
                    temptimer_enemyspawn = 0;
                    for (int i = 0; i < difficulties; i++)
                    {
                        Vector2 tempposition = enemygenration.Dequeue();
                        Random r = new Random();
                        if(r.Next(2) == 0)
                        {
                            enemies.Add(new enemy((int)tempposition.X, (int)tempposition.Y, enemydictionary.enemynames[spawnenemyrandom.Next(0,4)])); //use dequeue enemy when spawn
                        }
                        else
                        {
                            switch (mapname)
                            {
                                case "background":
                                    enemies.Add(new enemy((int)tempposition.X, (int)tempposition.Y, enemydictionary.enemynames[spawnenemyrandom.Next(4,6)]));
                                    break;
                                case "cave":
                                    enemies.Add(new enemy((int)tempposition.X, (int)tempposition.Y, enemydictionary.enemynames[spawnenemyrandom.Next(6, 8)]));
                                    break;
                                case "town":
                                    enemies.Add(new enemy((int)tempposition.X, (int)tempposition.Y, enemydictionary.enemynames[spawnenemyrandom.Next(8, 10)]));
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (temptimer_enemyspawn >= 5)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        Vector2 tempposition = enemygenration.Dequeue();
                        enemies.Add(new enemy((int)tempposition.X, (int)tempposition.Y, enemydictionary.enemynames[spawnenemyrandom.Next(difficulties - 2, difficulties)])); //use dequeue enemy when spawn
                    }
                    temptimer_enemyspawn = 0;
                }
            }
        }

        public bool allowtopass(Rectangle characterspace,Vector2 charactervelocity)//to stop any chracters walk through a wall
        {                       //current position, the velocity vector, the rectangle of the character space, the current map object
            for(int i = 0; i < map.rooms.Count; i++)//this is essetially every room in map
            {
                if (charactervelocity.Y == 0)// if player is wallking horizontal, is not going to intersects horizontal walls
                {
                    foreach(Rectangle wall in map.rooms[i].verticalwalls)
                    { //while player intersecting walls but not intersecting doors
                        while (characterspace.Intersects(wall) && (!characterspace.Intersects(map.rooms[i].door)))//this is to locate a specific wall in the map player met
                        {
                            Vector2 temp = new Vector2((int)characterspace.X, (int)characterspace.Y) + charactervelocity;
                            Rectangle futureposition = new Rectangle((int)temp.X, (int)temp.Y, characterspace.Width, characterspace.Height);
                            if (futureposition.Intersects(wall))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    for(int j = 0; j < 4; j++)
                    {
                        while (characterspace.Intersects(map.rooms[i].walls[j]) && (!characterspace.Intersects(map.rooms[i].door)))//this is to locate a specific wall in the map player met
                        {
                            Vector2 temp = new Vector2((int)characterspace.X, (int)characterspace.Y) + charactervelocity;
                            Rectangle futureposition = new Rectangle((int)temp.X, (int)temp.Y, characterspace.Width, characterspace.Height);
                            if (futureposition.Intersects(map.rooms[i].walls[j]))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    } 
                }
            }
            return true;
        }

        public bool isitintersectingwalls(Rectangle checkingpace, string walltype) //general function that check for intersection of walls
        {
            if(walltype == "verticle")
            {
                foreach(Rooms r in map.rooms)
                {
                    for(int i = 0; i < 2; i++)
                    {
                        if (checkingpace.Intersects(r.verticalwalls[i]))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else if (walltype == "horizontal")
            {
                foreach (Rooms r in map.rooms)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (checkingpace.Intersects(r.horizontalwalls[i]))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else if (walltype == "all")
            {
                foreach (Rooms r in map.rooms)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (checkingpace.Intersects(r.walls[i]))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else if(walltype == "rooms")
            {
                foreach(Rooms r in map.rooms)
                {
                    Rectangle temproomspace = new Rectangle((int)r.roompostition.X- 50,(int)r.roompostition.Y -50,r.roomspace.Width + 100,r.roomspace.Height + 100);
                    if (temproomspace.Intersects(checkingpace))
                    {
                        return true;
                    }    
                }
                return false;
            }
            return true;
        }
        void updateenemystats() //increase the health, damage, and coin drop from the enemy
        {
            foreach(var key in enemydictionary.enemystats.Keys.ToList())
            {
                var stats = enemydictionary.enemystats[key];
                stats[0] = Convert.ToString(Convert.ToInt16(stats[0])+2); //HEALTH +2
                stats[1] = Convert.ToString(Convert.ToInt16(stats[1])+1); // DAMAGE +1
                stats[5] = Convert.ToString(Convert.ToInt16(stats[5])+5); // COIN + 5
            }
        }
    }
}
