using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace xnaPetGame
{
    abstract class Sprite
    {
        Game1 parent;
        public Texture2D buttonnorm;
        public Texture2D buttonhover;
        public Texture2D buttonpressed;
        public Point frameSize;
        Point currentFrame;
        Point sheetSize;
        protected int framedelay = 0;
        int collisionOffset;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 16;
        public Vector2 speed;
        public float opacity = 1.0f;
        public float _opacity = 1.0f;
        public Vector2 position;
        string text;
        public Color fontcolor;
        SpriteFont font;// = Game.Content.Load<SpriteFont>("Spritefont");
        SpriteEffects spriteEffects;

        public Sprite(Game game)
        {
            parent = (Game1)game;
        }
    
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, SpriteFont font, Color fontcolor, string text)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame, font, fontcolor, text)
        {
            //default constructor
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, SpriteFont font, Color fontcolor, string text)
        {
            //requires millisecondsPerFrame
            //this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.font = font;
            this.fontcolor = fontcolor;
            this.text = text;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //updates sprite image
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonnorm,
            position,
            new Rectangle(currentFrame.X * frameSize.X,
            currentFrame.Y * frameSize.Y,
            frameSize.X, frameSize.Y),
            Color.White * opacity, 0, Vector2.Zero,
            1f, SpriteEffects.None, 0.75f);

            spriteBatch.Draw(buttonhover,
            position,
            new Rectangle(currentFrame.X * frameSize.X,
            currentFrame.Y * frameSize.Y,
            frameSize.X, frameSize.Y),
            Color.White * _opacity, 0, Vector2.Zero,
            1f, SpriteEffects.None, 0.5f);

            spriteBatch.Draw(buttonpressed,
            position,
            new Rectangle(currentFrame.X * frameSize.X,
            currentFrame.Y * frameSize.Y,
            frameSize.X, frameSize.Y),
            Color.White, 0, Vector2.Zero,
            1f, SpriteEffects.None, 0.25f);

            spriteBatch.DrawString(font, text,
                new Vector2(position.X+10, position.Y+10), fontcolor, 0.0f, Vector2.Zero, 1.0f, spriteEffects, 1.0f);
        }

        public abstract Vector2 direction
        {
            get;
        }

        public int scoreValue { get; protected set; }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                (int)position.X + collisionOffset,
                (int)position.Y + collisionOffset,
                frameSize.X - (collisionOffset * 2),
                frameSize.Y - (collisionOffset * 2));
            }
        }
    }
}
