using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace xnaPetGame
{
    class Button: Sprite
    {

        SoundEffect soundfxButtonHover;
        SoundEffect soundfxButtonClick;
        bool justEntered;
        bool justClicked;

        public Button(Texture2D textureImage, Vector2 position, Point size,
            int collisionOffset, SpriteFont font, Color fontcolor, string text)
            : base(textureImage, position, size, collisionOffset, font, fontcolor, text)
        {
            soundfxButtonHover = Game1.soundfxButton;
            soundfxButtonClick = Game1.soundfxButton2;
            justEntered = false;
            //default constructor
        }

        public override void Update(GameTime gameTime)
        {
            //updates sprite image
            MouseState mouse = Mouse.GetState();

            if (collisionRect.Contains(mouse.X, mouse.Y))
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    // Play button click sound
                    if (!justClicked && soundfxButtonClick != null)
                    {
                        soundfxButtonClick.Play();
                        justEntered = true;
                        justClicked = true;
                    }
                    _opacity = 0.0f;
                    fontcolor = Color.White;
                }
                else
                {
                    // Play button hover sound
                    if (!justEntered && soundfxButtonHover != null)
                    {
                        soundfxButtonHover.Play();
                        justEntered = true;
                    }
                    _opacity = 1.0f;
                    if (opacity >= 0.0f) opacity -= 0.08f;
                    fontcolor = Color.White;
                    justClicked = false;
                }
            }
            else
            {
                justEntered = false;
                justClicked = false;
                _opacity = 1.0f;
                if (opacity <= 1.0f) opacity += 0.03f;
                fontcolor = Color.Black;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw Normal Button
            spriteBatch.Draw(buttonnorm, //Texture
            position,
            new Rectangle(0, 0, (int)(size.X), (int)(size.Y)), //Button Size
            Color.White * opacity, //Button Alpha Value
            0, //Rotation
            Vector2.Zero, //Origin
            1f, //Scale
            SpriteEffects.None,
            0.75f); //Z-Order

            //Draw Hover button
            spriteBatch.Draw(buttonhover, //Texture
            position,
            new Rectangle(0, 0, (int)(size.X), (int)(size.Y)), //Button Size
            Color.White * _opacity, //Button Alpha Value
            0, //Rotation
            Vector2.Zero, //Origin
            1f, //Scale
            SpriteEffects.None,
            0.5f); //Z-Order

            //Draw Pressed button
            spriteBatch.Draw(buttonpressed, //Texture
            position,
            new Rectangle(0, 0, (int)(size.X), (int)(size.Y)), //Button Size
            Color.White, //Button Alpha Value
            0, //Rotation
            Vector2.Zero, //Origin
            1f, //Scale
            SpriteEffects.None,
            0.25f); //Z-Order

            //Draw button text
            spriteBatch.DrawString(font, //Specified font
                text, //String to be printed
                new Vector2((int)((position.X + size.X / 2) - (font.MeasureString(text).X / 2) - 2),//X positon on button
                    (int)((position.Y + size.Y / 2) - (font.MeasureString(text).Y / 2) - 4)), //Y Positon on button
                Color.Lerp(Color.White, Color.Black, opacity), //Allows for color fade
                0.0f, //rotation 
                Vector2.Zero, //origin 
                1.0f, //scale
                SpriteEffects.None,
                1.0f); //Z-order

        }
    }
}
