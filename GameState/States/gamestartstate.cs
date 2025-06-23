using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using prototype.enemies;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using prototype.Utilities_classes;
using prototype.Map;
using prototype.enemies.enemy_treversal_algorithm;

namespace prototype.GameState.States
{
    internal class gamestartstate : Staterecipe //inheritance
    {
        //objects used in game
        player player;
        enemy enemy;
        public static Camera camera;
        attack attack;
        List<enemy> enemies = new List<enemy>();
        List<attack> attacks = new List<attack>();
        Mapgeneration map;
        Gridsystem gridsystem;
        //utilities fileds
        mousedetection mouse;
        game_inspector game_inspector;
        Texture2D background;
        //selections from menu
        string[] choices = new string[2]; //(map,character)
        //fields that needs to be inherit from maingame class
        GraphicsDeviceManager graphicsdevicemanager;
        SpriteBatch spritebatch; 
        ContentManager Content;

        public gamestartstate(GraphicsDeviceManager g, ContentManager C, SpriteBatch s,string mapchoice, string characterchoice)
        {
            this.graphicsdevicemanager = g;
            this.Content = C;
            this.spritebatch = s;
            choices[0] = mapchoice;
            choices[1] = characterchoice;
        }


        public override void initialize()
        {
            player = new player(3840, 2160, choices[1]); //player intialise
            player.screenedge = new Vector2(7680,4320);
            map = new Mapgeneration(graphicsdevicemanager, Content, spritebatch);//map initialize
            mouse = new mousedetection();  //mousedetection intialise
            game_inspector = new game_inspector(map, choices[0]);  //game inspector intialise
            game_inspector.playerpositionrecord = player.position;
            camera = new Camera();// camera intialize
            gridsystem = new Gridsystem(map);

        }

        public override void load(ContentManager cm)
        {
            //player image
            player.hero = Content.Load<Texture2D>(choices[1]);
            player.healthbar = Content.Load<Texture2D>("healthbar");
            player.moneyfont = Content.Load<SpriteFont>("font");
            //enemy image
            enemydictionary.enemycontentload(Content);
            //fireball image
            switch (choices[1])
            {
                case "hero":
                    attack.fireball = Content.Load<Texture2D>("fireball");
                    break;
                case "swordhero":
                    attack.fireball = Content.Load<Texture2D>("swordattackeffect");
                    break;
                case "gunhero":
                    attack.fireball = Content.Load<Texture2D>("gunheroattack");
                    break;
            }
            attack.fireballeffect = Content.Load<Texture2D>("fireballeffect");
            background = Content.Load<Texture2D>(choices[0]);
            //chesttexture
            chest.chesttexture = Content.Load<Texture2D>("chest");
            //fonts
            game_inspector.totalgametimerfont = Content.Load<SpriteFont>("font");
            chest.chestfont = Content.Load<SpriteFont>("font");
        }
        public override void update(GameTime gametime, GameStates gameStates)
        {
            //total game time update,and record player's position
            game_inspector.gametimerupdate(gametime,player.position);

            //player update
            if (game_inspector.allowtopass(player.playerhitbox,player.positionupdate()))
            {
                player.Update(gametime,map);
            }

            //camera update
            camera.update(player);

            //enemy update
            game_inspector.enemyspawn(enemies); //(enemy spawn)

            for (int i = 0; i < enemies.Count(); i++)
            {  //stop enemy going through walls
                if(!enemies[i].getdirections(gridsystem, player.position,player.playerhitbox,player.isinaroom,attacks,Content))
                {
                    enemies.RemoveAt(i);
                }
            }
            foreach(enemy e in enemies)
            {
                e.enemymotionupdate(gametime);
            }
            game_inspector.checkhealth(enemies,player,spritebatch,Content); //(remove dead enemy)

            //fireballs update
            mouse.mouseactivityupdate();
            if (mouse.leftbuttonpressed) //(check mouse input to add attack, and attack speed control)
            {
                attack fireball = new attack(player.position, mouse.leftclickedposition, new Vector2(7680, 4320), choices[1]);
                game_inspector.attackspeedcontrol(gametime, player, attacks, fireball); //(add fireballs and controll attacking speed)
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) //if esc pressed back to menu
            {
                gameStates.Pop();
            }
            
            foreach (attack fireball in attacks)
            {
                    fireball.update(game_inspector,player);
            }
            //chests update
            map.update(player, attacks, enemies);
            //(remove fireball hits wall)
            game_inspector.removeuselessattack(attacks, spritebatch, choices[1]);

            //check for hits
            game_inspector.attackhitcontrol(enemies, attacks, player, gametime);

        }
        public override void draw(GameTime gametime)
        {
            if (!player.isinaroom)
            {
                spritebatch.Draw(background, Vector2.Zero, Color.White);
                //player draw
                player.Draw(spritebatch);

                //enemy draw
                foreach (enemy e in enemies)
                {
                    e.Draw(spritebatch);
                }

                //attack draw
                foreach (attack fireball in attacks)
                {
                    fireball.draw(spritebatch);
                }

                // game_insepctor draw
                game_inspector.draw(spritebatch, gametime);
                //map draw
                map.draw(player.isinaroom, player.roomnumberin);
            }
            else
            {
                map.draw(player.isinaroom, player.roomnumberin);
                player.Draw(spritebatch);
            }
        }
        public override void drawUI()
        {
            //draw the player health bar
            player.drawUI(spritebatch);
            map.drawui();
            //game timer draw
            spritebatch.DrawString(game_inspector.totalgametimerfont, game_inspector.totalgametimer.ToString(@"mm\:ss"), new Vector2(1800, 0), Color.Yellow, 0f, Vector2.Zero, 2, SpriteEffects.None, 0f);
        }
    }
}
