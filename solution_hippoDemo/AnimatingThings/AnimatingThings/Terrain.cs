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

    public class Terrain : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Game1 parent;
        SpriteBatch spriteBatch;

        // vertices and indices
        VertexPositionNormalTexture[] surf;
        Int16[] _indices;

        private float angle = 0f;
        private int terrainWidth = 4;
        private int terrainHeight = 3;
        private float[,] heightData;
        int[] indices;
        VertexPositionColor[] vertices;

        //texture used on the terrain
        Texture2D tex;

        BasicEffect be;

        // phase of the ripple function
        float phase = 0.0f;

        // grid size
        int sizeX, sizeY;

        public Terrain(Game game)
            : base(game)
        {
            parent = (Game1)game;

            be = new BasicEffect(parent.GraphicsDevice);


        }

        public override void Initialize()
        {

            Texture2D heightMap = parent.Content.Load<Texture2D>("Untitled");
            LoadHeightData(heightMap);
            // initialize the vertices 

            // load the texture 
            tex = parent.Content.Load<Texture2D>("grass-texture-2");
            sizeX = 100;
            sizeY = 100;
            createTerrain();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        //Populates the vertices for the primitives
        private void SetUpVertices()
        {
            vertices = new VertexPositionColor[terrainWidth * terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int y = 0; y < terrainHeight; y++)
                {
                    vertices[x + y * terrainWidth].Position = new Vector3(x, heightData[x, y], -y);
                    vertices[x + y * terrainWidth].Color = Color.White;
                }
            }
        }

        //Populates the indicies of the primitives
        private void SetUpIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainHeight - 1) * 6];
            int counter = 0;
            for (int y = 0; y < terrainHeight - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + 1) + y * terrainWidth;
                    int topLeft = x + (y + 1) * terrainWidth;
                    int topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }
        }

        //Loads the height from the texture colors
        private void LoadHeightData(Texture2D heightMap)
        {
            terrainWidth = heightMap.Width;
            terrainHeight = heightMap.Height;

            Color[] heightMapColors = new Color[terrainWidth * terrainHeight];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainHeight];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainHeight; y++)
                {
                    heightData[x, y] = 0;
                }
        }

        //Builds the terrain based on array populated in SetUpVertices,SetUpIndices, and LoadHeightData
        public void createTerrain()
        {

            _indices = new Int16[(sizeX - 1) * (sizeY - 1) * 6];
            int accum = 0;
            for (int z = 0; z < sizeY - 1; z++)
            {
                for (int x = 0; x < sizeX - 1; x++)
                {
                    Int16 index = (Int16)(x + z * sizeY);
                    Int16 indexR = (Int16)((x + 1) + z * sizeY);
                    Int16 indexUR = (Int16)((x + 1) + (z + 1) * sizeY);
                    Int16 indexUL = (Int16)(x + (z + 1) * sizeY);

                    _indices[accum++] = index;
                    _indices[accum++] = indexUR;
                    _indices[accum++] = indexR;

                    _indices[accum++] = index;
                    _indices[accum++] = indexUL;
                    _indices[accum++] = indexUR;

                }
            }

            surf = new VertexPositionNormalTexture[sizeX * sizeY];

            // accum will be used for vectorizing the grid
            accum = 0;
            for (int z = 0; z < sizeY; z++) // notice the order of loop nesting is important - be consistent
            {
                for (int x = 0; x < sizeX; x++)
                {

                    // compute distance for the damping term
                    float distX = x - sizeX / 2;
                    float distY = z - sizeY / 2;
                    float dist = (float)Math.Sqrt((double)(distX * distX + distY * distY));

                    surf[accum].Position = new Vector3((float)(x - sizeX / 2), heightData[x, z], (float)(-z + sizeY / 2));
                    surf[accum].TextureCoordinate = new Vector2((float)x / (float)sizeX, (float)z / (float)sizeY);
                    accum++;
                }
            }

            // compute normal vectors
            for (int z = 1; z < sizeY - 1; z++) // consistency 
            {
                for (int x = 1; x < sizeX - 1; x++)
                {
                    Int16 index = (Int16)(x + z * sizeY);
                    Int16 indexR = (Int16)((x + 1) + z * sizeY);
                    Int16 indexUR = (Int16)((x + 1) + (z + 1) * sizeY);

                    Int16 indexL = (Int16)((x - 1) + z * sizeY);
                    Int16 indexLL = (Int16)((x - 1) + (z - 1) * sizeY);
                    Int16 indexD = (Int16)((x) + (z - 1) * sizeY);
                    Int16 indexU = (Int16)((x) + (z + 1) * sizeY);

                    // compute normals for the neighborhood of 6 triangles 
                    Vector3 n1 = Vector3.Cross(surf[index].Position - surf[indexL].Position, surf[index].Position - surf[indexU].Position);
                    Vector3 n2 = Vector3.Cross(surf[index].Position - surf[indexU].Position, surf[index].Position - surf[indexUR].Position);
                    Vector3 n3 = Vector3.Cross(surf[index].Position - surf[indexUR].Position, surf[index].Position - surf[indexR].Position);
                    Vector3 n4 = Vector3.Cross(surf[index].Position - surf[indexR].Position, surf[index].Position - surf[indexD].Position);
                    Vector3 n5 = Vector3.Cross(surf[index].Position - surf[indexD].Position, surf[index].Position - surf[indexLL].Position);
                    Vector3 n6 = Vector3.Cross(surf[index].Position - surf[indexLL].Position, surf[index].Position - surf[indexL].Position);
                    // compute their average
                    Vector3 normal = (n1 + n2 + n3 + n4 + n5 + n6) / 6;

                    normal = Vector3.Normalize(normal);
                    // set the vertex normal
                    surf[index].Normal = normal;

                }
            }

        }


        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            be.Projection = parent.c.proj;
            be.View = parent.c.view;
            be.World = Matrix.Identity;

            // enable and set texture for this BE
            be.TextureEnabled = true;
            be.Texture = tex;

            // enable lighting
            be.LightingEnabled = true;

            // set a directional light
            be.DirectionalLight0.Enabled = true;
            be.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, 1, 1));
            //be.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
            be.DirectionalLight0.SpecularColor = Color.White.ToVector3();

            // apply vertex/pixel shaders
            be.Techniques[0].Passes[0].Apply();


            // disable culling
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            //rs.FillMode = FillMode.WireFrame; //Wireframe for debugging
            parent.GraphicsDevice.RasterizerState = rs;

            // draw mesh
            parent.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, surf, 0, (sizeX) * (sizeY), _indices, 0, (sizeX - 1) * (sizeY - 1) * 2);
            
            base.Draw(gameTime);
        }

    }
}