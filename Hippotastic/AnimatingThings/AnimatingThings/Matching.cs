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
    public class Matching : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteFont sf, sfWin;
        Game1 game;
        int win_width, win_height;
        Point select = new Point();
        Random rand = new Random();
        Boolean oneClick = false, twoClick = false;
        Texture2D one, two;
        //Textures for each box on board
        Texture2D tex11, tex21, tex31, tex41, tex12, tex22, tex32, tex42, tex13, tex23, tex33, tex43;
        Texture2D alertTex, balloonTex, cameraTex, coneTex, diceTex, floppyTex, globeTex, mammathTex, paintTex, treeTex, coverTex;
        //Texture2D[] matches = new Texture2D[12];
        List<Texture2D> matches = new List<Texture2D>() { };
        List<Texture2D> texList = new List<Texture2D>() { };
        Boolean b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, won = false;
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
        Texture2D[] t = new Texture2D[6];
        Texture2D[] boxToSet = new Texture2D[12];
        MouseState cur, prev;
        int resetCnt = 0;

        int l = 100, u = 100;

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
            texList.Add(tex11 = coverTex);
            texList.Add(tex21 = coverTex);
            texList.Add(tex31 = coverTex);
            texList.Add(tex41 = coverTex);
            texList.Add(tex12 = coverTex);
            texList.Add(tex22 = coverTex);
            texList.Add(tex32 = coverTex);
            texList.Add(tex42 = coverTex);
            texList.Add(tex13 = coverTex);
            texList.Add(tex23 = coverTex);
            texList.Add(tex33 = coverTex);
            texList.Add(tex43 = coverTex);
            t[0] = alertTex;
            t[1] = paintTex;
            t[2] = cameraTex;
            t[3] = mammathTex;
            t[4] = floppyTex;
            t[5] = globeTex;
            for (int i = 0; i < 12; i++)
            {
                boxToSet[i] = coverTex;
            }
            setBoard();
            sf = game.Content.Load<SpriteFont>("fonts/font");
            sfWin = game.Content.Load<SpriteFont>("fonts/SpriteFont1");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void setBoard()
        {
            int cnt = 0;
            int[] texSeen = new int[6];
            int[] seen = new int[12];

            for (int i = 0; i < 12; i += 2)
            {
                Boolean validNums = false;
                int n1 = rand.Next(12) + 1;
                int n2 = rand.Next(12) + 1;
                while (!validNums)
                {
                    if (n1 != n2 && i == 0)
                    {
                        seen[i] = n1;
                        seen[i + 1] = n2;
                        validNums = true;
                    }
                    if (n1 == n2)
                    {
                        n2 = rand.Next(12) + 1;
                    }
                    else if (seen.Contains(n1) == true && i > 0)
                    {
                        n1 = rand.Next(12) + 1;
                    }
                    else if (seen.Contains(n2) == true && i > 0)
                    {
                        n2 = rand.Next(12) + 1;
                    }
                    else
                    {
                        seen[i] = n1;
                        seen[i + 1] = n2;
                        validNums = true;
                    }
                }
                boxToSet[n1 - 1] = t[cnt];
                boxToSet[n2 - 1] = t[cnt];
                cnt++;
            }
        }

        public void recoverBoxes()
        {
            if (matches.Contains(tex11) == false)
            {
                tex11 = coverTex;
                b1 = false;
            }
            if (matches.Contains(tex21) == false)
            {
                tex21 = coverTex;
                b2 = false;
            }
            if (matches.Contains(tex31) == false)
            {
                tex31 = coverTex;
                b3 = false;
            }
            if (matches.Contains(tex41) == false)
            {
                tex41 = coverTex;
                b4 = false;
            }
            if (matches.Contains(tex12) == false)
            {
                tex12 = coverTex;
                b5 = false;
            }
            if (matches.Contains(tex22) == false)
            {
                tex22 = coverTex;
                b6 = false;
            }
            if (matches.Contains(tex32) == false)
            {
                tex32 = coverTex;
                b7 = false;
            }
            if (matches.Contains(tex42) == false)
            {
                tex42 = coverTex;
                b8 = false;
            }
            if (matches.Contains(tex13) == false)
            {
                tex13 = coverTex;
                b9 = false;
            }
            if (matches.Contains(tex23) == false)
            {
                tex23 = coverTex;
                b10 = false;
            }
            if (matches.Contains(tex33) == false)
            {
                tex33 = coverTex;
                b11 = false;
            }
            if (matches.Contains(tex43) == false)
            {
                tex43 = coverTex;
                b12 = false;
            }
        }

        public void ResetMatching()
        {
            won = false;
            matches.Clear();
            recoverBoxes();
            setBoard();
        }

        public override void Update(GameTime gameTime)
        {
            if (game.currentState == Game1.GameState.Matching)
            {
                KeyboardState keyInput = Keyboard.GetState();
                if (keyInput.IsKeyDown(Keys.Escape))
                {
                    game.Exit();
                }
                if (keyInput.IsKeyDown(Keys.R))
                {
                    won = false;
                    matches.Clear();
                    recoverBoxes();
                    setBoard();
                }

                //Get mouse left click to identify which sprite the user chooses
                cur = Mouse.GetState();
                if (cur.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released)
                {
                    select = new Point(cur.X, cur.Y);
                    //sets the boxes texture to it icon when clicked   
                    if (one1.Contains(select) && !b1)
                    {
                        b1 = true;
                        tex11 = boxToSet[0];
                        if (oneClick == false)
                        {
                            one = tex11;
                        }
                        else if (twoClick == false)
                        {
                            two = tex11;
                            twoClick = true;
                        }
                    }
                    else if (two1.Contains(select) && !b2)
                    {
                        b2 = true;
                        tex21 = boxToSet[1];
                        if (oneClick == false)
                        {
                            one = tex21;
                        }
                        else if (twoClick == false)
                        {
                            two = tex21;
                            twoClick = true;
                        }
                    }
                    else if (three1.Contains(select) && !b3)
                    {
                        b3 = true;
                        tex31 = boxToSet[2];
                        if (oneClick == false)
                        {
                            one = tex31;
                        }
                        else if (twoClick == false)
                        {
                            two = tex31;
                            twoClick = true;
                        }
                    }
                    else if (four1.Contains(select) && !b4)
                    {
                        b4 = true;
                        tex41 = boxToSet[3];
                        if (oneClick == false)
                        {
                            one = tex41;
                        }
                        else if (twoClick == false)
                        {
                            two = tex41;
                            twoClick = true;
                        }
                    }
                    else if (one2.Contains(select) && !b5)
                    {
                        b5 = true;
                        tex12 = boxToSet[4];
                        if (oneClick == false)
                        {
                            one = tex12;
                        }
                        else if (twoClick == false)
                        {
                            two = tex12;
                            twoClick = true;
                        }
                    }
                    else if (two2.Contains(select) && !b6)
                    {
                        b6 = true;
                        tex22 = boxToSet[5];
                        if (oneClick == false)
                        {
                            one = tex22;
                        }
                        else if (twoClick == false)
                        {
                            two = tex22;
                            twoClick = true;
                        }
                    }
                    else if (three2.Contains(select) && !b7)
                    {
                        b7 = true;
                        tex32 = boxToSet[6];
                        if (oneClick == false)
                        {
                            one = tex32;
                        }
                        else if (twoClick == false)
                        {
                            two = tex32;
                            twoClick = true;
                        }
                    }
                    else if (four2.Contains(select) && !b8)
                    {
                        b8 = true;
                        tex42 = boxToSet[7];
                        if (oneClick == false)
                        {
                            one = tex42;
                        }
                        else if (twoClick == false)
                        {
                            two = tex42;
                            twoClick = true;
                        }
                    }
                    else if (one3.Contains(select) && !b9)
                    {
                        b9 = true;
                        tex13 = boxToSet[8];
                        if (oneClick == false)
                        {
                            one = tex13;
                        }
                        else if (twoClick == false)
                        {
                            two = tex13;
                            twoClick = true;
                        }
                    }
                    else if (two3.Contains(select) && !b10)
                    {
                        b10 = true;
                        tex23 = boxToSet[9];
                        if (oneClick == false)
                        {
                            one = tex23;
                        }
                        else if (twoClick == false)
                        {
                            two = tex23;
                            twoClick = true;
                        }
                    }
                    else if (three3.Contains(select) && !b11)
                    {
                        b11 = true;
                        tex33 = boxToSet[10];
                        if (oneClick == false)
                        {
                            one = tex33;
                        }
                        else if (twoClick == false)
                        {
                            two = tex33;
                            twoClick = true;
                        }
                    }
                    else if (four3.Contains(select) && !b12)
                    {
                        b12 = true;
                        tex43 = boxToSet[11];
                        if (oneClick == false)
                        {
                            one = tex43;
                        }
                        else if (twoClick == false)
                        {
                            two = tex43;
                            twoClick = true;
                        }
                    }
                    if (oneClick == true)
                    {
                        twoClick = true;
                    }
                    resetCnt++;
                    oneClick = true;
                }//end of click check

                if (resetCnt == 2 && cur.LeftButton == ButtonState.Released)
                {
                    //if a match is found add those boxes to the matches list
                    if (one == two)
                    {
                        matches.Add(one);
                        matches.Add(two);
                    }
                    int waitTime = 0;
                    while (waitTime < 999999)
                    {
                        waitTime++;
                    }
                    //recover boxes that were uncovered but not a match
                    if (waitTime >= 99999)
                    {
                        recoverBoxes();
                        one = two = null;
                        oneClick = twoClick = false;
                        resetCnt = 0;
                    }
                }

                if (matches.Count() == 12)
                {
                    matches.Clear();
                    game.score += 50;
                    won = true;
                }
                prev = cur;
                //Console.WriteLine("Score in matching = " + game.score);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = new SpriteBatch(Game.GraphicsDevice); ;
            Rectangle text = new Rectangle(5, 2, 2, 2);
            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            if (won == false)
            {
                sb.DrawString(sf, "Find the matches!", new Vector2(5, 2), Color.Black);
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
            }
            else if (won == true)
            {
                String s = "You Won!";
                String ss = "Your score is now";
                sb.DrawString(sfWin, game.score + "", new Vector2(355, 261), Color.Black);
                sb.DrawString(sfWin, ss, new Vector2(100, 179), Color.Black);
                sb.DrawString(sfWin, s, new Vector2(219, 100), Color.Black);
            }
            sb.End();
        }
    }
}
