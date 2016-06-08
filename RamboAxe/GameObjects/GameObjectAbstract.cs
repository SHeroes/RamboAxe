using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;


namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    public abstract class GameObjectAbstract
    {
        protected static TgcSceneLoader loader = new TgcSceneLoader();
        protected TgcMesh mesh;
        protected List<TgcMesh> bounds;
        float x;
        float y;
        float z;
        Vector3 normalizadoOriginal;
        protected static Dictionary<String, TgcScene> scenes;
       
        public float delayUso
        {
            get;
            set;
        }

       

        public GameObjectAbstract(float x, float y, float z)
        {
            this.bounds = new List<TgcMesh>();
            if (scenes == null)
            {
                scenes = new Dictionary<string, TgcScene>();
            }
            this.x = x;
            this.y = y;
            this.z = z;
            this.delayUso = 0;
            normalizadoOriginal = new Vector3(1,1,1);
        }

        protected void loadMeshes(string meshfile){
            TgcScene scene = null;
            if (scenes.ContainsKey(meshfile))
            {
                scene = scenes[meshfile];   
            }
            else
            {
                scene = loader.loadSceneFromFile(meshfile);
                scenes.Add(meshfile, scene);
            }

            bool atleastOne = false;
            foreach (TgcMesh mesh in scene.Meshes)
            {
              
                if (mesh.Layer == "0")
                {
                    
                    this.mesh = mesh.clone(mesh.Name + " " + x + "_" + z);
                    normalizadoOriginal = new Vector3(-1f / this.mesh.BoundingBox.calculateSize().X, 1f / this.mesh.BoundingBox.calculateSize().Y, 1f / this.mesh.BoundingBox.calculateSize().Z);
                    this.mesh.Scale = normalizadoOriginal;
                    this.mesh.updateBoundingBox();
                    this.mesh.Position = new Vector3(x, y, z);
                    
                }
                else if (mesh.Layer == "Default")
                {
                    TgcMesh meshBound = mesh.clone(mesh.Name + " " + x + "_" + z);
                    atleastOne = true;
                    meshBound.Scale = normalizadoOriginal;//new Vector3(-1f / this.mesh.BoundingBox.calculateSize().X, 1f / this.mesh.BoundingBox.calculateSize().Y,1f / this.mesh.BoundingBox.calculateSize().Z);
                    meshBound.Position = new Vector3(x, y, z);
                    meshBound.updateBoundingBox();
                    this.bounds.Add(meshBound);
                }
            }
            if (!atleastOne)
            {
                this.bounds.Add(this.mesh);
            }
        }
       /* public void normalizarTamanio()
        {
            this.mesh.Scale = new Vector3(-1f / this.mesh.BoundingBox.calculateSize().X, 1f / this.mesh.BoundingBox.calculateSize().Y, 1f / this.mesh.BoundingBox.calculateSize().Z);
            this.mesh.updateBoundingBox();
            foreach (TgcMesh boundMesh in this.bounds)
            {
                boundMesh.Scale = new Vector3(-1f / boundMesh.BoundingBox.calculateSize().X, 1f / boundMesh.BoundingBox.calculateSize().Y, 1f / boundMesh.BoundingBox.calculateSize().Z);
                boundMesh.updateBoundingBox();
            }
        }*/
        public void resize(float x,float y, float z){
            this.mesh.Scale = new Vector3(normalizadoOriginal.X * x, normalizadoOriginal.Y * y, normalizadoOriginal.Z * z);
            this.mesh.updateBoundingBox();
            this.mesh.Position = new Vector3(this.x, this.y, this.z);
            foreach (TgcMesh boundMesh in this.bounds)
            {
                boundMesh.Scale = new Vector3(normalizadoOriginal.X * x, normalizadoOriginal.Y * y, normalizadoOriginal.Z * z);
                boundMesh.updateBoundingBox();
                boundMesh.Position = new Vector3(this.x, this.y, this.z);
                
            }
        }

        public GameObjectAbstract()
        {

        }
        public float getX()
        {
            return this.x;
        }
        public void setY(float y)
        {
            this.y = y;
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
        public virtual void place(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
           
        }
        public List<TgcMesh> getBounds()
        {
            return this.bounds;
        }
    }
}
