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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        // view matrix
        public Matrix view, proj;

        // camera position 
        public Vector3 position = new Vector3(0.0f, 50.0f, 200.0f);

        // Vector3 used to store yaw, pitch and roll (roll is not used)
        public Vector3 angle = new Vector3();

        // scale factor for movement speed
        private float speed = 0.5f;
        // scale factor for turning speed
        private float turnSpeed = 90f;

        int centerX = 0, centerY = 0;

        // store previous mouse state
        MouseState prevMouseState;

        // parent class
        Game1 parentgame;

        public Camera(Game game)
            : base(game)
        {
            parentgame = (Game1)game;
            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            // Set mouse position and do initial get state

            centerX = Game.Window.ClientBounds.Width / 2;
            centerY = Game.Window.ClientBounds.Height / 2;

            proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, parentgame.Window.ClientBounds.Width / parentgame.Window.ClientBounds.Height, 0.001f, 1000.0f);

            angle.Y = 0;// MathHelper.PiOver4 / 2; // used for initialization to look at both frame and model

            prevMouseState = Mouse.GetState();
            
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2,
                Game.Window.ClientBounds.Height / 2);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            centerX = Game.Window.ClientBounds.Width / 2;
            centerY = Game.Window.ClientBounds.Height / 2;



            MouseState mouse = Mouse.GetState();

            if (angle.X <= MathHelper.ToRadians(-30.0f))
            {
                angle.X = MathHelper.ToRadians(-30.0f);
            }
            else if (angle.X >= MathHelper.ToRadians(30.0f))
            {
                angle.X = MathHelper.ToRadians(30.0f);
            }

            if (angle.Y <= MathHelper.ToRadians(-30.0f))
            {
                angle.Y = MathHelper.ToRadians(-30.0f);
            }
            else if (angle.Y >= MathHelper.ToRadians(30.0f))
            {
                angle.Y = MathHelper.ToRadians(30.0f);
            }

            if (mouse.MiddleButton == ButtonState.Pressed) { base.Update(gameTime); return; }

            // update yaw and pitch angles
            angle.X = MathHelper.ToRadians((mouse.Y - centerY) * turnSpeed * 0.0001f); // pitch
            //angle.Y += MathHelper.ToRadians((mouse.X - centerX) * turnSpeed * 0.001f); // yaw
            angle.Y = MathHelper.ToRadians((mouse.X - centerX) * turnSpeed * 0.0001f);

            // compute forward and side vectors (they are orthogonal to each other)
            Vector3 forward = Vector3.Transform(new Vector3(0, 0f, -1f), Matrix.CreateRotationX(-angle.X) * /**/Matrix.CreateRotationY(-angle.Y) );
            Vector3 left = Vector3.Transform(new Vector3(-1f, 0, 0f), Matrix.CreateRotationX(-angle.X) * /**/Matrix.CreateRotationY(-angle.Y));
            
            // handle keyboard input
            //KeyboardState keyboard = Keyboard.GetState();
            //if (keyboard.IsKeyDown(Keys.S))
            //    position -= forward * speed;
            //if (keyboard.IsKeyDown(Keys.W))
            //    position += forward * speed;

            //if (keyboard.IsKeyDown(Keys.A))
            //    position += left * speed;
            //if (keyboard.IsKeyDown(Keys.D))
            //    position -= left * speed;



            // compute view matrix
            view = Matrix.Identity;
            view *= Matrix.CreateTranslation(-position);
            view *= Matrix.CreateRotationZ(angle.Z);
            view *= Matrix.CreateRotationY(angle.Y);
            view *= Matrix.CreateRotationX(angle.X);

            // store old mouse position
            prevMouseState = mouse;
            // reset position of mouse to center of window
            //Mouse.SetPosition(centerX, centerY);

            base.Update(gameTime);
        }
    }
}
