using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Learning_Livestream.Rendering.GameObjects {
    public class Sphere : GameObject {
        public Color          ObjectColor = Color.White;
        public bool           Wireframe   = false;

        VertexPositionColor[] vertices; 

        int[]                 indices;
        float                 radius;
        int                   nvertices, nindices;

        VertexBuffer          vbuffer;
        IndexBuffer           ibuffer;

        // TODO: MAKE THE SPHERE PROGRESSIVE
        public Sphere(Engine eng, float Radius, Color col, bool Wireframe = false) : base(eng, "Sphere") {
            radius          = Radius;
            this.Wireframe  = Wireframe;
            
            nvertices = 90 * 90;    // 90 vertices in a circle, 90 circles in a sphere
            nindices  = nvertices * 6;

            vbuffer = new VertexBuffer  (eng.grafx(), typeof(VertexPositionNormalTexture), nvertices, BufferUsage.WriteOnly);
            ibuffer = new IndexBuffer   (eng.grafx(), IndexElementSize.SixteenBits, nindices, BufferUsage.WriteOnly);

            setupVertices();
            createIndices();

            vbuffer.SetData<VertexPositionColor>(vertices);
            ibuffer.SetData<int>(indices); }


        void setupVertices() {
            vertices        = new VertexPositionColor[nvertices];
            Vector3 center  = new Vector3(0, 0, 0);
            Vector3 rad     = new Vector3(Math.Abs(radius), 0, 0);

            // 90 circles, difference between each is 4 degrees
            for (int x = 0; x < 90; x++)  {
                float difx = 360.0f / 90.0f;

                //90 veritces, difference between each is 4 degrees 
                for (int y = 0; y < 90; y++) {
                    float dify = 360.0f / 90.0f;

                    Matrix zrot   = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify));  // Rotate vertex around z
                    Matrix yrot   = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx));  // Rotate circle around y
                    Vector3 point = Vector3.Transform(Vector3.Transform(rad, zrot), yrot);   // Transformation

                    vertices[x + y * 90] = new VertexPositionColor(point, ObjectColor); } } }

        void createIndices() {
            indices = new int[nindices];
            int i = 0;
            for (int x = 0; x < 90; x++) {
                for (int y = 0; y < 90; y++) {
                    int s1          = x == 89 ? 0 : x + 1,
                        s2          = y == 89 ? 0 : y + 1,
                        upperLeft   = (x  * 90 + y),
                        upperRight  = (s1 * 90 + y),
                        lowerLeft   = (x  * 90 + s2),
                        lowerRight  = (s1 * 90 + s2);

                    indices[i++] = upperLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerLeft;
                    indices[i++] = lowerLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerRight; } } }

        public override void OnRender(GameTime gameTime) {
            game.GraphicsDevice.SetVertexBuffer(vbuffer);

            //Turn off culling so we see both sides of our rendered 
            //  triangle
            RasterizerState rasterizerState             = new RasterizerState();
            rasterizerState.CullMode                    = CullMode.CullClockwiseFace;
            if(Wireframe) rasterizerState.FillMode      = FillMode.WireFrame;
            game.GraphicsDevice.RasterizerState         = rasterizerState;

            foreach (EffectPass pass in game.RenderingEffect.CurrentTechnique.
                    Passes) {
                pass.Apply();

                game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>
                    (PrimitiveType.TriangleList, vertices, 0, nvertices, indices, 0, indices.Length / 3); } }

        public override void OnUpdate(GameTime gameTime) { }

        
        private VertexPositionColor[] generateVerts(float Radius, float nParal, float nMerid) {
            List<VertexPositionColor> verts = new List<VertexPositionColor>();

            float r = Radius;
            float x, y, z, i, j;

            for (j = 0; j < Math.PI; j += (float)Math.PI / (nParal + 1)) {
                y = (float)(r * Math.Cos(j));

                for (i = 0; i < 2 * Math.PI; i += (float)Math.PI / 60) {
                    x = (float)(r * Math.Cos(i) * Math.Sin(j));
                    z = (float)(r * Math.Sin(i) * Math.Sin(j));

                    verts.Add(new VertexPositionColor(new Vector3(x, y, z), ObjectColor));  } }

            for (j = 0; j < Math.PI; j += (float)Math.PI / nMerid) {
                for (i = 0; i < 2 * Math.PI; i += (float)Math.PI / 60f) {
                    x = (float)(r * Math.Sin(i) * Math.Cos(j));
                    y = (float)(r * Math.Cos(i));
                    z = (float)(r * Math.Sin(j) * Math.Sin(i));
                    verts.Add(new VertexPositionColor(new Vector3(x, y, z), ObjectColor)); } }

            return verts.ToArray(); }

    }
}
