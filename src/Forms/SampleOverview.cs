using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonoGame_Learning_Livestream.Forms {
    public partial class SampleOverview : Form {
        public SampleOverview() {
            InitializeComponent();

            Timer tim1 = new Timer();
            tim1.Tick += Tim1_Tick;
            tim1.Interval = 10;
            tim1.Start();
        }

        private void Tim1_Tick(object sender, EventArgs e) {
            try {
                var camLoc = Program.Game.GetCamera().camLoc;
                label8.Text = $"X: {camLoc.X}\nY: {camLoc.Y}\nZ: {camLoc.Z}";
            } catch { }
        }

        private Rendering.GameObject[] gameObjects;

        private void button1_Click(object sender, EventArgs e) {
            // List of the items inside the game's instance
            // First hard code in the camera object because that is always present.

            listBox1.Items.Clear();
            listBox1.Items.Add("Camera");

            gameObjects = Program.Game.GetGameObjects();

            foreach(var o in gameObjects) 
                listBox1.Items.Add($"[{o.ID} : {o.ObjectType}] {o.Name}");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            var index = listBox1.SelectedIndex;
            if(index == 0) 
                 propertyEditor.SelectedObject = Program.Game.GetCamera();
            else propertyEditor.SelectedObject = gameObjects[index-1];
        }

        private void trackBar1_Scroll(object sender, EventArgs e) {
            Program.Game.GetCamera().Radius = (trackBar1.Value);
            label4.Text = "" + trackBar1.Value;
        }

        private void trackBar2_Scroll(object sender, EventArgs e) {
            float v = ((trackBar2.Value / 10f) / 10f);
            label5.Text = ""+v;
            Program.Game.GetCamera().Phi = v;
        }

        private void trackBar3_Scroll(object sender, EventArgs e) {
            float v = ((trackBar3.Value / 10f) / 10f);
            label6.Text = "" + v;
            Program.Game.GetCamera().Psi = v;
        }
    }
}
