﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Learning_Livestream.Rendering.GameObjects {
    class Cube : GameObject {

        // The dumb and ugly way.
        static Vector3[] g_vertex_buffer_data = {
            new Vector3(-1.0f,-1.0f,-1.0f), // triangle 1 : begin
            new Vector3(-1.0f,-1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f), // triangle 1 : end
            new Vector3(1.0f, 1.0f,-1.0f),  // triangle 2 : begin
            new Vector3(-1.0f,-1.0f,-1.0f),
            new Vector3(-1.0f, 1.0f,-1.0f), // triangle 2 : end
            new Vector3(1.0f,-1.0f, 1.0f),  new Vector3(-1.0f,-1.0f,-1.0f),
            new Vector3(1.0f,-1.0f,-1.0f),  new Vector3(1.0f, 1.0f,-1.0f),
            new Vector3(1.0f,-1.0f,-1.0f),  new Vector3(-1.0f,-1.0f,-1.0f),
            new Vector3(-1.0f,-1.0f,-1.0f), new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f,-1.0f), new Vector3(1.0f,-1.0f, 1.0f),
            new Vector3(-1.0f,-1.0f, 1.0f), new Vector3(-1.0f,-1.0f,-1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(-1.0f,-1.0f, 1.0f),
            new Vector3(1.0f,-1.0f, 1.0f),  new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f,-1.0f,-1.0f),  new Vector3(1.0f, 1.0f,-1.0f),
            new Vector3(1.0f,-1.0f,-1.0f),  new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f,-1.0f, 1.0f),  new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f,-1.0f),  new Vector3(-1.0f, 1.0f,-1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),  new Vector3(-1.0f, 1.0f,-1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(1.0f,-1.0f, 1.0f)
        }; static Vector3[] g_color_buffer_data = {
            new Vector3(0.583f,  0.771f,  0.014f), new Vector3(0.609f,  0.115f,  0.436f),
            new Vector3(0.327f,  0.483f,  0.844f), new Vector3(0.822f,  0.569f,  0.201f),
            new Vector3(0.435f,  0.602f,  0.223f), new Vector3(0.310f,  0.747f,  0.185f),
            new Vector3(0.597f,  0.770f,  0.761f), new Vector3(0.559f,  0.436f,  0.730f),
            new Vector3(0.359f,  0.583f,  0.152f), new Vector3(0.483f,  0.596f,  0.789f),
            new Vector3(0.559f,  0.861f,  0.639f), new Vector3(0.195f,  0.548f,  0.859f),
            new Vector3(0.014f,  0.184f,  0.576f), new Vector3(0.771f,  0.328f,  0.970f),
            new Vector3(0.406f,  0.615f,  0.116f), new Vector3(0.676f,  0.977f,  0.133f),
            new Vector3(0.971f,  0.572f,  0.833f), new Vector3(0.140f,  0.616f,  0.489f),
            new Vector3(0.997f,  0.513f,  0.064f), new Vector3(0.945f,  0.719f,  0.592f),
            new Vector3(0.543f,  0.021f,  0.978f), new Vector3(0.279f,  0.317f,  0.505f),
            new Vector3(0.167f,  0.620f,  0.077f), new Vector3(0.347f,  0.857f,  0.137f),
            new Vector3(0.055f,  0.953f,  0.042f), new Vector3(0.714f,  0.505f,  0.345f),
            new Vector3(0.783f,  0.290f,  0.734f), new Vector3(0.722f,  0.645f,  0.174f),
            new Vector3(0.302f,  0.455f,  0.848f), new Vector3(0.225f,  0.587f,  0.040f),
            new Vector3(0.517f,  0.713f,  0.338f), new Vector3(0.053f,  0.959f,  0.120f),
            new Vector3(0.393f,  0.621f,  0.362f), new Vector3(0.673f,  0.211f,  0.457f),
            new Vector3(0.820f,  0.883f,  0.371f), new Vector3(0.982f,  0.099f,  0.879f)
        };

        //public Color CubeColor;
        public float Size = 1f;

        private int Count = 0;

        private VertexBuffer vertexBuffer;

        public Cube(Engine eng, float Size) : base(eng, nameof(Cube)) {
            //if (Col.HasValue) CubeColor = Col.Value;
            //else              CubeColor = Color.White;

            this.Size = Size;
            this.Count = g_vertex_buffer_data.Length;


            // Setup our vertexBuffer
            var Vertices = new List<VertexPositionColor>();

            for (int x = 0; x < Count; x++) {
                var 
                    vec = g_vertex_buffer_data[x];
                float 
                    vx = vec.X * Size, 
                    vy = vec.Y * Size , 
                    vz = vec.Z * Size;

                Vertices.Add(new VertexPositionColor(
                    new Vector3(vx,vy,vz),
                    Color.FromNonPremultiplied(new Vector4(g_color_buffer_data[x], 1f))));
            }

            vertexBuffer = new VertexBuffer(game.GraphicsDevice,
                            typeof(VertexPositionColor), g_vertex_buffer_data.Length,
                            BufferUsage.WriteOnly);

            vertexBuffer.SetData<VertexPositionColor>(Vertices.ToArray());
        }
    

        public override void OnRender(GameTime gameTime) {
            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullClockwiseFace;
            game.GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in game.RenderingEffect.CurrentTechnique.
                    Passes) {
                pass.Apply();

                game.GraphicsDevice.DrawPrimitives
                    (PrimitiveType.TriangleList, 0, Count);
            }
        }

        public override void OnUpdate(GameTime gameTime) {
            
        }
    }
}
