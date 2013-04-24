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


namespace xnaPetGame
{

    public class Hippo : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Game1 parent;
        Model hippo;
        AnimationPlayer animationPlayer;
        public AnimationClip clip;
        public SkinningData skinningData;
        public bool restart;

        Random r = new Random();

        SoundEffect hipposound;
        SoundEffect birds;

        public Texture2D texture;

        public string[] clips = {
            "Dance",
            "Headshake",
            "Jump",
            "Looking",
            "Stomplong",
            "Wiggleface",
            "Grazing"
        };

        public enum AnimationState
        {
            Dance,
            Headshake,
            Jump,
            Looking,
            Stomplong,
            Wiggleface,
            Grazing
        }
        public AnimationState previousaction = AnimationState.Dance;
        public AnimationState currentaction = AnimationState.Dance;
        public string currentclip;
        public Matrix worldMatrix;

        bool animating = false;

        public Texture2D[] textures = new Texture2D[4];

        public Hippo(Game game)
            : base(game)
        {
            parent = (Game1)game;
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            hippo = parent.Content.Load<Model>(@"hippo/pet_hippo");
            textures[0] = parent.Content.Load<Texture2D>(@"hippo/hippo_blueeyes_default");
            textures[1] = parent.Content.Load<Texture2D>(@"hippo/hippo_blueeyes_pink");
            textures[2] = parent.Content.Load<Texture2D>(@"hippo/hippo_blueeyes_blue");
            textures[3] = parent.Content.Load<Texture2D>(@"hippo/hippo_blueeyes_camo");

            texture = textures[0];

            skinningData = hippo.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);

            // Set clip to an animation track as written in the .fbx file
            currentclip = clips[0];
            clip = skinningData.AnimationClips[currentclip];

            // initialize animation
            animationPlayer.StartClip(clip);
            animating = true;

            hipposound = parent.Content.Load<SoundEffect>(@"sound\hipposound");
            birds = parent.Content.Load<SoundEffect>(@"sound\birds");

        }

        private void playbackgroundsounds(){
            if (r.Next(600) == 0)
            {
                hipposound.Play();
            }
            if (r.Next(1000) == 0)
            {
                birds.Play(0.3f,0.0f,0.0f);
            }
        }

        public override void Update(GameTime gameTime)
        {

            switch (parent.currentState)
            {
                case Game1.GameState.Instructions:
                    playbackgroundsounds();
                    break;
                case Game1.GameState.Home:
                    playbackgroundsounds();
                    break;
                case Game1.GameState.Start:
                    playbackgroundsounds();
                    break;
            }


            if (animating)  // Process animation
            {
                animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);

                Matrix[] boneTransforms = new Matrix[hippo.Bones.Count];
                animationPlayer.GetBoneTransforms().CopyTo(boneTransforms, 0);

                animationPlayer.UpdateWorldTransforms(Matrix.Identity, boneTransforms);
                animationPlayer.UpdateSkinTransforms();
            }
            else //Stop animation at current frame
            {
                Matrix[] boneTransforms = new Matrix[hippo.Bones.Count];
            }

            if (parent.happiness >= 75)
            {
                currentaction = AnimationState.Dance;
            }
            else if (parent.happiness >= 50)
            {
                currentaction = AnimationState.Jump;
            }
            else if (parent.happiness >= 25)
            {
                currentaction = AnimationState.Looking;
            }
            else currentaction = AnimationState.Headshake;

            worldMatrix = Matrix.Identity *
                Matrix.CreateScale(5.0f) *
                Matrix.CreateRotationZ(parent.r2 / 10) *
                Matrix.Identity * Matrix.CreateTranslation(0.0f, 0.0f, 0.0f) *
                Matrix.CreateRotationX((float)(Math.PI) * 1.5f);

            switch (currentaction)
            {
                case AnimationState.Headshake:
                    worldMatrix = Matrix.Identity *
                        Matrix.CreateScale(5.0f) *
                        Matrix.CreateRotationZ(-parent.r2 / 10) *
                        Matrix.Identity * Matrix.CreateTranslation(0.0f, 0.0f, -35.0f) *
                        Matrix.CreateRotationX((float)(Math.PI) * 0.5f);
                    currentclip = clips[1];
                    break;
                case AnimationState.Grazing:
                    worldMatrix = Matrix.Identity *
                        Matrix.CreateScale(5.0f) *
                        Matrix.CreateRotationZ(-parent.r2 / 10) *
                        Matrix.Identity * Matrix.CreateTranslation(0.0f, 0.0f, -35.0f) *
                        Matrix.CreateRotationX((float)(Math.PI) * 0.5f);
                    currentclip = clips[6];
                    break;
                case AnimationState.Dance:
                    currentclip = clips[0];
                    break;
                case AnimationState.Jump:
                    currentclip = clips[2];
                    break;
                case AnimationState.Looking:
                    currentclip = clips[3];
                    break;
                case AnimationState.Stomplong:
                    currentclip = clips[4];
                    break;
                case AnimationState.Wiggleface:
                    currentclip = clips[5];
                    break;
            }
            //worldMatrix *= Matrix.Identity * Matrix.CreateScale(5.0f * (parent.happiness / 100));

            previousaction = currentaction;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            //clip = skinningData.AnimationClips[currentclip];
            if (restart == true)
            {
                clip = skinningData.AnimationClips[currentclip];
                restart = !restart;
                animationPlayer.StartClip(clip);
            }
            Matrix[] bones = animationPlayer.GetSkinTransforms();

            // Render the skinned mesh.
            foreach (ModelMesh mesh in hippo.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);
                    effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    effect.Texture = texture;
                    effect.World = worldMatrix;
                    effect.View = parent.c.view;
                    effect.Projection = parent.c.proj;
                    effect.EnableDefaultLighting();

                }

                mesh.Draw();
            }

            base.Draw(gameTime);

        }
    }
}

