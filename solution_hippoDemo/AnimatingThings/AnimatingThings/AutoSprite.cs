using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace xnaPetGame
{
    class AutoSprite: Sprite
    {
        //int width = GlobalClass.ScreenWidth;
        //int height = GlobalClass.ScreenHeight;


        public AutoSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, SpriteFont font, Color fontcolor, string text)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, font, fontcolor, text)
        {
            //default construcotr
        }

        public AutoSprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, SpriteFont font, Color fontcolor, string text)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame, font, fontcolor, text)
        {
            //constructor with msPerFrame
        }

        public override Vector2 direction
        {
            get { return speed; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //int MaxX = GlobalClass.ScreenWidth - frameSize.X;
            //int MinX = 0;
            //int MaxY = GlobalClass.ScreenHeight - frameSize.Y;
            //int MinY = 0;

            //position += direction;

            //position.X = (int)Math.Round((position.X / frameSize.X)) * frameSize.X;
            //position.Y = (int)Math.Round((position.Y / frameSize.Y)) * frameSize.Y;

            //if (position.X > MaxX)
            //{
            //    speed.X *= -1;
            //    position.X = MaxX;
            //}

            //else if (position.X < MinX)
            //{
            //    speed.X *= -1;
            //    position.X = MinX;
            //}

            //if (position.Y > MaxY)
            //{
            //    speed.Y *= -1;
            //    position.Y = MaxY;
            //}

            //else if (position.Y < MinY)
            //{
            //    speed.Y *= -1;
            //    position.Y = MinY;
            //}

            base.Update(gameTime, clientBounds);
        }
    }
}
