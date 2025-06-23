using DryIoc.ImTools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using prototype.GameState.States;
using System;
using System.Runtime.InteropServices;

namespace prototype
{
    internal class attack
    {
        //appearance
        string charactername;
        public static Texture2D fireball;
        public static Texture2D fireballeffect;
        public static Texture2D specialenemyattack;

        //fireball motion fields
        public Vector2 position;
        private float speed = 10f;
        public float Speed   //speed a and m
        {
            get { return speed; }
            set { speed = value; }
        }
        public Vector2 direction;

        //attack states
        Vector2 shootingposition; 
        public Vector2 screenedge;
        public bool screenedgereached = false;
        public Rectangle fireballhitbox;  //(fireball hitbox)
        private int attackdamage;
        public static int damageadder = 0;
        public int Attackdamage
        {
            get { return attackdamage; } set { attackdamage = value; }
        }
        public bool enemyattack = false;
        //for item effect
        int widthsize = 0;
        int heightsize = 0;
        public static int sizeadder = 0;
        public static float speedadder = 0;
        public attack(Vector2 heroposition, Vector2 targetposition, Vector2 screenedge, string charactername)
        { //pass in essential field that helps generating an attack
            this.shootingposition = targetposition;
            this.charactername = charactername;
            switch (charactername)
            {
                case "hero":
                    this.speed = 10f + speedadder;
                    this.Attackdamage = 1 + damageadder;
                    widthsize = 30 + sizeadder;
                    heightsize = 30 + sizeadder;
                    break;
                case "swordhero":
                    this.speed = 15f + speedadder;
                    this.Attackdamage = 2 + damageadder;
                    widthsize = 50 + sizeadder;
                    heightsize = 50 + sizeadder;
                    break;
                case "gunhero":
                    this.speed = 30f + speedadder;
                    this.attackdamage = 4 + damageadder;
                    widthsize = 30 + sizeadder;
                    heightsize = 30 + sizeadder;
                    break;
            }
            position = heroposition;//shoot from heroposition
            //mouse position changes with camera moves, so apply a transform of mouse posstion and a invert of camera's transformation matrix to make it the actual position on map
            Vector3 mapposition = Vector3.Transform(new Vector3(shootingposition.X, shootingposition.Y, 0), Matrix.Invert(gamestartstate.camera.transformationmat));
            this.shootingposition = new Vector2(mapposition.X, mapposition.Y);
            this.screenedge = screenedge;
            // normalize to make bullet travel towards the click on the screen
            direction = shootingposition - position;
            direction = normalizevectors(direction);
        }
        //overload
        public attack(bool specialenemy, Vector2 startingpos, Vector2 targetpos,Vector2 screenedge,ContentManager cm,string enemyname)
        {
            specialenemyattack = cm.Load<Texture2D>(enemyname); 
            speed = 12f;
            attackdamage = 6;
            widthsize = 30;
            heightsize = 30;
            enemyattack = specialenemy;
            this.screenedge = screenedge;
            position = startingpos;
            direction = targetpos - startingpos;
            direction = normalizevectors(direction);
        }

        public void update(game_inspector inspector,player player)
        {
            if (!enemyattack)
            {
                switch (charactername)
                {
                    case "hero":
                        if (position.X > 0 && position.Y > 0 && position.X < screenedge.X - fireball.Width && position.Y < screenedge.Y - fireball.Height)
                        {
                            position += direction * speed;
                            fireballhitbox = new Rectangle((int)position.X, (int)position.Y, widthsize, heightsize);
                        }
                        else
                        {
                            screenedgereached = true;
                        }
                        break;
                    case "swordhero":
                        if (inspector.isitintersectingwalls(fireballhitbox, "all"))
                        {
                            screenedgereached = true;
                        }
                        else if (position.X > 0 && position.Y > 0 && position.X < screenedge.X - fireball.Width && position.Y < screenedge.Y - fireball.Height)
                        {
                            position += direction * speed;
                            fireballhitbox = new Rectangle((int)position.X, (int)position.Y, widthsize, heightsize);
                            KeyboardState control = Keyboard.GetState();
                            if (control.IsKeyDown(Keys.E))
                            {
                                player.position = position;
                            }
                        }
                        else
                        {
                            screenedgereached = true;
                        }
                        break;
                    case "gunhero":
                        if (inspector.isitintersectingwalls(fireballhitbox, "all"))
                        {
                            screenedgereached = true;
                        }
                        else if (position.X > 0 && position.Y > 0 && position.X < screenedge.X - fireball.Width && position.Y < screenedge.Y - fireball.Height)
                        {
                            position += direction * speed;
                            fireballhitbox = new Rectangle((int)position.X, (int)position.Y, widthsize, heightsize);
                        }
                        else
                        {
                            screenedgereached = true;
                        }
                        break;
                }
            }
            else
            {
                if (inspector.isitintersectingwalls(fireballhitbox, "all"))
                {
                    screenedgereached = true;
                }
                else if (position.X > 0 && position.Y > 0 && position.X < screenedge.X - fireball.Width && position.Y < screenedge.Y - fireball.Height)
                {
                    position += direction * speed;
                    fireballhitbox = new Rectangle((int)position.X, (int)position.Y, widthsize, heightsize);
                }
                else
                {
                    screenedgereached = true;
                }
            }
           
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (!enemyattack)
            {
                spriteBatch.Draw(fireball, position, Color.White);
            }
            else
            {
                spriteBatch.Draw(specialenemyattack, position, Color.White);
            }
        }
        Vector2 normalizevectors(Vector2 directionvector) //produces a stright line motion for attack using unit vectors
        {
            float magnitude = (float)Math.Sqrt(Math.Pow(directionvector.X,2)+ Math.Pow(directionvector.Y, 2));
            if(magnitude > 0)
            {
                return new Vector2(directionvector.X/magnitude, directionvector.Y/magnitude);  
            }
            return new Vector2(0,0);
        }
    }
}
