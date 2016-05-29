using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using Microsoft.DirectX;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    public abstract class GameObjectAbstract
    {
        protected TgcMesh mesh;
        int x;
        int y;
        int z;
        public float delayUso
        {
            get;
            set;
        }
        public GameObjectAbstract(TgcMesh mesh,int x, int y,int z)
        {
            this.mesh = mesh;
            this.x = x;
            this.y = y;
            this.z = z;
            this.delayUso = 0;
        }
        public int getX()
        {
            return this.x;
        }
        public int getY(){
            return this.y;
        }
        public TgcMesh getMesh()
        {
            return this.mesh;
        }
        public int getZ()
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
