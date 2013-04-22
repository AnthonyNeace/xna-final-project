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

    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        MouseState currentmouse, previousmouse;

        Game1 parent;

        SpriteBatch spriteBatch;
        SpriteFont font;
        Color fontcolor = Color.Black;
        bool isgametrayopen = false;
        bool isfoodtrayopen = false;

        List<Sprite> buttons = new List<Sprite>();
        List<Sprite> gamelist = new List<Sprite>();
        List<Sprite> foodlist = new List<Sprite>();
        Button exitbutton;

        public SpriteManager(Game game)
            : base(game)
        {
            parent = (Game1)game;
        }

        public override void Initialize()
        {
            //spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            font = Game.Content.Load<SpriteFont>(@"fonts/Spritefont");



            buttons.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(10, 20),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Food >"));

            buttons.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(10, 70),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Accessories"));

            buttons.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(10, 120),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Games >"));

            buttons.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(10, 170),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Save Data"));

            gamelist.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(-200, 120),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "RPS"));

            gamelist.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(-200, 120),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Matching"));

            gamelist.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(-200, 120),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Cards"));

            foodlist.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(-200, 20),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Apple\nS:-10/H:+1"));

            foodlist.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(-200, 20),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Cherry\nS:-25/H:+5"));

            foodlist.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(-200, 20),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Banana\nS:-50/H:+10"));



            exitbutton = new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(parent.Window.ClientBounds.Width - 120, 0),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Go Back");

            foreach (Sprite b in buttons)
            {
                b.buttonhover = Game.Content.Load<Texture2D>("buttonhover");
                b.buttonnorm = Game.Content.Load<Texture2D>("buttonnorm");
                b.buttonpressed = Game.Content.Load<Texture2D>("buttonpressed");
            }

            foreach (Sprite b in gamelist)
            {
                b.buttonhover = Game.Content.Load<Texture2D>("buttonhover");
                b.buttonnorm = Game.Content.Load<Texture2D>("buttonnorm");
                b.buttonpressed = Game.Content.Load<Texture2D>("buttonpressed");
            }

            foreach (Sprite b in foodlist)
            {
                b.buttonhover = Game.Content.Load<Texture2D>("buttonhover");
                b.buttonnorm = Game.Content.Load<Texture2D>("buttonnorm");
                b.buttonpressed = Game.Content.Load<Texture2D>("buttonpressed");
            }

            exitbutton.buttonhover = Game.Content.Load<Texture2D>("buttonhover");
            exitbutton.buttonnorm = Game.Content.Load<Texture2D>("buttonnorm");
            exitbutton.buttonpressed = Game.Content.Load<Texture2D>("buttonpressed");

            //font = Game.Content.Load<SpriteFont>("Spritefont");
            base.Initialize();
        }

        public void ClearLists()
        {
                buttons.Clear();
                gamelist.Clear();
                foodlist.Clear();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        void opengametray()
        {
            int i = 120;
            foreach (Sprite b in gamelist)
            {
                b.position.X = i;
                i += 110;
            }
        }

        void openfoodtray()
        {
            int i = 120;
            foreach (Sprite b in foodlist)
            {
                b.position.X = i;
                i += 110;
            }
        }

        void closegametray()
        {
            foreach (Sprite b in gamelist)
            {
                b.position.X = -300;
            }
        }

        void closefoodtray()
        {
            foreach (Sprite b in foodlist)
            {
                b.position.X = -300;
            }
        }

        public override void Update(GameTime gameTime)
        {
            currentmouse = Mouse.GetState();

            if (isgametrayopen) opengametray();
            else closegametray();

            if (isfoodtrayopen) openfoodtray();
            else closefoodtray();

            switch (parent.currentState)
            {
                case Game1.GameState.Home:
                    foreach (Sprite b in buttons)
                    {
                        if (b.text.CompareTo("Games >") == 0 && isgametrayopen)
                        {
                            b.opacity = 0.0f;
                            b.fontcolor = Color.White;
                        }
                        if (b.text.CompareTo("Food >") == 0 && isfoodtrayopen)
                        {
                            b.opacity = 0.0f;
                            b.fontcolor = Color.White;
                        }
                        if (b.collisionRect.Contains(currentmouse.X, currentmouse.Y) &&
                            currentmouse.LeftButton == ButtonState.Pressed &&
                            previousmouse.LeftButton == ButtonState.Released)
                        {
                            if (b.text.CompareTo("Games >") == 0)
                            {
                                isgametrayopen = !isgametrayopen;
                            }

                            if (b.text.CompareTo("Food >") == 0)
                            {
                                isfoodtrayopen = !isfoodtrayopen;
                            }
                            if (b.text.CompareTo("Save Data") == 0)
                            {
                                parent.gameFile.saveFile();
                            }
                        }

                        b.Update(gameTime);
                    }
                    foreach (Sprite b in gamelist)
                    {
                        if (b.collisionRect.Contains(currentmouse.X, currentmouse.Y) &&
                            currentmouse.LeftButton == ButtonState.Pressed &&
                            previousmouse.LeftButton == ButtonState.Released)
                        {
                            if (b.text.CompareTo("RPS") == 0)
                            {
                                parent.currentState = Game1.GameState.RPS;
                                isgametrayopen = !isgametrayopen;
                            }
                            if (b.text.CompareTo("Matching") == 0)
                            {
                                parent.currentState = Game1.GameState.Matching;
                                isgametrayopen = !isgametrayopen;
                            }
                            if (b.text.CompareTo("Cards") == 0)
                            {
                                parent.currentState = Game1.GameState.Cards;
                                isgametrayopen = !isgametrayopen;
                            }
                        }
                        b.Update(gameTime);
                    }
                    foreach (Sprite b in foodlist)
                    {
                        if (b.collisionRect.Contains(currentmouse.X, currentmouse.Y) &&
                            currentmouse.LeftButton == ButtonState.Pressed &&
                            previousmouse.LeftButton == ButtonState.Released)
                        {
                            if (b.text.CompareTo("Apple\nS:-10/H:+1") == 0)
                            {
                                if (parent.score >= 10)
                                {
                                    parent.score -= 10;
                                    parent.happiness += 1;
                                }
                                //isfoodtrayopen = !isfoodtrayopen;
                            }
                            if (b.text.CompareTo("Cherry\nS:-25/H:+5") == 0)
                            {
                                if (parent.score >= 25)
                                {
                                    parent.score -= 25;
                                    parent.happiness += 5;
                                }
                                //isfoodtrayopen = !isfoodtrayopen;
                            }
                            if (b.text.CompareTo("Banana\nS:-50/H:+10") == 0)
                            {
                                if (parent.score >= 50)
                                {
                                    parent.score -= 50;
                                    parent.happiness += 10;
                                }
                                //isfoodtrayopen = !isfoodtrayopen;
                            }
                        }
                        b.Update(gameTime);
                    }
                    break;
                case Game1.GameState.RPS:
                    if (exitbutton.collisionRect.Contains(currentmouse.X, currentmouse.Y) &&
                            currentmouse.LeftButton == ButtonState.Pressed &&
                            previousmouse.LeftButton == ButtonState.Released)
                    {
                        parent.currentState = Game1.GameState.Home;
                        parent.gameFile.saveFile();
                    }
                    exitbutton.Update(gameTime);
                    break;
                case Game1.GameState.Matching:
                    if (exitbutton.collisionRect.Contains(currentmouse.X, currentmouse.Y) &&
                            currentmouse.LeftButton == ButtonState.Pressed &&
                            previousmouse.LeftButton == ButtonState.Released)
                    {
                        parent.currentState = Game1.GameState.Home;
                        parent.gameFile.saveFile();
                    }
                    exitbutton.Update(gameTime);
                    break;
                case Game1.GameState.Cards:
                    if (exitbutton.collisionRect.Contains(currentmouse.X, currentmouse.Y) &&
                            currentmouse.LeftButton == ButtonState.Pressed &&
                            previousmouse.LeftButton == ButtonState.Released)
                    {
                        parent.currentState = Game1.GameState.Home;
                        parent.gameFile.saveFile();
                    }
                    exitbutton.Update(gameTime);
                    break;
            }

            previousmouse = currentmouse;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            
            // Draw the buttons
            switch (parent.currentState)
            {
                case Game1.GameState.Home:
                    foreach (Sprite b in buttons)
                    {
                        b.Draw(gameTime, spriteBatch);
                    }

                    foreach (Sprite b in gamelist)
                    {
                        b.Draw(gameTime, spriteBatch);
                    }
                    foreach (Sprite b in foodlist)
                    {
                        b.Draw(gameTime, spriteBatch);
                    }
                    break;
                case Game1.GameState.RPS:
                    exitbutton.Draw(gameTime, spriteBatch);
                    break;
                case Game1.GameState.Matching:
                    exitbutton.Draw(gameTime, spriteBatch);
                    break;
                case Game1.GameState.Cards:
                    exitbutton.Draw(gameTime, spriteBatch);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
