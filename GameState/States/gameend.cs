using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prototype.GameState.States
{
    internal class gameend : Staterecipe
    {
        Texture2D background;
        SpriteFont spriteFont;
        //player stats
        TimeSpan totaltimelived;
        int enemykilled;
        int damagecaused;
        //fields that needs to be inherit from maingame class
        SpriteBatch spritebatch;
        public gameend(TimeSpan totaltimelived, int enemykilled,int damagecaused,SpriteBatch sb)
        {
            this.totaltimelived = totaltimelived;
            this.enemykilled = enemykilled;
            this.damagecaused = damagecaused;
            spritebatch = sb;   
        }
        public override void initialize()
        {
        }
        public override void load(ContentManager cm)
        {
            background = cm.Load<Texture2D>("endpage");
            spriteFont = cm.Load<SpriteFont>("font");
        }
        public override void update(GameTime gametime, GameStates gameStates)
        {
            
        }
        public override void draw(GameTime gametime)
        {
            spritebatch.Draw(background, Vector2.Zero,Color.White);
            spritebatch.DrawString(spriteFont, totaltimelived.ToString(@"mm\:ss"), new Vector2(400, 800), Color.Red, 0f, Vector2.Zero, 2, SpriteEffects.None, 0f);
            spritebatch.DrawString(spriteFont, enemykilled.ToString(), new Vector2(1000, 800), Color.Red, 0f, Vector2.Zero, 2, SpriteEffects.None, 0f);
            spritebatch.DrawString(spriteFont, damagecaused.ToString(), new Vector2(1600, 800), Color.Red, 0f, Vector2.Zero, 2, SpriteEffects.None, 0f);
        }
        public override void drawUI()
        {
            throw new NotImplementedException();
        }
    }
    
}
