using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc.ImTools;
using Microsoft.Xna.Framework.Input;
using prototype.GameState.States;
using prototype.enemies;

namespace prototype.Map
{
    internal class Mapgeneration
    {
        //rooms, chest and items
        public List<Rooms> rooms;
        public chest[] chests;
        public string[] items = { "powerrunes", "flyingboots", "gunpowder", "speedgear", "enlargepotion", "ancientice", "Demonforce"};
        Texture2D powerrunes;
        Texture2D flyingboots;
        Texture2D gunpowder;
        Texture2D speedgear;
        Texture2D enlargepotion;
        Texture2D ancientice;
        Texture2D Demonforce;
        mousedetection mouse = new mousedetection();
        //fields that needs to be inherit from maingame class
        SpriteBatch spritebatch; 
        ContentManager Content;
        //fileds responsible for draw
        List<Texture2D> itemtodraw = new List<Texture2D>();
        List<Vector2> itemposition = new List<Vector2>();
        int drawingposition = 500;

        public Mapgeneration(GraphicsDeviceManager g, ContentManager C, SpriteBatch s)
        {
            this.Content = C;
            this.spritebatch = s;
            //load all items texture
            load();
            //this constructor will generate a list of rooms, without overlapping each other
            rooms = new List<Rooms>();
            Random a = new Random();//number of rooms
            Random b = new Random();  //settings for each room
            Rooms temproom = new Rooms(b.Next(1000, 1500), b.Next(800, 1000), new Vector2(b.Next(200, 5980), b.Next(200, 3120)), Content,0);
            rooms.Add(temproom);
            for (int i = 0; i < a.Next(4,9); i++) //generate rooms
            {
                bool overlap = true;
                whilestart: //use of labels
                while (overlap)
                {
                    temproom = new Rooms(b.Next(1000, 1500), b.Next(800, 1000), new Vector2(b.Next(200, 5980), b.Next(200, 3120)), Content,i+1);
                    foreach (Rooms r in rooms)
                    {
                        if (temproom.roomspace.Intersects(r.roomspace))
                        {
                            goto whilestart;
                        }
                    }
                    overlap = false;
                }
                rooms.Add(temproom);
            }
            //code here is handling chest and item
            chests = new chest[rooms.Count];
            int counter = 0;
            foreach(Rooms r in rooms)
            {
                chest c = new chest(new Vector2(r.roomspace.X + r.roomspace.Width / 2, r.roomspace.Y + r.roomspace.Height / 2), r.roomnumber, items[a.Next(0, 7)]); 
                chests[counter] = c; counter++;
            }
        }
        void load()
        {
            powerrunes = Content.Load<Texture2D>("powerrunes");
            flyingboots = Content.Load<Texture2D>("flyingboots");
            gunpowder = Content.Load<Texture2D>("gunpowder");
            speedgear = Content.Load<Texture2D>("speedgear");
            enlargepotion = Content.Load<Texture2D>("enlargepotion");
            ancientice = Content.Load<Texture2D>("acientice");
            Demonforce = Content.Load<Texture2D>("demonforce");
        }
        public void update(player player, List<attack> attacks, List<enemy> enemies) //check player open a chest and add on according effect
        {
            mouse.mouseactivityupdate(true);

            if(mouse.leftbuttonpressed)
            {
                chest c;
                foreach (chest temp in chests)
                {
                    if (temp.roomnumberin == player.roomnumberin)
                    { 
                        c = temp;
                        Vector3 mapposition = Vector3.Transform(new Vector3(mouse.leftclickedposition.X, mouse.leftclickedposition.Y, 0), Matrix.Invert(gamestartstate.camera.transformationmat));
                        Vector2 clickposition = new Vector2(mapposition.X, mapposition.Y);
                        if (new Rectangle((int)clickposition.X,(int) clickposition.Y, 1, 1).Intersects(c.chestspace) && player.money >= c.chestvalue)
                        {
                            player.money -= c.chestvalue;   
                            switch (c.itemname)
                            {
                                case ("powerrunes"): //increase damage by 4
                                    attack.damageadder += 4;
                                    itemtodraw.Add(powerrunes);
                                    itemposition.Add(new Vector2(drawingposition, 0));
                                    drawingposition += 50;
                                    break;
                                case ("flyingboots"): //increases player's moving speed by 1
                                    player.Speed += 1;
                                    itemtodraw.Add(flyingboots);
                                    itemposition.Add(new Vector2(drawingposition, 0));
                                    drawingposition += 50;
                                    break;
                                case ("gunpowder"): //increases attacks flying speed of player by 1
                                    attack.speedadder += 5f;
                                    itemtodraw.Add(gunpowder);
                                    itemposition.Add(new Vector2(drawingposition, 0));
                                    drawingposition += 50;
                                    break;
                                case ("speedgear"): //increases attack frequency by 0.05 secibds
                                    player.Attackspeedlimit -= 0.05f;
                                    itemtodraw.Add(speedgear);
                                    itemposition.Add(new Vector2(drawingposition, 0));
                                    drawingposition += 50;
                                    break;
                                case ("enlargepotion"): //make the hitbox of attacks bigger
                                    attack.sizeadder += 10;  
                                    itemtodraw.Add(enlargepotion);
                                    itemposition.Add(new Vector2(drawingposition, 0));
                                    drawingposition += 50;
                                    break;
                                case ("ancientice"): //freeze enemy for 8 seconds
                                    foreach(enemy e in enemies)
                                    {
                                        e.ancientice = true;
                                    }
                                    itemtodraw.Add(ancientice);
                                    itemposition.Add(new Vector2(drawingposition, 0));
                                    drawingposition += 50;
                                    break;
                                case ("Demonforce"): // remove all enemy currently inthe game
                                    enemies.Clear();    
                                    itemtodraw.Add(Demonforce);
                                    itemposition.Add(new Vector2(drawingposition, 0));
                                    drawingposition += 50;
                                    break;
                            }
                        }
                        break;
                    }
                } 
            }   
        }
        public void draw(bool isplayerinroom,int roomnumber)
        {
            if (isplayerinroom) //only draws the room the player is currently in
            {
                rooms[roomnumber].draw(spritebatch, isplayerinroom);
                foreach(chest c in chests)
                {
                    if(c.roomnumberin == roomnumber)
                    {
                        c.draw(spritebatch);
                        break;
                    }
                }
            }
            else
            {
                foreach (Rooms r in rooms) //draw all room if player isn't in any of them
                {
                    r.draw(spritebatch, isplayerinroom);
                }
            }
            
        }
        public void drawui()
        {
            for (int i = 0; i < itemtodraw.Count(); i++)
            {
                spritebatch.Draw(itemtodraw[i], itemposition[i], Color.White);
            }
        }

    }
}
