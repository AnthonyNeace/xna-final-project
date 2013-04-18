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
        SpriteBatch spriteBatch;
        SpriteFont font;
        Color fontcolor = Color.Black;
        SpriteEffects spriteEffects;
        //Sprite button;
        List<Sprite> buttons = new List<Sprite>();

        public SpriteManager(Game game)
            : base(game)
        {

        }

        public override void Initialize()
        {
            //spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            for (int i = 0; i < 400; i += 50)
            {
                buttons.Add(new AutoSprite(Game.Content.Load<Texture2D>("buttonnorm"),//Testure
                    new Vector2(10, i+10),//Position
                    new Point(100, 50),//Framesize
                    0, //Collision Offset
                    new Point(0, 0), //Current Frame
                    new Point(2, 1), //Sheetsize
                    new Vector2(0, 1), //Speed
                    0)); //Score
            }
            font = Game.Content.Load<SpriteFont>("Spritefont");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            foreach(Sprite b in buttons){
                if (b.collisionRect.Contains(mouse.X, mouse.Y))
                {
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        b.textureImage = Game.Content.Load<Texture2D>("buttonpressed");
                        fontcolor = Color.White;
                    }
                    else
                    {
                        b.textureImage = Game.Content.Load<Texture2D>("buttonhover");
                        fontcolor = Color.White;
                    }
                }
                else
                {
                    fontcolor = Color.Black;
                    b.textureImage = Game.Content.Load<Texture2D>("buttonnorm");
                }

            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            // Draw the buttons
            
            string text = "Feed";
            spriteBatch.DrawString(font, text,
                new Vector2(20, 20), fontcolor, 0.0f, Vector2.Zero, 1.0f, spriteEffects, 1.0f);
            text = "Pet";
            spriteBatch.DrawString(font, text,
                new Vector2(20, 20+50), fontcolor, 0.0f, Vector2.Zero, 1.0f, spriteEffects, 1.0f);
            // Draw the buttons
            foreach(Sprite b in buttons)
            {
                b.Draw(gameTime, spriteBatch);
            }
            //b.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
