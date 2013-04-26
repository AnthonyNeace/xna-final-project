/*
 *  Anthony Neace
 *  CS485-003
 *  University of Kentucky
 *  TextInterface.cs Class
 *  Description:
 *      - Handles text interface through various game states
 *      - All game text should be in this class.
 *      - Fonts loaded in Game1.cs with most other game content.
 */


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
    class TextInterface : Microsoft.Xna.Framework.DrawableGameComponent
    {

        Game1 parent;
        Color fontcolor = Color.Black;
        //string text;
        Texture2D logo;

        public TextInterface(Game game)
            : base(game)
        {
            parent = (Game1)game;                       // Associate Game1 with parent
            logo = game.Content.Load<Texture2D>(@"hippotastic");


        }

        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {

            //enter.Update(gameTime);
            base.Update(gameTime);
        }

        // Most content in this class will be here, drawn.
        public override void Draw(GameTime gameTime)
        {
            switch (parent.currentState)
            {
                // Splash Screen Text
                case Game1.GameState.Start:
                    // Main Screen Text
                    parent.spriteBatch.Begin();
                    parent.spriteBatch.Draw(logo, new Vector2(parent.Window.ClientBounds.Width / 2 - 400, -40), new Rectangle(0,0,800,300), Color.White);
                    parent.spriteBatch.End();
                    
                    break;
                // INSTRUCTION TEXT
                case Game1.GameState.Instructions:
                    parent.spriteBatch.Begin();
                    parent.spriteBatch.Draw(logo, new Vector2(parent.Window.ClientBounds.Width / 2 - 400, -40), new Rectangle(0, 0, 800, 300), Color.White);
                    parent.spriteBatch.DrawString(parent.descriptionFont, "You've decided to adopt a hippo!\n" +
                        "Keep your pet hippo happy by playing minigames and earning points!\n" +
                        "The more points you earn, the more things you can buy!\n",
                        new Vector2(20, 200), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    parent.spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);

        }

    }
}
