using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame_Learning_Livestream.Lib;

namespace MonoGame_Learning_Livestream.Rendering {
    public class Camera {

        [Category("Location")] [TypeConverter(typeof (VectorConverters))] public Vector3   l_Target      { get; set; } = Vector3.Zero;
        [Category("Location")] public float     Radius      { get; set; } = 100f;
        [Category("Location")] public float     Phi         { get; set; } = 0f;
        [Category("Location")] public float     Psi         { get; set; } = 0f;

        [Category("Rendering")] [TypeConverter(typeof (MatrixConverter))] public Matrix   m_Projection { get; set; }
        [Category("Rendering")] [TypeConverter(typeof (MatrixConverter))] public Matrix   m_View       { get; set; }
        [Category("Rendering")] [TypeConverter(typeof (MatrixConverter))] public Matrix   m_World      { get; set; }

        [Category("Rendering")] public float    c_Close      { get; set; } = 1f;
        [Category("Rendering")] public float    c_Far        { get; set; } = 1000f;

        private Engine game;
        public Camera(Engine game) { this.game = game; }


        private Vector3 l_Camera() {
            Vector3 l_Loc = Vector3.Zero;

            /* x = r sin θ cos φ
               y = r sin θ sin φ
               z = r cos θ        */

            float sinPhi = (float)Math.Sin(Phi);
            float x      = (float)(Radius * sinPhi * Math.Cos(Psi));
            float y      = (float)(Radius * sinPhi * Math.Sin(Psi));
            float z      = (float)(Radius * Math.Cos(Phi));

            // What is going on o_0

            return new Vector3(x, y, z);
        }
        
        public void Initialize() {
            // Setup projection matrix
            m_Projection =
                Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(45f),
                    game.GraphicsDevice.DisplayMode.AspectRatio,
                    c_Close, c_Far);

            // Setup view matrix
            m_View = Matrix.CreateLookAt(l_Camera(), l_Target,
                            new Vector3(0f, 1f, 0f));

            m_World = Matrix.Identity;
        }

        public Vector3 camLoc = Vector3.Zero;
        public void OnUpdate() {
            camLoc   = l_Camera();
            var camDir   = -camLoc;
            var right    = Vector3.Cross(camDir, Vector3.UnitY);
            var up       = Vector3.Cross(camDir, right);

            // Setup projection matrix
            m_Projection =
                Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(45f),
                    game.GraphicsDevice.DisplayMode.AspectRatio,
                    c_Close, c_Far);

            // Update the view matrix
            m_View = Matrix.CreateLookAt(camLoc, l_Target, up);
        }

        public void OnDraw() {
            game.RenderingEffect.Projection = m_Projection;
            game.RenderingEffect.View       = m_View;
            game.RenderingEffect.World      = m_World;

            // Clear the color bit 
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        // TODO: REWRITE CAMERA CONTROLS
    }
}
