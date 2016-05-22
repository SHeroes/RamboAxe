using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class Cuadrante
    {
        List<GameObjectAbstract> cuadrantObjects;
        int x; 
        int z;
        private int temperatura;
        private string temperaturaString;
        public string getTempratura()
        {
            return temperaturaString;
        }
        public void setTempratura(int temp)
        {
            this.temperatura = temp;
            switch (this.temperatura)
            {
                case 0: temperaturaString = "TEMPLADO"; break;
                case 1: temperaturaString = "FRIO"; break;
                case 2: temperaturaString = "CONGELADOR"; break;
                case 3: temperaturaString = "FRIO"; break;
                case 4: temperaturaString = "TEMPLADO"; break;
                case 5: temperaturaString = "TEMPLADO"; break;
                case 6: temperaturaString = "CALUROSO"; break;
                case 7: temperaturaString = "ARDIENTE"; break;
                case 8: temperaturaString = "CALUROSO"; break;
                case 9: temperaturaString = "TEMPLADO"; break;              

                default: temperaturaString = "TEMPLADO"; break;
            }
        }
        public Cuadrante(bool randomizedCuadrante,int width,int height,int x,int z)
        {
            setTempratura( (x + z)%10); //el resto de la division por 10

            this.x = x;
            this.z = z;
            this.cuadrantObjects = new List<GameObjectAbstract>();
            Random rX = new Random();
            
            for (int inCuadx = 1; inCuadx < 4; inCuadx++)
            {
                for (int inCuadz = 1; inCuadz < 4; inCuadz++)
                {
                    if (rX.NextDouble() > 0.5)
                    {
                        
                        TgcMesh mesh;
                        GameObjectAbstract go;
                        if (rX.NextDouble() < 0.3) {
                            mesh = MapaDelJuego.getGameMesh(1).clone("comida_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Racion(mesh, inCuadx * 500, 0, inCuadz * 500);
                            go.getMesh().rotateY((float)(Math.PI * (new Random().Next(2))));
                        }
                        else if (rX.NextDouble() < 0.4)
                        {
                            mesh = MapaDelJuego.getGameMesh(2).clone("metal_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Obstaculo(mesh, inCuadx * 500, new Random().Next(-50, 0), inCuadz * 500);
                            go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));


                        }
                        else if (rX.NextDouble() < 0.5)
                        {
                            mesh = MapaDelJuego.getGameMesh(4).clone("arbolin_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Arbol(mesh, inCuadx * 500, new Random().Next(-50, 0), inCuadz * 500);
                            go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));


                        }
                        else
                        {
                            mesh = MapaDelJuego.getGameMesh(0).clone("agua_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Dispencer(mesh, inCuadx * 500, 0, inCuadz * 500);
                            go.getMesh().rotateY((float)(Math.PI * (new Random().Next(2))));
                        }

                        this.cuadrantObjects.Add(go);
                    }
                }
            }
          /*  int randomX = new Random().Next(-500, 500);
            int randomZ = new Random().Next(-500, 500);
            TgcMesh amesh = MapaDelJuego.getGameMesh(3).clone("groundBall" + x.ToString() + z.ToString());
            amesh.Scale = new Vector3(10f, 0.3f, 10f);
            amesh.updateBoundingBox();
            GameObjectAbstract ago = new BallGround(amesh, 1250+randomX, -50, 1250+randomZ);
            
            ago.getMesh().rotateY((float)(Math.PI * (new Random().Next(2))));
            this.cuadrantObjects.Add(ago);*/
 
 
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
