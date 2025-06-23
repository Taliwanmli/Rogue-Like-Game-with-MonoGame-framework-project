    using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace prototype.enemies
{
    internal class enemydictionary
    {
        //appearance
        public static Texture2D flyingpurple;
        public static Texture2D purplemushroom;
        public static Texture2D wizard;
        public static Texture2D nianmonster;
        public static Texture2D forest1;
        public static Texture2D forest2;
        public static Texture2D cave1;
        public static Texture2D cave2;
        public static Texture2D town1;
        public static Texture2D town2;

        //load content
        public static void enemycontentload(ContentManager Content)
        {
            flyingpurple = Content.Load<Texture2D>("enemy");
            purplemushroom = Content.Load<Texture2D>("purplemashroom");
            wizard = Content.Load<Texture2D>("wizard");
            nianmonster = Content.Load<Texture2D>("nianmonster");
            forest1 = Content.Load<Texture2D>("forest1");
            forest2 = Content.Load<Texture2D>("forest2");
            cave1 = Content.Load<Texture2D>("cave1");
            cave2 = Content.Load<Texture2D>("cave2");
            town1 = Content.Load<Texture2D>("town1");
            town2 = Content.Load<Texture2D>("town2");

        }

        //list of their name
        public static List<string> enemynames = new List<string> { "flyingpurple", "purplemushroom", "wizard", "nianmonster", "forest1", "forest2", "cave1", "cave2", "town1", "town2" };
        //enemy database
        public static Dictionary<string, string[]> enemystats = new Dictionary<string, string[]>()
        {
            // structure: health, damage,speed, apearance,special,coint drop + throw attack damage
            {"flyingpurple", new string[] {"1","1","0.3","enemy","false","1"} },
            {"purplemushroom", new string[] {"3","1","0.5","purplemushroom","false","100"} },
            {"wizard", new string[] {"2","2", "1.5625", "wizard","false","2"} },
            {"nianmonster",new string[] {"3", "2", "1.5625", "nianmonster","false","2"} },
            {"forest1" , new string[] {"5","3", "3", "forest1", "true","5","6"} },
            {"forest2" , new string[] {"5","3", "3", "forest2", "true","5","6"} },
            {"cave1" , new string[] {"5","3", "3", "cave1", "true","5","6"} },
            {"cave2" , new string[] {"5","3", "3", "cave2", "true","5","6"} },
            {"town1" , new string[] {"5","3", "3", "town1", "true","5","6"} },
            {"town2" , new string[] {"5","3", "3", "town2", "true","5","6"} },
        };

    }
}
