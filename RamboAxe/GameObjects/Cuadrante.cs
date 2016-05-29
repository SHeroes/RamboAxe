using AlumnoEjemplos.Ramboaxe;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class Cuadrante
    {
        List<GameObjectAbstract> cuadrantObjects;
        int latitud; 
        int longitud;
        Vector3[][] puntas;
        private int temperatura;
        private HeightMapTerrain terrain;
        public Cuadrante(bool randomizedCuadrante, int width, int height, int latitud, int longitud)
        {
            terrain = new HeightMapTerrain();
            string terrainHm = GuiController.Instance.AlumnoEjemplosDir+ "Ramboaxe\\Media\\" + "hm_plain_border.jpg";
            terrain.loadTexture(terrainHm);
            terrain.loadHeightmap(terrainHm, 10f, 0.7f, new Vector3(((float)(latitud + 0.5f) * (width/10)), 0, (longitud + 0.5f) * (height/10)));
            //terrain.loadPlainHeightmap(100, 100, 50, 100, 1, new Vector3(500, 0, 500));

            setTempratura((latitud + longitud) % 10); //el resto de la division por 10

            this.latitud = latitud;
            this.longitud = longitud;
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
                        if (rX.NextDouble() < 0.3)
                        {
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
                            mesh = MapaDelJuego.getGameMesh(5).clone("arbolin_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Arbol(mesh, inCuadx * 500, new Random().Next(-50, 0), inCuadz * 500);
                            go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
                        }
                        else if (rX.NextDouble() < 0.6)
                        {
                            mesh = MapaDelJuego.getGameMesh(3).clone("hacha_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Hacha(mesh, inCuadx * 500, new Random().Next(-50, 0), inCuadz * 500);
                            go.getMesh().rotateY((float)(Math.PI * (new Random().Next(2))));
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
          
        }


        public HeightMapTerrain getTerrain()
        {
            return terrain;
        }

        public int getTempratura()
        {
            return temperatura;
        }
        public void setTempratura(int temp)
        {
            this.temperatura = temp;
            switch (this.temperatura)
            {
                // {"CONGELADOR","FRIO","TEMPLADO","CALUROSO","ARDIENTE"}; 
                // {"0"         ,"1",   "2",        "3",        "4"}; 
                case 0: temperatura = 2; break; // temperaturaString = "TEMPLADO";  break;
                case 1: temperatura = 1; break; //temperaturaString = "FRIO"; break;
                case 2: temperatura = 0; break; //temperaturaString = "CONGELADOR"; break;
                case 3: temperatura = 1; break; //temperaturaString = "FRIO"; break;
                case 4: temperatura = 2; break; //temperaturaString = "TEMPLADO"; break;
                case 5: temperatura = 2; break; //temperaturaString = "TEMPLADO"; break;
                case 6: temperatura = 3; break; //temperaturaString = "CALUROSO"; break;
                case 7: temperatura = 4; break; //temperaturaString = "ARDIENTE"; break;
                case 8: temperatura = 3; break; //temperaturaString = "CALUROSO"; break;
                case 9: temperatura = 2; break; //temperaturaString = "TEMPLADO"; break;              

                default: temperatura = 2; break; //temperaturaString = "TEMPLADO"; break;
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
