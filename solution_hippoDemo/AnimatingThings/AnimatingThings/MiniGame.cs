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
    class MiniGame: Microsoft.Xna.Framework.GameComponent
    {

        MouseState currentmouse, previousmouse;
        Texture2D rock, paper, scissors, p,c;
        SpriteFont sf;
        Game1 game;
        Rectangle rockRec, paperRec, scissorsRec, pr, cr;
        int win_width, win_height;
        String result = "";
        Point select = new Point();
        Random rand = new Random();
        Boolean played = false, waiting = true;
        int player = -1, computer = -1;   

        public MiniGame(Game g)
            : base(g)
        {
            game = (Game1)g;

            win_width = game.Window.ClientBounds.Width;
            win_height = game.Window.ClientBounds.Height;

            rock = game.Content.Load<Texture2D>(@"MiniGameImages/Rock3");
            paper = game.Content.Load<Texture2D>(@"MiniGameImages/Paper");
            scissors = game.Content.Load<Texture2D>(@"MiniGameImages/Scissor");

            //set default values for player and computer game choice textures
            p = game.Content.Load<Texture2D>(@"MiniGameImages/Paper");
            c = paper = game.Content.Load<Texture2D>(@"MiniGameImages/Paper");

            sf = game.Content.Load<SpriteFont>(@"fonts\SpriteFont");   
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

            if (game.inMini == true)
            {
                //Reset game
                if (keyInput.IsKeyDown(Keys.R))
                {
                    player = -1;
                    played = false;
                    waiting = true;
                    result = "";
                }


                //Get mouse left click to identify which sprite the user chooses
                currentmouse = Mouse.GetState();
                if (currentmouse.LeftButton == ButtonState.Pressed &&
                    previousmouse.LeftButton == ButtonState.Released && waiting == true)
                {
                    select.X = currentmouse.X;
                    select.Y = currentmouse.Y;
                    waiting = false;
                    played = true;
                    // Console.WriteLine("Point X: " + select.X + ", Point Y: " + select.Y);
                    computer = rand.Next(3);
                }

                //Get which sprite the player clicks on as their choice
                //if (rockRec.Contains(select))
                if (select.X <= 275)
                {
                    player = 0;
                    p = rock;
                }
                else if (select.X <= 510 && select.X > 275)
                {
                    player = 1;
                    p = paper;
                }
                else if (select.X > 510)
                {
                    player = 2;
                    p = scissors;
                }

                //Get pets choice by getting a random number between 0 and 2.
                //assign texture and rect of the resulting number to display                        
                if (computer == 0)
                {
                    c = rock;
                }
                else if (computer == 1)
                {
                    c = paper;
                }
                else if (computer == 2)
                {
                    c = scissors;
                }

                //Check who won.
                // 0->Rock, 1->Paper, 2->Scissors
                if (player == computer)
                {
                    result = "Tie";
                }
                else if ((player == 0 && computer == 2) || (player == 1 && computer == 0) || (player == 2 && computer == 1))
                {
                    result = "You Won!";
                }
                else
                {
                    result = "You Lost";
                }
            }

            previousmouse = currentmouse;
            base.Update(gameTime);
        }

        public void Draw(SpriteBatch sb)
        {            
            if (waiting == true)
            {                
                rockRec = new Rectangle(400, 305, 200, 200);
                paperRec = new Rectangle(685, 290, 200, 200);
                scissorsRec = new Rectangle(900, 275, 200, 200);
               
                sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                sb.Draw(rock, rockRec, null, Color.White, 0, new Vector2((win_width / 2 - rock.Width / 2), (win_height / 2 - rock.Height / 2)), SpriteEffects.None, 1.0f);
                sb.Draw(paper, paperRec, null, Color.White, 0, new Vector2((win_width / 2 - paper.Width / 2), win_height / 2 - paper.Height / 2), SpriteEffects.None, 1.0f);
                sb.Draw(scissors, scissorsRec, null, Color.White, 0, new Vector2((win_width / 2 - scissors.Width / 2), win_height / 2 - scissors.Height / 2), SpriteEffects.None, 1.0f);
                sb.End();
            }
            else if (played == true)
            {
                pr = new Rectangle(480, 290, 200, 200);
                cr = new Rectangle(800, 290, 200, 200);
                if (p == rock)
                {
                    pr = new Rectangle(480, 315, 200, 200);
                }
                if (c == rock)
                {
                    cr = new Rectangle(800, 315, 200, 200);
                }

                sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                sb.DrawString(sf, result, new Vector2((win_width / 2) - (sf.MeasureString(result).X / 2),(sf.MeasureString(result).Y / 2)), Color.Black);
                sb.Draw(p, pr, null, Color.White, 0, new Vector2((win_width / 2 - p.Width / 2), (win_height / 2 - p.Height / 2)), SpriteEffects.None, 1.0f);
                sb.Draw(c, cr, null, Color.White, 0, new Vector2((win_width / 2 - c.Width / 2), win_height / 2 - c.Height / 2), SpriteEffects.None, 1.0f);
                sb.DrawString(sf, "You", new Vector2((win_width / 4) - (sf.MeasureString("You").X / 2), (win_height - sf.MeasureString("You").Y ) ), Color.Black);
                sb.DrawString(sf, "Pet", new Vector2((win_width / 4)*3 - (sf.MeasureString("Pet").X / 2)-50, (win_height - sf.MeasureString("Pet").Y ) ), Color.Black);
                sb.End();
            }

            
        }

    }
}
