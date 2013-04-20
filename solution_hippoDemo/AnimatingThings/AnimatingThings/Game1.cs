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
        DepthStencilState treeDSS = new DepthStencilState();
        DepthStencilState hippoDSS = new DepthStencilState();

        Model m;
        Model tree;
        Model grass;

        public Camera c;
        Terrain t;

        Matrix[] modelTransforms;
        Matrix[] originalTransforms;

        Matrix[] treemodelTransforms;
        Matrix[] treeoriginalTransforms;

        Matrix[] grassmodelTransforms;
        Matrix[] grassoriginalTransforms;

        public Matrix worldMatrix;
        Boolean colorSwitch = false;
        public Color mainBGColor = new Color(0, 0, 0);

        Random r = new Random();
        List<Point> grasspos = new List<Point>();

        public SpriteFont startFont, descriptionFont, font;

        public float r1 = 0, r2 = 0;

        TextInterface text;

        //**** MiniGame ****//
        MiniGame minigame;
        Matching matching;
        Cards cards;

        //**** Game State ****//
        public enum GameState { Start, Instructions, InHome, RPS };
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
                    currentState = GameState.RPS;
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
            cards = new Cards(this);
            Components.Add(cards);

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

            //Load Grass
            grass = Content.Load<Model>("grass_low_poly");
            grassmodelTransforms = new Matrix[grass.Bones.Count];
            grassoriginalTransforms = new Matrix[grass.Bones.Count];
            grass.CopyBoneTransformsTo(grassoriginalTransforms);

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

            int hippobox = 25;
            for(int i = 0; i<200; i++){
                Point p = new Point(r.Next(80) - 40, r.Next(80) - 40);
                if((p.X < -hippobox || p.X > hippobox) || (p.Y < -hippobox || p.Y > hippobox))
                        grasspos.Add(p);
            }

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
                case GameState.RPS:
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

            if (Keyboard.GetState().IsKeyDown(Keys.H) == true && (currentState == GameState.RPS))
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

            //Creates Pretty Rotation Effect
            if (r2 >= 5.00f)
                goingdown = !goingdown;
            if (r2 <= -10.00f)
                goingdown = !goingdown;
            if (!goingdown)
                r2 -= 0.02f;
            else
                r2 += 0.02f;

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
                cards.Update(gameTime);
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

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            //rs.FillMode = FillMode.WireFrame; //Draws the wireframe, used for debugging
            GraphicsDevice.RasterizerState = rs;

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
                    //DepthStencilState DDS = new DepthStencilState();
                    //DDS = DepthStencilState.Default;
                    //GraphicsDevice.DepthStencilState = DDS;
                    drawHippo(gameTime);
                    t.Draw(gameTime);

                    //DDS = new DepthStencilState();
                    //DDS.DepthBufferWriteEnable = false;
                    //GraphicsDevice.DepthStencilState = DDS;
                    drawGrass(gameTime);
                    drawTree(gameTime);                    
                    break;
            }

            //RasterizerState rs = new RasterizerState();
            //rs.CullMode = CullMode.None;
            ////rs.FillMode = FillMode.WireFrame; //Draws the wireframe, used for debugging
            //GraphicsDevice.RasterizerState = rs;



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

            //GraphicsDevice.BlendState = BlendState.Opaque;
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.GraphicsDevice.BlendState = BlendState.Opaque;
                    effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                    effect.EnableDefaultLighting();
                    effect.World = worldMatrix;
                    effect.View = c.view;
                    effect.Projection = c.proj;
                }
                mesh.Draw();
            }
        }

        // Draws the tree.
        void drawTree(GameTime gameTime)
        {

            Matrix rotmat1 = Matrix.CreateRotationX(r1) * treeoriginalTransforms[0];
            tree.Bones[0].Transform = rotmat1;
            foreach(Point p in grasspos)
            {
                int hippobox = 38;
                if ((p.Y < -hippobox))
                {


                    worldMatrix = Matrix.Identity *
                        Matrix.CreateRotationX((float)(Math.PI) * 1.5f) *
                        Matrix.CreateRotationY(p.X) *
                        Matrix.CreateScale(10.0f) *
                        Matrix.CreateTranslation(p.X, -2.0f, p.Y) *
                        Matrix.CreateRotationY(r2 / 10);
                    //c.view = Matrix.CreateLookAt(c.position, Vector3.Zero, Vector3.Up);


                    tree.CopyAbsoluteBoneTransformsTo(treemodelTransforms);

                    //AlphaTestEffect e = (AlphaTestEffect)e

                    foreach (ModelMesh mesh in tree.Meshes)
                    {
                        foreach (AlphaTestEffect effect in mesh.Effects)
                        {
                            //effect.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                            effect.Texture = Content.Load<Texture2D>("treetexture");
                            effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                            effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                            //effect.EnableDefaultLighting();
                            effect.World = worldMatrix;
                            effect.View = c.view;
                            effect.Projection = c.proj;
                        }
                        mesh.Draw();
                    }
                }
            }
        }

        //Draws the grass.
        void drawGrass(GameTime gameTime)
        {

            Matrix rotmat1 = Matrix.CreateRotationX(r1) * grassoriginalTransforms[0];
            grass.Bones[0].Transform = rotmat1;
            foreach(Point pos in grasspos)
            {
                worldMatrix = Matrix.Identity *
                    //Matrix.CreateRotationX((float)(Math.PI)) *
                    Matrix.CreateRotationY(pos.X) *
                    //Matrix.CreateRotationY(i) *
                    Matrix.CreateScale(5.0f) *
                    Matrix.CreateTranslation(pos.X, 7.0f, pos.Y) *
                    Matrix.CreateRotationY(r2 / 10);
                //c.view = Matrix.CreateLookAt(c.position, Vector3.Zero, Vector3.Up);


                grass.CopyAbsoluteBoneTransformsTo(grassmodelTransforms);


                foreach (ModelMesh mesh in grass.Meshes)
                {
                    
                    foreach (AlphaTestEffect effect in mesh.Effects)
                    {
                        effect.Texture = Content.Load<Texture2D>("low_poly_grass2mag");
                        effect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                        effect.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
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
