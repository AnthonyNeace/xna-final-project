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
        string text;

        public TextInterface(Game game)
            : base(game)
        {
            parent = (Game1)game;                       // Associate Game1 with parent
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
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
                    parent.spriteBatch.DrawString(parent.startFont,
                        "UKY    PETS",
                        new Vector2((parent.Window.ClientBounds.Width / 2) - 200, 60f),
                        Color.White,
                        0,
                        Vector2.Zero,
                        1,
                        SpriteEffects.None,
                        1);
                    parent.spriteBatch.DrawString(parent.descriptionFont, "Hold enter to begin!", new Vector2(295, 400), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    parent.spriteBatch.End();
                    break;
                // INSTRUCTION TEXT
                case Game1.GameState.Instructions:
                    parent.spriteBatch.Begin();
                    parent.spriteBatch.DrawString(parent.startFont, "UKY    PETS", new Vector2(parent.Window.ClientBounds.Width / 2 - 200, 60), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    parent.spriteBatch.DrawString(parent.descriptionFont, "Controls:  ", new Vector2(20, 200), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    parent.spriteBatch.DrawString(parent.descriptionFont, "           M  -  Enter 'Rock, Paper, Scissors' Minigame", new Vector2(20, 220), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    parent.spriteBatch.DrawString(parent.descriptionFont, "           H  -  Return from minigame", new Vector2(20, 240), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);                    
                    parent.spriteBatch.DrawString(parent.descriptionFont, "           Mouse    -  Look, Select Actions", new Vector2(20, 260), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    parent.spriteBatch.DrawString(parent.descriptionFont, "Game:", new Vector2(20, 280), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    parent.spriteBatch.DrawString(parent.descriptionFont, "        You've decided to get a pet today!", new Vector2(20, 300), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    parent.spriteBatch.DrawString(parent.descriptionFont, "        You can win food and items for your pet in minigames!", new Vector2(20, 320), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    parent.spriteBatch.DrawString(parent.descriptionFont, "        Win minigames to unlock new actions with your pet!", new Vector2(20, 340), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    parent.spriteBatch.DrawString(parent.descriptionFont, "Hold enter to begin!", new Vector2(295, 400), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    parent.spriteBatch.End();
                    break;
                // INGAME TEXT
                case Game1.GameState.InHome:
                    parent.spriteBatch.Begin();
                    //string text;
                    text = "To play Rock Paper Scissors, Press M";
                    parent.spriteBatch.DrawString(parent.font, text,
                        new Vector2(20, 20 + 375), fontcolor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
                    text = "To return Home, Press H";
                    parent.spriteBatch.DrawString(parent.font, text,
                        new Vector2(20, 20 + 400), fontcolor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
                    parent.spriteBatch.End();
                    break;
                // GAMEOVER TEXT
                case Game1.GameState.InMiniGame:
                    parent.spriteBatch.Begin();
                    text = "To play Rock Paper Scissors, Press M";
                    parent.spriteBatch.DrawString(parent.font, text,
                        new Vector2(20, 20 + 375), fontcolor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
                    text = "To return Home, Press H";
                    parent.spriteBatch.DrawString(parent.font, text,
                        new Vector2(20, 20 + 400), fontcolor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
                    parent.spriteBatch.End();
                    break;

            }

            base.Draw(gameTime);

        }

    }
}
