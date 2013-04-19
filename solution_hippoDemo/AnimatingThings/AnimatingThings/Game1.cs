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

namespace xnaPetGame
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        Model m;
        Model tree;
        public Camera c;
        Terrain t;
        Matrix[] modelTransforms;
        Matrix[] treemodelTransforms;
        Matrix[] originalTransforms;
        Matrix[] treeoriginalTransforms;
        public Matrix worldMatrix;
        Boolean colorSwitch = false;
        public Color mainBGColor = new Color(0, 0, 0);


        public SpriteFont startFont, descriptionFont, font;

        public float r1 = 0, r2 = 0;

        TextInterface text;

        //**** MiniGame ****//
        MiniGame minigame;
        Matching matching;

        //**** Game State ****//
        public enum GameState { Start, Instructions, InHome, InMiniGame };
        public GameState currentState = GameState.Start;

        public Boolean inMain = false;
        public Boolean inMini = false;
        public Boolean inMatching = true;

        int updatecounter = 0;

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
            this.IsMouseVisible = true; //shows mouse
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Fonts
            startFont = Content.Load<SpriteFont>(@"fonts\Start");
            descriptionFont = Content.Load<SpriteFont>(@"fonts\Description");
            font = Content.Load<SpriteFont>(@"fonts\SpriteFont");

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

            //Load Tree
            tree = Content.Load<Model>("tree4-fbx");
            treemodelTransforms = new Matrix[tree.Bones.Count];
            treeoriginalTransforms = new Matrix[m.Bones.Count];
            tree.CopyBoneTransformsTo(treeoriginalTransforms);

            //Loading Terrain
            t = new Terrain(this);
            t.Initialize();

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

            // Initializing Text Utility Class
            text = new TextInterface(this);
            Components.Add(text);
        }



        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        void gameStateUpdateManager(GameTime gameTime){

            // This handles the passing of components to their appropriate states.  This also
            // will eventually be where the updating of gamestates occur.
            switch(currentState){
                case GameState.Start:
                    // Delay for keyboard input
                    // If you hold enter, proceed to next game state
                    if (updatecounter == 6)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            currentState = GameState.Instructions;
                        }
                        updatecounter = 0;
                        break;
                    }
                    break;
                case GameState.Instructions:
                    if (updatecounter == 12)
                    {
                        // Delay for keyboard input
                        // If you hold enter, proceed to next game state
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            currentState = GameState.InHome;
                            Components.Add(spriteManager);
                        }
                        updatecounter = 0;
                        break;
                    }
                    break;
                case GameState.InHome:
                    keyboardControls(gameTime);
                    break;
                case GameState.InMiniGame:
                    keyboardControls(gameTime);
                    break;
            }

        }

        // Watches for keyboard input, reacts appropriately
        void keyboardControls(GameTime gameTime)
        {
            if (inMini)
            {
                spriteManager.ClearLists();
                Components.Remove(spriteManager);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.H) == true && (currentState == GameState.InMiniGame))
            {
                inMain = true;
                inMini = false;
                inMatching = false;
                //playing = true;
                currentState = GameState.InHome;
                Components.Add(spriteManager);

            }

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                inMain = false;
                inMini = false;
                inMatching = true;
                //playing = true;
            }
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            r1 = MathHelper.Pi * 1.5f;
            //r2 += 0.04f;

            gameStateUpdateManager(gameTime);

            if (inMain == true)
            {
                r1 = MathHelper.Pi * 1.5f;
                //r2 += 0.04f;

            }
            else if (inMini == true)
            {
                minigame.Update(gameTime);
            }
            //else if (inMatching == true)
            //{
            //    matching.Update(gameTime);
            //}

            updatecounter++;
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            //GraphicsDevice.BlendState = BlendState.AlphaBlend;
            
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //text.Draw(gameTime);
            //draw model
            switch (currentState)
            {
                case GameState.Start:
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    mainBGColor = mainScreenColorFade(mainBGColor);
                    GraphicsDevice.Clear(mainBGColor);
                    drawHippo(gameTime);
                    break;
                case GameState.InHome:
                    //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    t.Draw(gameTime);
                    drawTree(gameTime);
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    drawHippo(gameTime);
                    
                    break;
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

        bool goingdown = false;

        // Draws the hippo.
        void drawHippo(GameTime gameTime)
        {

            Matrix rotmat1 = Matrix.CreateRotationX(r1) * originalTransforms[0];
            m.Bones[0].Transform = rotmat1;


            //Creates Pretty Rotation Effect
            if (r2 >= 10.00f)
                goingdown = !goingdown;
            if (r2 <= -20.00f)
                goingdown = !goingdown;
            if (!goingdown)
                r2-=0.02f;
            else
                r2+=0.02f;

            switch (currentState)
            {
                case GameState.Start:
                    worldMatrix = Matrix.Identity * 
                        Matrix.CreateRotationZ(r2) * 
                        Matrix.CreateRotationX((float)(Math.PI) * 1.5f) * 
                        Matrix.CreateScale((float)(10.0f+Math.Sin(r2))) * 
                        Matrix.CreateTranslation(0.0f, -25.0f, 0.0f);
                    break;
                case GameState.InHome:
                    
                    worldMatrix = Matrix.Identity * 
                        Matrix.CreateRotationX((float)(Math.PI) * 1.5f) * 
                        Matrix.CreateScale(5.0f) * 
                        Matrix.CreateTranslation(0.0f, 0.0f, 0.0f) *
                        Matrix.CreateRotationY(r2/10);
                    //c.view = Matrix.CreateLookAt(c.position, Vector3.Zero, Vector3.Up);
                    break;
            }
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
        }

        // Draws the hippo.
        void drawTree(GameTime gameTime)
        {

            Matrix rotmat1 = Matrix.CreateRotationX(r1) * originalTransforms[0];
            tree.Bones[0].Transform = rotmat1;
            for (int i = -45; i <= 45; i += 15)
            {
                worldMatrix = Matrix.Identity *
                    Matrix.CreateRotationX((float)(Math.PI) * 1.5f) *
                    Matrix.CreateRotationY(i)*
                    Matrix.CreateScale(5.0f) *
                    Matrix.CreateTranslation(i, 0.0f, -30.0f) *
                    Matrix.CreateRotationY(r2 / 10);
                //c.view = Matrix.CreateLookAt(c.position, Vector3.Zero, Vector3.Up);


                tree.CopyAbsoluteBoneTransformsTo(modelTransforms);


                foreach (ModelMesh mesh in tree.Meshes)
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
            }
        }


        // Creates an animated gradient.
        public Color mainScreenColorFade(Color bgColor)
        {
            // Increment or decrecrement, depending on switch
            if (!colorSwitch)
                bgColor = new Color((bgColor.R + 1f) / 255, (bgColor.G + 1f) / 255, (bgColor.B + 1f) / 255);
            else
                bgColor = new Color((bgColor.R - 1f) / 255, (bgColor.G - 1f) / 255, (bgColor.B - 1f) / 255);

            // Boolean switch to switch count direction
            if (bgColor.R == 100)
                colorSwitch = true;
            else if (bgColor.R == 0)
                colorSwitch = false;

            return bgColor;
        }
    }
}
