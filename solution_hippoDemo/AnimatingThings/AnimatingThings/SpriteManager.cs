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


namespace AnimatingThings
{

    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
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
                    }
                    else b.textureImage = Game.Content.Load<Texture2D>("buttonhover");
                }
                else b.textureImage = Game.Content.Load<Texture2D>("buttonnorm");
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
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
