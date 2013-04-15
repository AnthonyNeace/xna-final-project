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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model m; Camera c;
        Matrix[] modelTransforms;
        Matrix[] originalTransforms;

        float r1 = 0, r2 = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //m = Content.Load<Model>("test");
            m = Content.Load<Model>("hippo7");
            

            modelTransforms = new Matrix[m.Bones.Count];
            originalTransforms = new Matrix[m.Bones.Count];
            
            m.CopyBoneTransformsTo(originalTransforms);


            c = new Camera(this);
            Components.Add(c);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            r1 = MathHelper.Pi * 1.5f;
            r2 += 0.04f;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);



            //draw model
            Matrix rotmat1 = Matrix.CreateRotationX(r1) * originalTransforms[0];
            m.Bones[0].Transform = rotmat1;

            //Matrix rotmat2 = Matrix.CreateRotationX(r1) * originalTransforms[1];
            //m.Bones[4].Transform = rotmat2;

            //Matrix rotmat2 = Matrix.CreateRotationY(r2) * originalTransforms[1];
            //m.Bones[1].Transform = rotmat2;

            //Matrix rotmat3 = Matrix.CreateRotationZ(r1) * originalTransforms[2];
            //m.Bones[2].Transform = rotmat3;

            //Matrix rotmat4 = Matrix.CreateRotationY(r2) * originalTransforms[0];
            //m.Bones[4].Transform = rotmat4;


            Matrix worldMatrix = Matrix.CreateScale(1f, 1f, 1f);
            m.CopyAbsoluteBoneTransformsTo(modelTransforms);


            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = modelTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(0.01f);
                    effect.View = c.view;
                    effect.Projection = c.proj;
                }
                mesh.Draw();
            }



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
