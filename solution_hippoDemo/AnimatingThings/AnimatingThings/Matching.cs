using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace xnaPetGame
{
    class Matching : Microsoft.Xna.Framework.GameComponent
    {                           
        SpriteFont sf;
        Game1 game;        
        Boolean alertF = false, balloonF = false, cameraF = false, coneF = false, diceF = false, floppyF = false, globeF = false, mammathF = false, paintF = false, treeF = false, coverF=true;
        int win_width, win_height;        
        Point select = new Point();
        Random rand = new Random();
        Boolean oneClick = false, twoClick = false;
        Texture2D one, two;
        
        //Textures for each box on board
        Texture2D tex11, tex21, tex31, tex41, tex12, tex22, tex32, tex42, tex13, tex23, tex33, tex43;
        Texture2D setTex11, setTex21, setTex31, setTex41, setTex12, setTex22, setTex32, setTex42, setTex13, setTex23, setTex33, setTex43;
        Texture2D alertTex, balloonTex, cameraTex, coneTex, diceTex, floppyTex, globeTex, mammathTex, paintTex, treeTex, coverTex;

        //rectangles used for sprite placement
        Rectangle box11 = new Rectangle(145, 90, 100, 100);
        Rectangle box21 = new Rectangle(345, 90, 100, 100);
        Rectangle box31 = new Rectangle(545, 90, 100, 100);
        Rectangle box41 = new Rectangle(745, 90, 100, 100);
        Rectangle box12 = new Rectangle(145, 250, 100, 100);
        Rectangle box22 = new Rectangle(345, 250, 100, 100);
        Rectangle box32 = new Rectangle(545, 250, 100, 100);
        Rectangle box42 = new Rectangle(745, 250, 100, 100);
        Rectangle box13 = new Rectangle(145, 400, 100, 100);
        Rectangle box23 = new Rectangle(345, 400, 100, 100);
        Rectangle box33 = new Rectangle(545, 400, 100, 100);
        Rectangle box43 = new Rectangle(745, 400, 100, 100);
       
        //rectangles used to for click detection
        Rectangle one1 = new Rectangle(45, 45, 100, 100);
        Rectangle two1 = new Rectangle(245, 45, 100, 100);
        Rectangle three1 = new Rectangle(445, 45, 100, 100);
        Rectangle four1 = new Rectangle(645, 45, 100, 100);
        Rectangle one2 = new Rectangle(45, 200, 100, 100);
        Rectangle two2 = new Rectangle(245, 200, 100, 100);
        Rectangle three2 = new Rectangle(445, 200, 100, 100);
        Rectangle four2 = new Rectangle(645, 200, 100, 100);
        Rectangle one3 = new Rectangle(45, 345, 100, 100);
        Rectangle two3 = new Rectangle(245, 345, 100, 100);
        Rectangle three3 = new Rectangle(445, 345, 100, 100);
        Rectangle four3 = new Rectangle(645, 345, 100, 100);
       
        public Matching(Game g)
            : base(g)
        {
            game = (Game1)g;

            win_width = game.Window.ClientBounds.Width;
            win_height = game.Window.ClientBounds.Height;
            
            alertTex = game.Content.Load<Texture2D>(@"MiniGameImages/Alert");
            balloonTex = game.Content.Load<Texture2D>(@"MiniGameImages/Balloonboy");
            cameraTex = game.Content.Load<Texture2D>(@"MiniGameImages/Camera");
            coneTex = game.Content.Load<Texture2D>(@"MiniGameImages/Cone");
            diceTex = game.Content.Load<Texture2D>(@"MiniGameImages/Dice");
            floppyTex = game.Content.Load<Texture2D>(@"MiniGameImages/Floppy");
            globeTex = game.Content.Load<Texture2D>(@"MiniGameImages/Globe");
            mammathTex = game.Content.Load<Texture2D>(@"MiniGameImages/Mammath2");
            paintTex = game.Content.Load<Texture2D>(@"MiniGameImages/Paint");
            treeTex = game.Content.Load<Texture2D>(@"MiniGameImages/Tree");
            coverTex = game.Content.Load<Texture2D>(@"MiniGameImages/Cover copy");

            tex11=coverTex;
            tex21=coverTex;
            tex31=coverTex;
            tex41=coverTex;
            tex12=coverTex;           
            tex22=coverTex;
            tex32=coverTex;
            tex42=coverTex;
            tex13=coverTex;
            tex23=coverTex;
            tex33 = coverTex;
            tex43 = coverTex;
           
            setBoard();
            //test = new Texture2D(game.GraphicsDevice, 1, 1);
            //test.SetData(new[] { Color.White });

            sf = game.Content.Load<SpriteFont>("SpriteFont");   
        }

         public override void Initialize()
        {           
            base.Initialize();
        }

         public void setBoard()
         {
             int[] seen = new int[12];             
             for (int i = 0; i < 12; i+=2)
             {
                 Boolean validNums = false;
                 int n1 = rand.Next(12)+1;
                 int n2 = rand.Next(12)+1;
                 while (!validNums)
                 {                     
                     Console.WriteLine("n1= " + n1 + ", n2= " + n2);
                     if (n1 != n2 && i == 0)
                     {
                         seen[i] = n1;
                         seen[i + 1] = n2;
                         validNums = true;
                     }
                     
                     if (n1 == n2 )
                     {
                         n2 = rand.Next(12) + 1;                        
                     }
                     else if (seen.Contains(n1) == true && i>0)
                     {
                         n1 = rand.Next(12)+1;
                     }
                     else if (seen.Contains(n2) == true && i > 0)
                     {
                         n2 = rand.Next(12) + 1;
                     }
                     else
                     {
                         Console.WriteLine("Chosen " + n1 + ", " + n2);
                         seen[i] = n1;
                         seen[i + 1] = n2;
                         validNums = true;
                     }
                 }
                
             }
             

         }
        public override void Update(GameTime gameTime)
        {            
            KeyboardState keyInput = Keyboard.GetState();           
            if (keyInput.IsKeyDown(Keys.Escape))
            {
                game.Exit();
            }

            //Get mouse left click to identify which sprite the user chooses
            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed ){
                select.X = mouse.X;
                select.Y = mouse.Y;                
                oneClick = true;
                        
                if (one1.Contains(select))
                {
                    alertF=true;
                    tex11 = alertTex;
                }
                if (two1.Contains(select))
                {
                    cameraF=true;
                    tex21 = cameraTex;
                }
                if (three1.Contains(select))
                {
                    coneF=true;
                    tex31 = coneTex;
                }                 
                if (four1.Contains(select))
                {
                    diceF=true;
                    tex41 = diceTex;
                }
                if (one2.Contains(select))
                {
                    floppyF=true;
                    tex12 = floppyTex;
                }
                if (two2.Contains(select))
                {
                    mammathF=true;
                    tex22 = mammathTex;
                }
                if (three2.Contains(select))
                {
                    paintF=true;
                    tex32 = paintTex;
                }
                if (four2.Contains(select))
                {
                    treeF=true;
                    tex42 = treeTex;
                }                        
                if (one3.Contains(select))
                {
                    globeF = true;
                    tex13 = globeTex;
                }
                if (two3.Contains(select))
                {
                    globeF = true;
                    tex23 = globeTex;
                }
                if (three3.Contains(select))
                {
                    globeF = true;
                    tex33 = globeTex;
                }
                if (four3.Contains(select))
                {
                    globeF = true;
                    tex43 = globeTex;
                }                
            }
           

            base.Update(gameTime);
        }

        public void Draw(SpriteBatch sb)
        {                     
            
            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);            
            //sb.Draw(cover, coverR, null, Color.White, 0, new Vector2((win_width / 2 - cover.Width / 2), win_height / 2 - cover.Height / 2), SpriteEffects.None, 1.0f);                                       
            sb.Draw(tex11, box11, null, Color.White, 0, new Vector2((win_width / 2 - tex11.Width / 2), (win_height / 2 - tex11.Height / 2)), SpriteEffects.None, 1.0f);
            sb.Draw(tex21, box21, null, Color.White, 0, new Vector2((win_width / 2 - tex21.Width / 2), (win_height / 2 - tex21.Height / 2)), SpriteEffects.None, 1.0f);
            sb.Draw(tex31, box31, null, Color.White, 0, new Vector2((win_width / 2 - tex31.Width / 2), win_height / 2 - tex31.Height / 2), SpriteEffects.None, 1.0f);
            sb.Draw(tex41, box41, null, Color.White, 0, new Vector2((win_width / 2 - tex41.Width / 2), win_height / 2 - tex41.Height / 2), SpriteEffects.None, 1.0f);

            sb.Draw(tex12, box12, null, Color.White, 0, new Vector2((win_width / 2 - tex12.Width / 2), (win_height / 2 - tex12.Height / 2)), SpriteEffects.None, 1.0f);
            sb.Draw(tex22, box22, null, Color.White, 0, new Vector2((win_width / 2 - tex22.Width / 2), win_height / 2 - tex22.Height / 2), SpriteEffects.None, 1.0f);
            sb.Draw(tex32, box32, null, Color.White, 0, new Vector2((win_width / 2 - tex32.Width / 2), win_height / 2 - tex32.Height / 2), SpriteEffects.None, 1.0f);
            sb.Draw(tex42, box42, null, Color.White, 0, new Vector2((win_width / 2 - tex42.Width / 2), (win_height / 2 - tex42.Height / 2)), SpriteEffects.None, 1.0f);

            sb.Draw(tex13, box13, null, Color.White, 0, new Vector2((win_width / 2 - tex13.Width / 2), win_height / 2 - tex13.Height / 2), SpriteEffects.None, 1.0f);
            sb.Draw(tex23, box23, null, Color.White, 0, new Vector2((win_width / 2 - tex23.Width / 2), win_height / 2 - tex23.Height / 2), SpriteEffects.None, 1.0f);
            sb.Draw(tex33, box33, null, Color.White, 0, new Vector2((win_width / 2 - tex33.Width / 2), win_height / 2 - tex33.Height / 2), SpriteEffects.None, 1.0f);
            sb.Draw(tex43, box43, null, Color.White, 0, new Vector2((win_width / 2 - tex43.Width / 2), win_height / 2 - tex43.Height / 2), SpriteEffects.None, 1.0f);
            
            sb.End();
           
            
        }

    }
}
