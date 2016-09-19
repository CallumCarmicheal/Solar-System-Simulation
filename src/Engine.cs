using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Learning_Livestream.Rendering;

namespace MonoGame_Learning_Livestream {
    public class Engine : Game {
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;
        public BasicEffect RenderingEffect;

        int objectNextID;
        bool GrafxApplyUpdates;
        Logic.InputManager inputManager;

        // Game Objects
        Rendering.Camera Camera;
        List<GameObject> gameObjects;

        public Engine() {
            Graphics = new GraphicsDeviceManager(this);

            objectNextID = 0;
        }

        public int RegisterObjectID() {
            return objectNextID++;
        }

        public Camera GetCamera() {
            return this.Camera;
        }

        public GameObject[] GetGameObjects() {
            return this.gameObjects.ToArray();
        }

        public GraphicsDevice grafx() {
            return this.GraphicsDevice;
        }

        void Window_ClientSizeChanged(object sender, EventArgs e) {
            Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            GrafxApplyUpdates = true;
        }

        protected override void Initialize() {
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.AllowAltF4 = false;

            base.Initialize();

            // Setup our required objected for stuff to do even more stuffy stuff
            gameObjects = new List<GameObject>();

            Camera = new Rendering.Camera(this);
            Camera.Initialize();

            inputManager = new Logic.InputManager(this);

            // Setup the basic effect for 3D
            RenderingEffect = new BasicEffect(GraphicsDevice);
            RenderingEffect.Alpha = 1f;

            // Enable colors on the verts.
            RenderingEffect.VertexColorEnabled = true;

            // Enable Lighting
            //      Requires extra informatio]#[n that
            //      is not present on the VertexPosition
            //          Color
            //      TO USE LIGHTING GOOGLE !!!
            RenderingEffect.LightingEnabled = false;


            ///////// SETUP GAME OBJECTS \\\\\\\\\

            // Setup the triangle
            //gameObjects.Add((GameObject)(new Rendering.GameObjects.Triangle(this)));
            gameObjects.Add((GameObject)(new Rendering.GameObjects.Cube(this, 30)));


        }



        protected override void LoadContent() {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime) {
            if (GrafxApplyUpdates) Graphics.ApplyChanges();

            inputManager.PollInputs();
            /// DO OTHER CRAP HERE
            /// DO OTHER CRAP HERE
            Camera.OnUpdate();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gt) {
            Camera.OnDraw();

            foreach (var obj in gameObjects)
                obj.OnRender(gt);

            //base.Draw(gt);
        }
    }
}
