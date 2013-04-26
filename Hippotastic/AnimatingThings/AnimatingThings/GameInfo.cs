using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace xnaPetGame
{
    public class GameInfo : Microsoft.Xna.Framework.GameComponent
    {
        Game1 game;

        public GameInfo(Game1 g) : base(g)
        {
            game = (Game1)g;
        }

        //writes game score, pet, hunger and happiness levels to file
        public void saveFile()
        {
            //File location: \AnimatingThings\AnimatingThings\bin\x86\Debug
            using (StreamWriter siteWriter = new StreamWriter("gameInfo.txt"))
            {                
                siteWriter.WriteLine(("Score:" + game.score).TrimEnd());
                siteWriter.WriteLine(("Pet:" + game.pet).TrimEnd());
                siteWriter.WriteLine(("Hunger:" + game.hunger).TrimEnd());
                siteWriter.WriteLine(("Happiness:" + game.happiness).TrimEnd());                
            }
        }

        //reads in the saved game score, pet, hunger and happiness levels
        public void getFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader("gameInfo.txt"))
                {
                    char[] newline = new char[] { '\n' };
                    char[] c = new char[] { ':' };

                    String file = sr.ReadToEnd(); //reads in file
                    String[] lines = file.Split(newline);//splits file by lines

                    for (int i = 0; i < lines.Length; i++)
                    {
                        String[] info = lines[i].Split(c);//splits each line by colon                        
                        if (info[0] == "Score")
                        {
                            game.score = Convert.ToInt32(info[1]);
                        }
                        else if (info[0] == "Pet")
                        {
                            game.pet = info[1];
                        }
                        else if (info[0] == "Hunger")
                        {
                            game.hunger = Convert.ToInt32(info[1]);
                        }
                        else if (info[0] == "Happiness")
                        {
                            game.happiness = Convert.ToInt32(info[1]);
                        }                                               
                    }                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }
}
