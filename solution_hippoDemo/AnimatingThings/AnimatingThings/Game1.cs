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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        Model m;
        public Camera c;
        Terrain t;
        Matrix[] modelTransforms;
        Matrix[] originalTransforms;

        float r1 = 0, r2 = 0;

        //**** MiniGame ****//
        MiniGame minigame;
        Matching matching;

        //**** Game State ****//
        public enum GameState { InHome, InMiniGame };
        GameState currentState = GameState.InHome;

        public Boolean inMain = false;
        public Boolean inMini = false;
        public Boolean inMatching = true;


        public Boolean playing
        {
            get { return inMain; }
            set
            {
                if (inMain == false)
                {
                    currentState = GameState.InMiniGame;
                }
            }
        }

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true; //shows mouse
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Add MiniGame
            minigame = new MiniGame(this);
            Components.Add(minigame);
            //matching = new Matching(this);
            //Components.Add(matching);

            //Loading Model
            m = Content.Load<Model>("hippo7");
            modelTransforms = new Matrix[m.Bones.Count];
            originalTransforms = new Matrix[m.Bones.Count];
            m.CopyBoneTransformsTo(originalTransforms);

            //Loading Terrain
            t = new Terrain(this);
            t.Initialize();
            Components.Add(t);

            //Loading Height Texture
            Texture2D heightMap = Content.Load<Texture2D>("Untitled");
            //LoadHeightData(heightMap);
            //SetUpVertices();
            //SetUpIndices();

            //Intializing Camera
            c = new Camera(this);
            Components.Add(c);

            //Initalizing Sprite Manager
            spriteManager = new SpriteManager(this);
            spriteManager.Initialize();
            Components.Add(spriteManager);

            // TODO: use this.Content to load your game content here
        }



        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            r1 = MathHelper.Pi * 1.5f;
            r2 += 0.04f;

            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                inMini = true;
                inMain = false;
                inMatching = false;
                //playing = false;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                inMain = true;
                inMini = false;
                inMatching = false;
                //playing = true;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                inMain = false;
                inMini = false;
                inMatching = true;
                //playing = true;
            }

            if (inMain == true)
            {
                r1 = MathHelper.Pi * 1.5f;
                r2 += 0.04f;

            }
            else if (inMini == true)
            {
                minigame.Update(gameTime);
            }
            //else if (inMatching == true)
            //{
            //    matching.Update(gameTime);
            //}

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            t.Draw(gameTime);

            //draw model
            Matrix rotmat1 = Matrix.CreateRotationX(r1) * originalTransforms[0];
            m.Bones[0].Transform = rotmat1;

            Matrix worldMatrix = Matrix.Identity * Matrix.CreateRotationX((float)(Math.PI) * 1.5f) * Matrix.CreateScale(10.0f) * Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            m.CopyAbsoluteBoneTransformsTo(modelTransforms);

            
            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = worldMatrix;
                    effect.View = c.view;
                    effect.Projection = c.proj;
                }
                mesh.Draw();
            }

            //RasterizerState rs = new RasterizerState();
            //rs.CullMode = CullMode.None;
            //rs.FillMode = FillMode.WireFrame; //Draws the wireframe, used for debugging
            //device.RasterizerState = rs;

            if (inMini == true)
            {
                GraphicsDevice.Clear(Color.DodgerBlue);
                minigame.Draw(spriteBatch);
            }

            //if (inMatching == true)
            //{
            //    GraphicsDevice.Clear(Color.DeepSkyBlue);
            //    matching.Draw(spriteBatch);
            //}
            base.Draw(gameTime);
        }
    }
}
