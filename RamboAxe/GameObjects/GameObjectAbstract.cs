using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;


namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    public abstract class GameObjectAbstract
    {
        protected TgcMesh mesh;
        float x;
        float y;
        float z;
        public float delayUso
        {
            get;
            set;
        }
        public GameObjectAbstract(TgcMesh mesh,float x, float y,float z)
        {
            this.mesh = mesh;
            this.x = x;
            this.y = y;
            this.z = z;
            this.delayUso = 0;
        }
        public float getX()
        {
            return this.x;
        }
        public float getY(){
            return this.y;
        }
        public TgcMesh getMesh()
        {
            return this.mesh;
        }
        public float getZ()
        {
            return this.z;
        }
        public abstract InteractuableResponse use();
		public void move(Vector3 vector)
        {
           
            this.getMesh().Position = vector;
        }
        public virtual void place(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
