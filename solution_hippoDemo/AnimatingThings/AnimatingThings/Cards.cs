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
    class Cards : Microsoft.Xna.Framework.GameComponent
    {
        List<Texture2D> cardList = new List<Texture2D>() { };
        Texture2D aceH, kingH, queenH, jackH, tenH, nineH, eightH, sevenH, sixH, fiveH, fourH, threeH, twoH, back;
        Texture2D aceD, kingD, queenD, jackD, tenD, nineD, eightD, sevenD, sixD, fiveD, fourD, threeD, twoD, playerCard;
        Texture2D aceC, kingC, queenC, jackC, tenC, nineC, eightC, sevenC, sixC, fiveC, fourC, threeC, twoC;
        Texture2D aceS, kingS, queenS, jackS, tenS, nineS, eightS, sevenS, sixS, fiveS, fourS, threeS, twoS;
        SpriteFont sf, sfScore, sfScore2, title;
        Game1 game;
        Rectangle cardR, higher, lower;        
        Rectangle higherClicked = new Rectangle(554, 74, 150, 150);
        Rectangle lowerClicked = new Rectangle(560, 266, 150, 150);
        Rectangle cardClicked = new Rectangle(288, 120, 200, 250);
        int win_width, win_height;       
        Point select = new Point();
        Random rand = new Random();
        Boolean played = false, waiting = true;
        int result, prevCard, curCard,score=0;
        Texture2D btn, background;
        Texture2D test;
        MouseState cur, prev;

        String output = "Score:", winLose="Begin";
        int left=150, up=50;

        public Cards(Game g)
            : base(g)
        {
            game = (Game1)g;

            win_width = game.Window.ClientBounds.Width;
            win_height = game.Window.ClientBounds.Height;           

            btn = game.Content.Load<Texture2D>(@"MiniGameImages/redChip");            
            //cardList.Add(back = game.Content.Load<Texture2D>(@"MiniGameImages/cards/back"));
            background = game.Content.Load<Texture2D>(@"MiniGameImages/felt"); 
            cardList.Add(twoS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/s2"));
            cardList.Add(twoH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/h2"));
            cardList.Add(twoC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/c2"));
            cardList.Add(twoD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/d2"));
            cardList.Add(threeD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/d3"));
            cardList.Add(threeC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/c3"));
            cardList.Add(threeH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/h3"));
            cardList.Add(threeS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/s3"));
            cardList.Add(fourS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/s4"));
            cardList.Add(fourH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/h4"));
            cardList.Add(fourC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/c4"));
            cardList.Add(fourD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/d4"));
            cardList.Add(fiveD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/d5"));
            cardList.Add(fiveH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/h5"));
            cardList.Add(fiveC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/c5"));
            cardList.Add(fiveS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/s5"));
            cardList.Add(sixS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/s6"));
            cardList.Add(sixC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/c6"));
            cardList.Add(sixH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/h6"));
            cardList.Add(sixD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/d6"));
            cardList.Add(sevenD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/d7"));
            cardList.Add(sevenH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/h7"));
            cardList.Add(sevenC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/c7"));
            cardList.Add(sevenS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/s7"));
            cardList.Add(eightS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/s8"));
            cardList.Add(eightH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/h8"));
            cardList.Add(eightC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/c8"));
            cardList.Add(eightD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/d8"));
            cardList.Add(nineD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/d9"));
            cardList.Add(nineC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/c9"));
            cardList.Add(nineH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/h9"));
            cardList.Add(nineS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/s9"));
            cardList.Add(tenS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/s10"));
            cardList.Add(tenH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/h10"));
            cardList.Add(tenC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/c10"));
            cardList.Add(tenD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/d10"));
            cardList.Add(jackD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/dj"));
            cardList.Add(jackC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/cj"));
            cardList.Add(jackH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/hj"));
            cardList.Add(jackS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/sj"));
            cardList.Add(queenS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/sq"));
            cardList.Add(queenC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/cq"));
            cardList.Add(queenH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/hq"));
            cardList.Add(queenD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/dq"));
            cardList.Add(kingD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/dk"));
            cardList.Add(kingH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/hk"));
            cardList.Add(kingC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/ck"));
            cardList.Add(kingS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/sk"));
            cardList.Add(aceC = game.Content.Load<Texture2D>(@"MiniGameImages/cards/c1"));
            cardList.Add(aceS = game.Content.Load<Texture2D>(@"MiniGameImages/cards/s1"));
            cardList.Add(aceH = game.Content.Load<Texture2D>(@"MiniGameImages/cards/h1"));
            cardList.Add(aceD = game.Content.Load<Texture2D>(@"MiniGameImages/cards/d1"));
            
                        
            test = new Texture2D(game.GraphicsDevice, 1, 1);
            test.SetData(new[] { Color.White });

            sf = game.Content.Load<SpriteFont>(@"fonts/SpriteFont");
            sfScore = game.Content.Load<SpriteFont>(@"fonts/SpriteFont");
            sfScore2 = game.Content.Load<SpriteFont>(@"fonts/SpriteFont");
            title = game.Content.Load<SpriteFont>(@"fonts/SpriteFont"); 
        }
       
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {            
            KeyboardState keyInput = Keyboard.GetState();           
            if (keyInput.IsKeyDown(Keys.Escape))
            {
                game.Exit();
            }

            if (keyInput.IsKeyDown(Keys.Up))
            {
                up -= 1;
            }
            if (keyInput.IsKeyDown(Keys.Down))
            {
                up += 1;
            }
            if (keyInput.IsKeyDown(Keys.Left))
            {
                left -= 1;
            }
            if (keyInput.IsKeyDown(Keys.Right))
            {
                left += 1;
            }
            //Console.WriteLine("left: " + left + ", up: " + up);
            //Get mouse left click to identify which sprite the user chooses
            cur = Mouse.GetState();
            if (cur.LeftButton == ButtonState.Pressed && prev.LeftButton==ButtonState.Released)
            {
                select.X = cur.X;
                select.Y = cur.Y;                              
                if (higherClicked.Contains(select))
                {
                    result = rand.Next(51) + 1;
                    curCard = (result / 4) + 1;
                    if (prevCard < curCard)
                    {
                        score += 2;
                        winLose = "Right";
                    }
                    else if (prevCard >= curCard)
                    {
                        score -= 1;
                        winLose = "Wrong";
                    }
                }
                if (lowerClicked.Contains(select))
                {
                    result = rand.Next(51) + 1;
                    curCard = (result / 4) + 1;
                    if (prevCard > curCard)
                    {
                        score += 2;
                        winLose = "Right";
                    }
                    else if (prevCard <= curCard)
                    {
                        score -= 1;
                        winLose = "Wrong";
                    }
                }                
                output = "Score:";
                if (score < 0)
                {
                    score = 0;
                }
            }
            prev = cur;
            prevCard = curCard;
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch sb)
        {                                               
            lower = new Rectangle(1106, 568, 150, 150);
            higher = new Rectangle(1106, 374, 150, 150);             
            cardR = new Rectangle(358, 96, 200, 250);
            Rectangle back = new Rectangle(0, 0, win_width, win_height);
                       
            sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);            
            sb.Draw(background, back, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1.0f); 
            sb.Draw(cardList[result], cardR, null, Color.White, 0, new Vector2((win_width / 2 - cardList[result].Width / 2), (win_height / 2 - cardList[result].Height / 2)), SpriteEffects.None, 0.5f);           
            sb.Draw(btn, higher, null, Color.White, 0, new Vector2((win_width / 2 - btn.Width / 2), (win_height / 2 - btn.Height / 2)), SpriteEffects.None, 0.1f);
            sb.Draw(btn, lower, null, Color.White, 0, new Vector2((win_width / 2 - btn.Width / 2), (win_height / 2 - btn.Height / 2)), SpriteEffects.None, 0.1f);
            sb.DrawString(sf, "Higher", new Vector2(598,137), Color.Black);
            sb.DrawString(sf, "Lower", new Vector2(603, 330), Color.Black);
            sb.DrawString(title, "Guess the card", new Vector2(20, 16), Color.Red);
            sb.DrawString(sfScore, winLose, new Vector2(70,168), Color.Red);
            sb.DrawString(sfScore2, output, new Vector2(92, 228), Color.Red);
            if (score < 10)
            {
                sb.DrawString(sfScore2, "" + score, new Vector2(142, 272), Color.Red);
            }
            else if (score > 9)
            {
                sb.DrawString(sfScore2, "" + score, new Vector2(126, 270), Color.Red);
            }
            sb.DrawString(sf, "Press H to return home", new Vector2(18, 444), Color.Black);
            sb.End();
            
        }

    }
}

