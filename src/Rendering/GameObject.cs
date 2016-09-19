using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGame_Learning_Livestream.Rendering {
    public abstract class GameObject {
        protected Engine game;

        protected   int         id;
        protected   string      name_object;
        protected   string      name_type;

        protected   Vector3     _position = Vector3.Zero;
        protected   Vector3     _rotation = Vector3.Zero;

        public GameObject (Engine eng, string Type) {
            game = eng;

            id = eng.RegisterObjectID();
            name_type = Type;
            name_object = name_type + "_" + id;
        }

        public abstract void OnUpdate(GameTime gameTime);
        public abstract void OnRender(GameTime gameTime);

        public Vector3 Position {
            get { return this._position; }
            set { this._position = value; }
        }

        public Vector3 Rotation {
            get { return this._rotation; }
            set { this._rotation = value; }
        }

        public string   ObjectType  { get { return this.name_type; } }
        public string   Name        { get { return this.name_object; } }
        public int      ID          { get { return this.id; } }

    }
}
