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

        MouseState currentmouse, previousmouse;

        Model m;
        Model tree;
        Model grass;

        public Camera c;
        Terrain t;
        public Hippo h;

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

        bool goingdown = false;

        public SpriteFont startFont, descriptionFont, font;

        public float r1 = 0, r2 = 0, timer = 0;
        int ctr = 0;

        TextInterface text;

        Button mainscore;
        Button happyscore;
        Button enter;

        //*** Save Game ***//
        public GameInfo gameFile;
        public String pet = "";
        public int hunger = 0;
        public int happiness = 100;
        public int score = 0;

        //*** Food items ***//
        Texture2D apple, cherries, banana;
        public Boolean eatApple = false, eatCherries = false, eatBanana = false;
        public int removeFood = 0;

        //**** MiniGames ****//
        public RPS rps;
        public Matching matching;
        Cards cards;

        //**** Game State ****//
        public enum GameState { Start, Instructions, Home, RPS, Cards, Matching };
        public GameState currentState = GameState.Start;

        int updatecounter = 0;

        // Audio
        SoundEffect audioBackground;
        SoundEffectInstance backgroundInstance;
        SoundEffect audioBackground2;
        SoundEffectInstance backgroundInstance2;
        public static SoundEffect soundfxButton;
        public static SoundEffect soundfxButton2;

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

            // Load Fonts
            startFont = Content.Load<SpriteFont>(@"fonts\Start");
            descriptionFont = Content.Load<SpriteFont>(@"fonts\Description");
            font = Content.Load<SpriteFont>(@"fonts\SpriteFont");

            String temp = "Score: " + score;

            mainscore = new Button(Content.Load<Texture2D>("widebutton"),//Texture
                new Vector2(Window.ClientBounds.Width-220, 20),//Position
                new Point(200, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                temp);

            temp = "Happiness: " + happiness;

            happyscore = new Button(Content.Load<Texture2D>("widebutton"),//Texture
                new Vector2(Window.ClientBounds.Width - 220, 70),//Position
                new Point(200, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                temp);

            mainscore.buttonhover = mainscore.buttonnorm = mainscore.buttonpressed = Content.Load<Texture2D>("widebutton");
            happyscore.buttonhover = happyscore.buttonnorm = happyscore.buttonpressed = Content.Load<Texture2D>("widebutton");

            enter = new Button(Content.Load<Texture2D>("buttonnorm"),//Texture
                new Vector2(Window.ClientBounds.Width / 2 - 50, 420),//Position
                new Point(100, 50),//Framesize
                0, //Collision Offset
                font,
                Color.Black,
                "Start!");

            enter.buttonhover = enter.buttonnorm = enter.buttonpressed = Content.Load<Texture2D>("buttonhover");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Add RPS
            rps = new RPS(this);

            //Add Matching
            matching = new Matching(this);

            //Add Cards
            cards = new Cards(this);

            //Save game to file class
            gameFile = new GameInfo(this);
            //Load in game info
            gameFile.getFile();

            //Load Food textures
            //apple = Content.Load<Texture2D>(@"Food/apple-icon");
            //cherries = Content.Load<Texture2D>(@"Food/Cherry");
            //banana = Content.Load<Texture2D>(@"Food/Banana");

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

            //Load Hippo
            h = new Hippo(this);
            h.Initialize();

            //Loading Height Texture
            Texture2D heightMap = Content.Load<Texture2D>("Untitled");

            // Load Sounds
            audioBackground = Content.Load<SoundEffect>(@"sound\background1");
            backgroundInstance = audioBackground.CreateInstance();
            backgroundInstance.Volume = 0.35f;

            audioBackground2 = Content.Load<SoundEffect>(@"sound\background2");
            backgroundInstance2 = audioBackground2.CreateInstance();
            backgroundInstance2.Volume = 0.35f;

            soundfxButton = Content.Load<SoundEffect>(@"sound\button");
            soundfxButton2 = Content.Load<SoundEffect>(@"sound\button2");

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
                if((p.X < -hippobox || p.X > hippobox) || (p.Y < -hippobox || p.Y > hippobox+10))
                        grasspos.Add(p);
            }

        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //Update Score
            if (score <= 0)
            {
                score = 0;
            }

            //Update Happiness
            if (happiness <= 0)
            {
                happiness = 0;
            }
            else if (happiness >= 100)
            {
                happiness = 100;
            }

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
            //int ctr = 0;

            currentmouse = Mouse.GetState();

            switch (currentState)
            {
                case GameState.Start:
                    if (backgroundInstance.State != SoundState.Playing)
                        backgroundInstance.Play();
                    // Delay for keyboard input
                    // If you hold enter, proceed to next game state
                    if (enter.collisionRect.Contains(currentmouse.X, currentmouse.Y) &&
                            currentmouse.LeftButton == ButtonState.Pressed &&
                            previousmouse.LeftButton == ButtonState.Released)
                    {
                        currentState = GameState.Instructions;
                        enter.text = "Continue";
                    }
                    enter.Update(gameTime);
                    h.worldMatrix = Matrix.Identity *
                        Matrix.CreateScale(5.0f) *
                        Matrix.CreateRotationZ(r2) *
                        Matrix.Identity * Matrix.CreateTranslation(0.0f, 0.0f, 0.0f) *
                        Matrix.CreateRotationX((float)(Math.PI) * 1.5f);
                    spriteManager.Update(gameTime);
                    break;
                case GameState.Instructions:
                    if (backgroundInstance.State != SoundState.Playing)
                        backgroundInstance.Play();
                    if (enter.collisionRect.Contains(currentmouse.X, currentmouse.Y) &&
                            currentmouse.LeftButton == ButtonState.Pressed &&
                            previousmouse.LeftButton == ButtonState.Released)
                    {
                        currentState = GameState.Home;
                        Components.Add(spriteManager);
                    }
                    enter.Update(gameTime);
                    spriteManager.Update(gameTime);
                    break;
                case GameState.Home:
                    if (backgroundInstance.State != SoundState.Playing)
                    {
                        backgroundInstance2.Stop();
                        backgroundInstance.Play();
                    }
                    if (happiness <= 0)
                    {
                        happiness = 0;
                    }
                    else if (happiness >= 100)
                    {
                        happiness = 100;
                    }
                    timer += gameTime.ElapsedGameTime.Milliseconds;
                    if (timer >= 5000)
                    {
                        h.restart = true;
                        happiness--;
                        timer -= 5000;
                    }
                    ctr = 0;
                    spriteManager.Update(gameTime);
                    
                    mainscore.text = "Score: " + score;
                    mainscore.Update(gameTime);
                    happyscore.text = "Happiness: " + happiness + "%";
                    happyscore.Update(gameTime);
                    break;
                case GameState.RPS:
                    if (backgroundInstance2.State != SoundState.Playing)
                    {
                        backgroundInstance.Stop();
                        backgroundInstance2.Play();
                    }
                    spriteManager.Update(gameTime);
                    if (ctr > 10)
                        rps.Update(gameTime);
                    ctr++;
                    break;
                case GameState.Matching:
                    if (backgroundInstance2.State != SoundState.Playing)
                    {
                        backgroundInstance.Stop();
                        backgroundInstance2.Play();
                    }
                    spriteManager.Update(gameTime);
                    if (ctr > 10)
                        matching.Update(gameTime);
                    ctr++;
                    break;
                case GameState.Cards:
                    if (backgroundInstance2.State != SoundState.Playing)
                    {
                        backgroundInstance.Stop();
                        backgroundInstance2.Play();
                    }
                    spriteManager.Update(gameTime);
                    if(ctr > 10)
                        cards.Update(gameTime);
                    ctr++;
                    break;
            }

            h.Update(gameTime);
            updatecounter++;
            previousmouse = currentmouse;
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rs;

            switch (currentState)
            {
                case GameState.Start:
                    GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                    mainBGColor = mainScreenColorFade(mainBGColor);
                    GraphicsDevice.Clear(mainBGColor);
                    h.Draw(gameTime);
                    spriteBatch.Begin();
                    enter.Draw(gameTime, spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.Instructions:
                    spriteBatch.Begin();
                    enter.Draw(gameTime, spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.Home:
                    spriteManager.Draw(gameTime);
                    t.Draw(gameTime);
                    h.Draw(gameTime);
                    drawGrass(gameTime);
                    drawTree(gameTime);
                    spriteBatch.Begin();
                    mainscore.Draw(gameTime, spriteBatch);
                    happyscore.Draw(gameTime, spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.RPS:
                    spriteManager.Draw(gameTime);
                    rps.Draw(gameTime);
                    break;
                case GameState.Matching:
                    spriteManager.Draw(gameTime);
                    matching.Draw(gameTime);
                    break;
                case GameState.Cards:
                    spriteManager.Draw(gameTime);
                    cards.Draw(gameTime);
                    break;
            }


            base.Draw(gameTime);
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
