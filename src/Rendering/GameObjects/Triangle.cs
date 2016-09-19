using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Learning_Livestream.Rendering.GameObjects {
    class Triangle : GameObject {
        
        VertexBuffer          vertexBuffer;

        public Triangle(Engine eng) : base(eng, nameof(Triangle)) {
            VertexPositionColor[] vertices;

            //Geometry  - a simple triangle about the origin
            vertices    = new VertexPositionColor[3];
            vertices[0] = new VertexPositionColor(
                             new Vector3( 0, 20, 0 ),  Color.Red);

            vertices[1] = new VertexPositionColor(
                             new Vector3(-20, -20, 0), Color.Green);

            vertices[2] = new VertexPositionColor(
                             new Vector3(20, -20, 0),   Color.Blue);

            // Vert buffer
            vertexBuffer = new VertexBuffer(game.GraphicsDevice, 
                            typeof(VertexPositionColor), 3, 
                            BufferUsage.WriteOnly);

            vertexBuffer.SetData<VertexPositionColor>(vertices); }
        
        public override void OnRender(GameTime gameTime) {
            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            //Turn off culling so we see both sides of our rendered 
            //  triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullClockwiseFace;
            game.GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in game.RenderingEffect.CurrentTechnique.
                    Passes) {
                pass.Apply();

                game.GraphicsDevice.DrawPrimitives
                    (PrimitiveType.TriangleList, 0, 3);
            }
        }

        public override void OnUpdate(GameTime gameTime) { }
    }
}
