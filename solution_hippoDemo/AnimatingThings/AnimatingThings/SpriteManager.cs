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

        List<Sprite> buttons = new List<Sprite>();
        List<Sprite> gamelist = new List<Sprite>();

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
                "Feed"));

            buttons.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(10, 70),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Pet"));

            buttons.Add(new Button(Game.Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(10, 120),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Play"));

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

            //font = Game.Content.Load<SpriteFont>("Spritefont");
            base.Initialize();
        }

        public void ClearLists()
        {
                buttons.Clear();
                gamelist.Clear();
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

        void closegametray()
        {
            foreach (Sprite b in gamelist)
            {
                b.position.X = -300;
            }
        }

        public override void Update(GameTime gameTime)
        {
            currentmouse = Mouse.GetState();

            if (isgametrayopen) opengametray();
            else closegametray();

            foreach (Sprite b in buttons)
            {
                if (b.text.CompareTo("Play") == 0 && isgametrayopen)
                {
                    b.opacity = 0.0f;
                    b.fontcolor = Color.White;
                }
                if (b.text.CompareTo("Play")==0 &&
                    b.collisionRect.Contains(currentmouse.X, currentmouse.Y) &&
                    currentmouse.LeftButton == ButtonState.Pressed &&
                    previousmouse.LeftButton == ButtonState.Released)
                {
                    isgametrayopen = !isgametrayopen;
                }

                b.Update(gameTime);
            }
            foreach (Sprite b in gamelist)
            {
                if (b.text.CompareTo("RPS") == 0 &&
                    b.collisionRect.Contains(currentmouse.X, currentmouse.Y) &&
                    currentmouse.LeftButton == ButtonState.Pressed &&
                    previousmouse.LeftButton == ButtonState.Released)
                {
                    parent.currentState = Game1.GameState.InMiniGame;
                    parent.inMini = true;
                }
                b.Update(gameTime);
            }

            previousmouse = currentmouse;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            
            // Draw the buttons
            foreach(Sprite b in buttons)
            {
                b.Draw(gameTime, spriteBatch);
            }

            foreach (Sprite b in gamelist)
            {
                b.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
