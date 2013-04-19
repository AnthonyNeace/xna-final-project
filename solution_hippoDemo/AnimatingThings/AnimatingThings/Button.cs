using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace xnaPetGame
{
    class Button: Sprite
    {

        public Button(Texture2D textureImage, Vector2 position, Point size,
            int collisionOffset, SpriteFont font, Color fontcolor, string text)
            : base(textureImage, position, size, collisionOffset, font, fontcolor, text)
        {
            //default construcotr
        }

        public override void Update(GameTime gameTime)
        {
            //updates sprite image
            MouseState mouse = Mouse.GetState();

            if (collisionRect.Contains(mouse.X, mouse.Y))
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    _opacity = 0.0f;
                    fontcolor = Color.White;
                }
                else
                {
                    _opacity = 1.0f;
                    if (opacity >= 0.0f) opacity -= 0.08f;
                    fontcolor = Color.White;
                }
            }
            else
            {
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
