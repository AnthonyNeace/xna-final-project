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

using SkinnedModel;

namespace SkinnedAnimation
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Model m;
        AnimationPlayer animationPlayer;
        Camera c;
        AnimationClip clip;
        SkinningData skinningData;
        // Bool to pause and start animation
        bool animating = false;

        Texture2D[] textures = new Texture2D[4];

        // The index of the texture to use
        int currentTexture = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            c = new Camera(this);
            Components.Add(c);

            // Load model
            m = Content.Load<Model>("pet_hippo");

            // Load textures into an array
            textures[0] = Content.Load<Texture2D>("hippo_blueeyes_default");
            textures[1] = Content.Load<Texture2D>("hippo_blueeyes_pink");
            textures[2] = Content.Load<Texture2D>("hippo_blueeyes_blue");
            textures[3] = Content.Load<Texture2D>("hippo_blueeyes_camo");

            skinningData = m.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);

            // Set clip to an animation track as written in the .fbx file
            clip = skinningData.AnimationClips["Dance"];
          
            // initialize animation
            animationPlayer.StartClip(clip);
            animating = true;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Allow switching the texture
            // This is just for demonstration purposes -- in the actual program
            // we'll need to have the texture control on a hold timer or mouse
            // button click so that it doesn't quickly flitter through textures
            // like in this demo.
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                currentTexture += 1;
            }

            // Use F1-F8 to switch animations
            // Animations are written into FBX file by their animation track name.  We cna call them
            // direction in the AnimationClips array.  These are all of the animations I want to use
            // for the hippo.  Some have gotten initialized away from the hippo's actual standing location
            // for some reason, but we can just transform the hippo's location in the game to correct
            // for this.
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                clip = skinningData.AnimationClips["Dance"];
                animationPlayer.StartClip(clip);
                animating = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                clip = skinningData.AnimationClips["Headshake"];
                animationPlayer.StartClip(clip);
                animating = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                clip = skinningData.AnimationClips["Jump"];
                animationPlayer.StartClip(clip);
                animating = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F4))
            {
                clip = skinningData.AnimationClips["Looking"];
                animationPlayer.StartClip(clip);
                animating = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F5))
            {
                clip = skinningData.AnimationClips["Stomplong"];
                animationPlayer.StartClip(clip);
                animating = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F6))
            {
                clip = skinningData.AnimationClips["Wiggleface"];
                animationPlayer.StartClip(clip);
                animating = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F7))
            {
                clip = skinningData.AnimationClips["Grazing"];
                animationPlayer.StartClip(clip);
                animating = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F8))
            {
                animating = false;
            }


            if (animating)  // Process animation
            {
                animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

                Matrix[] boneTransforms = new Matrix[m.Bones.Count];
                animationPlayer.GetBoneTransforms().CopyTo(boneTransforms, 0);

                animationPlayer.UpdateWorldTransforms(Matrix.Identity, boneTransforms);
                animationPlayer.UpdateSkinTransforms();
            }
            else //Stop animation at current frame
            {
               
                Matrix[] boneTransforms = new Matrix[m.Bones.Count];
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice device = graphics.GraphicsDevice;

            device.Clear(Color.CornflowerBlue);

            Matrix[] bones = animationPlayer.GetSkinTransforms();

            // Compute camera matrices.
            Matrix view = c.view;

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                    device.Viewport.AspectRatio,
                                                                    1,
                                                                    10000);

            Matrix worldMatrix = Matrix.Identity *
                        Matrix.CreateRotationX((float)(Math.PI) * 1.5f);

            // Render the skinned mesh.
            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    // Begin to render with the specified texture
                    // I currently only have four textures, so I allow the texture to increment forever
                    // and mod it by 4 for selection.
                    effect.Texture = textures[currentTexture%4];
                    effect.World = worldMatrix;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();

                }

                mesh.Draw();
            }

            base.Draw(gameTime);

        }
    }
}
