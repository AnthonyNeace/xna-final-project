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
    public class RPS : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D rock, paper, scissors, p, c, back;
        SpriteFont sf, sf1, sf2;
        Game1 game;
        Rectangle rockRec, paperRec, scissorsRec, pr, cr, backR;
        int win_width, win_height;
        String result = "";
        Point select = new Point();
        Random rand = new Random();
        Boolean played = false, waiting = true;
        int player = -1, computer = -1;
        MouseState mouse, prev;
        Texture2D rt, pt, st;
        Rectangle rr, pp, ss;
        Rectangle rockClick = new Rectangle(43, 119, 200, 200);
        Rectangle paperClick = new Rectangle(309, 125, 200, 200);
        Rectangle scissorsClick = new Rectangle(548, 128, 200, 200);

        public RPS(Game g)
            : base(g)
        {
            game = (Game1)g;
            win_width = game.Window.ClientBounds.Width;
            win_height = game.Window.ClientBounds.Height;
            rock = game.Content.Load<Texture2D>(@"MiniGameImages/Rock3");
            paper = game.Content.Load<Texture2D>(@"MiniGameImages/Paper");
            scissors = game.Content.Load<Texture2D>(@"MiniGameImages/Scissor");
            back = game.Content.Load<Texture2D>(@"MiniGameImages/74956");
            //set default values for player and computer game choice textures
            p = game.Content.Load<Texture2D>(@"MiniGameImages/Paper");
            c = paper = game.Content.Load<Texture2D>(@"MiniGameImages/Paper");
            sf = game.Content.Load<SpriteFont>("fonts/SpriteFont1");
            sf1 = game.Content.Load<SpriteFont>("fonts/font");
            sf2 = game.Content.Load<SpriteFont>("fonts/score");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void ResetRPS()
        {
            player = -1;
            played = false;
            waiting = true;
            result = "";
        }

        public override void Update(GameTime gameTime)
        {
                KeyboardState keyInput = Keyboard.GetState();
                if (keyInput.IsKeyDown(Keys.Escape))
                {
                    game.Exit();
                }

                //Reset game
                if (keyInput.IsKeyDown(Keys.R))
                {
                    player = -1;
                    played = false;
                    waiting = true;
                    result = "";
                }
                //Get mouse left click to identify which sprite the user chooses
                mouse = Mouse.GetState();
                if (mouse.LeftButton == ButtonState.Pressed && prev.LeftButton == ButtonState.Released && waiting == true)
                {
                    select.X = mouse.X;
                    select.Y = mouse.Y;
                    computer = rand.Next(3);

                    //Get which sprite the player clicks on as their choice                    
                    if (rockClick.Contains(select))
                    {
                        waiting = false;
                        played = true;
                        player = 0;
                        p = rock;
                    }
                    else if (paperClick.Contains(select))
                    {
                        waiting = false;
                        played = true;
                        player = 1;
                        p = paper;
                    }
                    else if (scissorsClick.Contains(select))
                    {
                        waiting = false;
                        played = true;
                        player = 2;
                        p = scissors;
                    }

                    if (played == true)
                    {
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
                            result = " Tie";
                            game.score += 1;
                        }
                        else if ((player == 0 && computer == 2) || (player == 1 && computer == 0) || (player == 2 && computer == 1))
                        {
                            result = "Won";
                            game.score += 2;
                        }
                        else
                        {
                            result = "Lost";
                            if (game.score > 0)
                            {
                                game.score -= 1;
                            }
                            else
                                game.score = 0;
                        }
                    }
                }
            

            prev = mouse;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = new SpriteBatch(Game.GraphicsDevice);
            if (waiting == true)
            {
                rockRec = new Rectangle(400, 305, 200, 200);
                paperRec = new Rectangle(685, 290, 200, 200);
                scissorsRec = new Rectangle(900, 275, 200, 200);
                Rectangle x = new Rectangle(-206, -382, 900, 700);

                sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                sb.DrawString(sf2, "Score: " + game.score, new Vector2(16, 12), Color.Black);
                sb.Draw(rock, rockRec, null, Color.White, 0, new Vector2((win_width / 2 - rock.Width / 2), (win_height / 2 - rock.Height / 2)), SpriteEffects.None, 0.1f);
                sb.Draw(paper, paperRec, null, Color.White, 0, new Vector2((win_width / 2 - paper.Width / 2), win_height / 2 - paper.Height / 2), SpriteEffects.None, 0.1f);
                sb.Draw(scissors, scissorsRec, null, Color.White, 0, new Vector2((win_width / 2 - scissors.Width / 2), (win_height / 2 - scissors.Height / 2)), SpriteEffects.None, 0.1f);
                sb.Draw(back, x, null, Color.White, 0, new Vector2((win_width / 2 - back.Width / 2), (win_height / 2 - back.Height / 2)), SpriteEffects.None, 1.0f);
                sb.End();
            }
            else if (played == true)
            {
                pr = new Rectangle(480, 290, 200, 200);
                cr = new Rectangle(800, 290, 200, 200);
                Rectangle x = new Rectangle(-206, -382, 900, 700);
                if (p == rock)
                {
                    pr = new Rectangle(480, 315, 200, 200);
                }
                if (c == rock)
                {
                    cr = new Rectangle(800, 315, 200, 200);
                }

                sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                sb.DrawString(sf, result, new Vector2(310, 40), Color.Black);
                sb.Draw(p, pr, null, Color.White, 0, new Vector2((win_width / 2 - p.Width / 2), (win_height / 2 - p.Height / 2)), SpriteEffects.None, 0.1f);
                sb.Draw(c, cr, null, Color.White, 0, new Vector2((win_width / 2 - c.Width / 2), win_height / 2 - c.Height / 2), SpriteEffects.None, 0.1f);
                sb.DrawString(sf1, "You", new Vector2(180, 376), Color.Black);
                sb.DrawString(sf1, "Pet", new Vector2(510, 376), Color.Black);
                sb.DrawString(sf2, "Score: " + game.score, new Vector2(16, 12), Color.Black);
                sb.Draw(back, x, null, Color.White, 0, new Vector2((win_width / 2 - back.Width / 2), (win_height / 2 - back.Height / 2)), SpriteEffects.None, 1.0f);
                sb.End();
            }


        }

    }
}
