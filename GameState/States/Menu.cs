using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using prototype.Utilities_classes;

namespace prototype.GameState.States
{
    internal class Menu : Staterecipe //inheritance
    {
        //classes(buttons)
        List<Button> buttonspage1 = new List<Button>();
        List<Button> buttonspage2 = new List<Button>();
        Texture2D menubackground;
        Texture2D page1background;
        Texture2D page2background;
        //fields that needs to be inherit from maingame class
        SpriteBatch sb;
        ContentManager cm;
        GraphicsDeviceManager gm;
        //menu flow
        int currentpage = 1;
        //the maingame state
        gamestartstate gamestartstate;
        string mapchoice;
        string characterchoice;
        public Menu(ContentManager C, SpriteBatch s, GraphicsDeviceManager gm)
        {
            sb = s;
            cm = C;
            this.gm = gm;
        }

        public override void initialize()
        {
            //add buttons here
            buttonspage1.Add(new Button("startbutton", new Vector2(800, 700), new Rectangle(800, 700, 200, 100)));
            //overloading here, and index for buton is required in page 2
            buttonspage2.Add(new Button("choosebutton", new Vector2(300, 700), new Rectangle(300, 700, 200, 100),1));
            buttonspage2.Add(new Button("choosebutton", new Vector2(900, 700), new Rectangle(900, 700, 200, 100),2));
            buttonspage2.Add(new Button("choosebutton", new Vector2(1500, 700), new Rectangle(1500, 700, 200, 100),3));
        }
        public override void load(ContentManager notwanted)
        {
            foreach (Button b in buttonspage1)
            {
                b.load(cm);
            }
            foreach (Button b in buttonspage2)
            {
                b.load(cm);
            }
            menubackground = cm.Load<Texture2D>("menubackground");
            page1background = cm.Load<Texture2D>("page1background");
            page2background = cm.Load<Texture2D>("page3background");
        }
        public override void update(GameTime gametime, GameStates gameStates)
        {
            switch (currentpage)
            {
                case 1:
                    foreach (Button b in buttonspage1)
                    {
                        if(b.update())
                        {
                            currentpage = 2;
                        }
                    }
                    break;
                case 2:
                    foreach(Button b in buttonspage2)
                    {
                        if (b.update())
                        {
                            switch(b.buttonindex)
                            {
                                case 1:
                                    mapchoice = "background";
                                    currentpage = 3;
                                    break;
                                case 2:
                                    mapchoice = "cave";
                                    currentpage = 3;
                                    break;
                                case 3:
                                    mapchoice = "town";
                                    currentpage = 3;
                                    break;
                            }
                        }
                    }
                    break;
                case 3: // I can use the buttons in page2 hence it's also 3 choices
                    foreach (Button b in buttonspage2)
                    {
                        if (b.update())
                        {
                            switch (b.buttonindex)
                            {
                                case 1:
                                    characterchoice = "hero";
                                    gamestartstate = new gamestartstate(gm, cm, sb,mapchoice,characterchoice);
                                    gamestartstate.initialize();
                                    gamestartstate.load(cm);
                                    GameStates.states.Push(gamestartstate);
                                    break;
                                case 2:
                                    characterchoice = "swordhero";
                                    gamestartstate = new gamestartstate(gm, cm, sb, mapchoice, characterchoice);
                                    gamestartstate.initialize();
                                    gamestartstate.load(cm);
                                    GameStates.states.Push(gamestartstate);
                                    break;
                                case 3:
                                    characterchoice = "gunhero";
                                    gamestartstate = new gamestartstate(gm, cm, sb, mapchoice, characterchoice);
                                    gamestartstate.initialize();
                                    gamestartstate.load(cm);
                                    GameStates.states.Push(gamestartstate);
                                    break;
                            }
                        }
                    }
                    break;
            }
        }
        public override void draw(GameTime gametime) //display pages
        {
            switch (currentpage)
            {
                case 1:
                    sb.Draw(menubackground, Vector2.Zero, Color.White);
                    foreach (Button b in buttonspage1)
                    {
                        b.draw(sb);
                    }
                    break;
                case 2:
                    sb.Draw(page1background, Vector2.Zero, Color.White);
                    foreach (Button b in buttonspage2)
                    {
                        b.draw(sb);
                    }
                    break;
                case 3:
                    sb.Draw(page2background, Vector2.Zero, Color.White);
                    foreach (Button b in buttonspage2)
                    {
                        b.draw(sb);
                    }
                    break;
            }  
        }
        public override void drawUI()
        {

        }
    }
}
