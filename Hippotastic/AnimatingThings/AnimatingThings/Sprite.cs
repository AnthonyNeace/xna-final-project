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

        public Texture2D buttonnorm;
        public Texture2D buttonhover;
        public Texture2D buttonpressed;
        public Point size;
        int collisionOffset;
        public Vector2 position;
        public string text;
        public Color fontcolor;
        public SpriteFont font;

        public float opacity = 1.0f;
        public float _opacity = 1.0f;

    
        public Sprite(Texture2D textureImage, Vector2 position, Point size,
            int collisionOffset, SpriteFont font, Color fontcolor, string text)
        {
            this.position = position;
            this.size = size;
            this.collisionOffset = collisionOffset;
            this.font = font;
            this.fontcolor = fontcolor;
            this.text = text;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                (int)position.X + collisionOffset,
                (int)position.Y + collisionOffset,
                size.X - (collisionOffset * 2),
                size.Y - (collisionOffset * 2));
            }
        }
    }
}
