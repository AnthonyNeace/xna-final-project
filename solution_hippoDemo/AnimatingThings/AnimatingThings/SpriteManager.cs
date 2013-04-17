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
                buttons.Add(new AutoSprite(Game.Content.Load<Texture2D>("buttonnorm"),
                    new Vector2(10, i+10), new Point(100, 50), 10, new Point(0, 0),
                    new Point(2, 1), new Vector2(0, 1), 0));
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            
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
            //.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
