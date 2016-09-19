using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Learning_Livestream.Rendering {
    class Floor {

        public void Initialize  (GraphicsDevice graphicsDevice) {
            floorVerts = new VertexPositionTexture[6];

            floorVerts[0].Position = new Vector3(-20, -20, 0);
            floorVerts[1].Position = new Vector3(-20, 20, 0);
            floorVerts[2].Position = new Vector3(20, -20, 0);

            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(20, 20, 0);
            floorVerts[5].Position = floorVerts[2].Position;

            effect = new BasicEffect(graphicsDevice);
        }

        VertexPositionTexture[] floorVerts;
        BasicEffect             effect;

        private void SetupCamera() {

        }

        public void DrawGround(GraphicsDevice g) {

        }
    }
}
