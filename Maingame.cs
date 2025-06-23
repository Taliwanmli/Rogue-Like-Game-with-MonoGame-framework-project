using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using prototype.enemies;
using prototype.GameState;
using prototype.GameState.States;
using prototype.Utilities_classes;
using System.Collections.Generic;

namespace prototype
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphicsdevicemanager;
        private SpriteBatch spritebatch;

        Menu menu;
        GameStates gameState;

        public Game1()
        {
            graphicsdevicemanager = new GraphicsDeviceManager(this);
            graphicsdevicemanager.PreferredBackBufferWidth = 1920;
            graphicsdevicemanager.PreferredBackBufferHeight = 1080;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            spritebatch = new SpriteBatch(GraphicsDevice);
            menu = new Menu(Content, spritebatch, graphicsdevicemanager);//create a menu
            
            gameState = new GameStates(menu);//then push this to game state
            gameState.peek().initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            menu.load(Content);
        }

        protected override void Update(GameTime gametime)
        {
            gameState.peek().update(gametime, gameState);
            base.Update(gametime);
        }

        protected override void Draw(GameTime gametime)
        {
            GraphicsDevice.Clear(Color.Black);
            var currentstate = gameState.peek();
            if (currentstate is Menu)
            {
                spritebatch.Begin();
                gameState.peek().draw(gametime);
                spritebatch.End();
                base.Draw(gametime);
            }
            else if(currentstate is gamestartstate) 
            {
                //draw the game world
                spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, gamestartstate.camera.transformationmat);
                gameState.peek().draw(gametime);
                spritebatch.End();
                //draw the UI
                spritebatch.Begin();
                gameState.peek().drawUI();
                spritebatch.End();
                base.Draw(gametime);  
            }
            else if(currentstate is gameend)
            {
                spritebatch.Begin();
                gameState.peek().draw(gametime); spritebatch.End();
            }
            
            
        }
    }
}