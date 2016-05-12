using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class Cuadrante
    {
        List<TgcMesh> meshes;
        int x; 
        int z; 
        public Cuadrante(bool randomizedCuadrante,int width,int height,int x,int z)
        {
            this.x = x;
            this.z = z;
            this.meshes = new List<TgcMesh>();
            TgcMesh mesh = MapaDelJuego.getGameMesh(0).clone("aMesh");
            mesh.Position = new Microsoft.DirectX.Vector3(width/2, 0,height/2);
            mesh.updateBoundingBox();
            this.meshes.Add(mesh);

        }
        public void removeMesh(TgcMesh aMesh){
            this.meshes.Remove(aMesh);
        }

        public List<TgcMesh> getMeshes()
        {
            return this.meshes;
        }

        public List<TgcMesh> getObjects()
        {
            return this.meshes;
        }
    }
}
