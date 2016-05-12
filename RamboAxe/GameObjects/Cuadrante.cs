using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class Cuadrante
    {
        List<GameObjectAbstract> cuadrantObjects;
        int x; 
        int z;
        public Cuadrante(bool randomizedCuadrante,int width,int height,int x,int z)
        {
            this.x = x;
            this.z = z;
            this.cuadrantObjects = new List<GameObjectAbstract>();
            Random rX = new Random();
            
            for (int inCuadx = 0; inCuadx < 5; inCuadx++)
            {
                for (int inCuadz = 0; inCuadz < 5; inCuadz++)
                {
                    if (rX.NextDouble() > 0.5)
                    {
                        TgcMesh mesh;
                        GameObjectAbstract go;
                        if (rX.NextDouble() > 0.5)
                        {
                            mesh = MapaDelJuego.getGameMesh(0).clone("agua_" +inCuadx.ToString()+inCuadz.ToString());
                            go = new Dispencer(mesh, inCuadx * 500, 100, inCuadz * 500);
                            
                        }
                        else
                        {
                           mesh = MapaDelJuego.getGameMesh(1).clone("comida_"+inCuadx.ToString()+inCuadz.ToString());
                           go = new Racion(mesh, inCuadx * 500, 0, inCuadz * 500);
                        }
                        go.delayUso = 5f;
                        this.cuadrantObjects.Add(go);
                    }
                }
            }
 
 
        }
        public void removeMesh(GameObjectAbstract anObject){
            this.cuadrantObjects.Remove(anObject);
        }


        public List<GameObjectAbstract> getObjects()
        {
            return this.cuadrantObjects;
        }
    }
}
