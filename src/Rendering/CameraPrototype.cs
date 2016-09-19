using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using wfrm = System.Windows.Forms;

namespace MonoGame_Learning_Livestream.Rendering {
    public class CameraPrototype {

        [Category("Location")] public Vector3 camTarget    { get; set; }
        [Category("Location")] public Vector3 camPosition  { get; set; }

        [Category("Rendering")] public Matrix  m_Projection { get; set; }
        [Category("Rendering")] public Matrix  m_View       { get; set; }
        [Category("Rendering")] public Matrix  m_World      { get; set; }
        [Category("Rendering")] public float clipping_Close { get; set; } = 1f;
        [Category("Rendering")] public float clipping_Far   { get; set; } = 1000f;
        
        [Category("Sensitivty")] [DisplayName("Mouse - Look: Vertical")]
        public float mouse_Sensitivity_Vertical             { get; set; } = 1f;

        [Category("Sensitivty")] [DisplayName("Mouse - Look: Horizontal")]
        public float mouse_Sensitivity_Horizontal           { get; set; } = 1f;

        [Category("Sensitivty")] [DisplayName("Mouse - Look Area: Horizontal")]
        public float mouse_Sensitivity_LookAt_H             { get; set; } = 0.01f;

        [Category("Sensitivty")] [DisplayName("Mouse - Look Area: Vertical")]
        public float mouse_Sensitivity_LookAt_V             { get; set; } = 0.01f;

        [Category("Sensitivty")] [DisplayName("Mouse - Zoom")]
        public float mouse_Sensitivity_Scroll               { get; set; } = 1f;


        private Engine game;
        public CameraPrototype(Engine game) {
            this.game = game;
        }

        public void Initialize() {
            camTarget = new Vector3();
            camPosition = new Vector3(0f, 0f, -100f);


            m_Projection =
                Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(45f),
                    game.GraphicsDevice.DisplayMode.AspectRatio,
                    clipping_Close, clipping_Far);

            m_View = Matrix.CreateLookAt(camPosition, camTarget,
                            new Vector3(0f, 1f, 0f));// Y up

            m_World = Matrix.CreateWorld(camTarget, Vector3.
                            Forward, Vector3.Up);
        }


        bool mbtn_Left,
             mbtn_Middle,
             mbtn_Right;
        int  m_scrl = 0;

        private void OnInput() {
            // Check if the game is active and if so
            // reset the toggles and wait until it is
            if (!game.IsActive) {
                mbtn_Left   = false;
                mbtn_Middle = false;
                mbtn_Right  = false;
                return;
            } //*/

            // Declare/Setup variables

            MouseState
                    ms = Mouse.GetState();
            KeyboardState
                    ks = Keyboard.GetState();
            GamePadState
                    gs = GamePad.GetState(0);
            int     w = game.Window.ClientBounds.Width,
                    h = game.Window.ClientBounds.Height;
            bool pressedLft = (ms.LeftButton == ButtonState.Pressed),
                    pressedMdl = (ms.MiddleButton == ButtonState.Pressed),
                    pressedRht = (ms.RightButton == ButtonState.Pressed),
                    pressedMod = (ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl)),
                    // ------------------------------------
                    resetPos = (pressedMod && pressedMdl),
                    resetLoc = (!pressedMod && pressedMdl),
                    // ------------------------------------
                    movePos = (!pressedMod && pressedLft),
                    moveTarget = (pressedMod && pressedLft);
            Matrix  rMX = default(Matrix),
                    rMY = default(Matrix),
                    tM = default(Matrix);
            float   rx = 0, 
                    ry = 0;

            /* Handle default actions           */ {

                // Check if the user had pressed the mouse
                // on this frame, if so then hide the mouse
                // and set the default mouse position
                if (pressedLft) {
                    // Hide the cursor, set the xy
                    if (!mbtn_Left)
                        game.IsMouseVisible = false;

                    Mouse.SetPosition(w / 2, h / 2);

                    // Do the processing in here to save time and 
                    // space instead of copy and pasting the logic
                    int x, y;
                    //float rx, ry;
                    float modh, modv;

                    if (movePos) {
                        modh = mouse_Sensitivity_Horizontal;
                        modv = mouse_Sensitivity_Vertical;
                    } else {
                        modh = mouse_Sensitivity_LookAt_H;
                        modv = mouse_Sensitivity_LookAt_V;
                    }

                    // Set our x y values with the sensitivity
                    x = ms.X - (w / 2);
                    y = ms.Y - (h / 2);
                    rx = x * modh * -1; // Invert movements
                    ry = y * modv;

                    // Y <-> X  = FLIPPED WHAT?
                    if (movePos) {
                        rMX = Matrix.CreateRotationX(MathHelper.ToRadians(ry));
                        rMY = Matrix.CreateRotationY(MathHelper.ToRadians(rx));
                    } else {
                        var trans = new Vector3(rx, 0f, ry);
                        tM = Matrix.CreateTranslation(trans);
                    }
                } 
                
                // If the mouse is not down then
                // re-show the mouse.
                else {
                    // LEFT IS NOT PRESSED
                    game.IsMouseVisible = true;
                }
            }
            
            /* Handle camera movement           */ {
                // Move position (Left Click)
                if (movePos) {
                    camPosition = Vector3.Transform(camPosition, rMX);
                    camPosition = Vector3.Transform(camPosition, rMY);

                    Mouse.SetPosition(w / 2, h / 2);
                } 
                
                else if (moveTarget) {
                    camTarget = Vector3.Transform(camTarget, tM); // Y U NO CHANGE?

                    //System.Diagnostics.Debugger.Log(0, "Dbg", 
                    //  $"camTarget = {camTarget.ToString()}. RX:{rx}, RY:{ry}\n");

                    m_View = Matrix.CreateLookAt(camPosition, camTarget,
                                new Vector3(0f, 1f, 0f));
                    m_World = Matrix.CreateWorld(camTarget, Vector3.
                                    Forward, Vector3.Up);

                    Mouse.SetPosition(w / 2, h / 2);
                }
            }

            /* Handle middle mouse actions      */ {
                if (resetPos)       camPosition = new Vector3(camPosition.X, camPosition.Y, -100f);
                else if (resetLoc)  camTarget = new Vector3();
            }

            /* Update zooming for mouse         */ {
                if (ms.ScrollWheelValue < m_scrl)
                    camPosition += new Vector3(0, 0, -1 * mouse_Sensitivity_Scroll);

                else if (ms.ScrollWheelValue > m_scrl)
                    camPosition += new Vector3(0, 0, 1 * mouse_Sensitivity_Scroll);
            }
            
            /* Store the current button state   */ {
                m_scrl = ms.ScrollWheelValue;
                mbtn_Left = pressedLft;
                mbtn_Middle = pressedMdl;
                mbtn_Left = pressedRht;
            }
        }

        public void OnUpdate() {
            OnInput();

            m_View = Matrix.CreateLookAt(camPosition, camTarget,
                        new Vector3(0f, 1f, 0f));
        }

        public void OnDraw() {
            game.RenderingEffect.Projection = m_Projection;
            game.RenderingEffect.View = m_View;
            game.RenderingEffect.World = m_World;

            // Clear the color bit 
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}
