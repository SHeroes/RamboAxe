using AlumnoEjemplos.Ramboaxe;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    public class Cuadrante
    {
        List<GameObjectAbstract> cuadrantObjects;
        int latitud; 
        int longitud;
        public static string HeightMapStyleTextureTerrain = "";
        string hgMapTexture; 
        private int temperatura;
        private HeightMapTerrain terrain;
        private TgcBoundingBox boundingBox;
        public TgcBoundingBox getBoundingBox()
        {
            return this.boundingBox;
        }
        public int getLatitud()
        {
            return latitud;
        }
        public int getLongitud()
        {
            return longitud;
        }
        public Color getColor()
        {
            return this.color;
        }
        private Color color;
        public Cuadrante(bool randomizedCuadrante, int width, int height, int latitud, int longitud)
        {
            boundingBox = new TgcBoundingBox(new Vector3(latitud*width,0,longitud*height),new Vector3(latitud*width+width,1000,longitud*height+height));
            terrain = new HeightMapTerrain();
            Random rX = new Random();
            //hardocdeado para la prueba tomas las texturas de la carpeta textureMap\\green 
            HeightMapStyleTextureTerrain = "green";

            string terrainHm = GuiController.Instance.AlumnoEjemplosDir + "Ramboaxe\\Media\\";
            if (rX.NextDouble() < 0.2)
            {
                hgMapTexture = terrainHm + "textureMap\\" + HeightMapStyleTextureTerrain +"\\hm1.jpg";
                terrainHm = terrainHm + "hm1.jpg";

            }else if (rX.NextDouble() < 0.4)
            {
                hgMapTexture = terrainHm + "textureMap\\" + HeightMapStyleTextureTerrain + "\\hm2.jpg";
                terrainHm = terrainHm + "hm2.jpg";
            }else if (rX.NextDouble() < 0.56)
            {
                hgMapTexture = terrainHm + "textureMap\\" + HeightMapStyleTextureTerrain + "\\hm3.jpg";
                terrainHm = terrainHm + "hm3.jpg";
            }
            else if (rX.NextDouble() < 0.61)
            {
                hgMapTexture = terrainHm + "textureMap\\" + HeightMapStyleTextureTerrain + "\\hm4.jpg";
                terrainHm = terrainHm + "hm4.jpg";
            }
            else if (rX.NextDouble() < 0.76)
            {
                hgMapTexture = terrainHm + "textureMap\\" + HeightMapStyleTextureTerrain + "\\hm5.jpg";
                terrainHm = terrainHm + "hm5.jpg";
            }
            else 
            {
                hgMapTexture = terrainHm + "textureMap\\" + HeightMapStyleTextureTerrain + "\\hm6.jpg";
                terrainHm = terrainHm + "hm6.jpg";
            }

            terrain.loadTexture(hgMapTexture);
            terrain.loadHeightmap(terrainHm, 10f, 0.7f, new Vector3(((float)(latitud + 0.5f) * (width/10)), 0, (longitud + 0.5f) * (height/10)));
            //terrain.loadPlainHeightmap(100, 100, 50, 100, 1, new Vector3(500, 0, 500));
            setTemperatura((latitud + longitud) % 10); //el resto de la division por 10
            
            this.latitud = latitud;
            this.longitud = longitud;
            this.cuadrantObjects = new List<GameObjectAbstract>();
            float y;
            
            getTerrain().interpoledHeight(100 + boundingBox.PMin.X, 100 + boundingBox.PMin.Z, out y);
            GameObjectAbstract go = new JugoGo(100 + boundingBox.PMin.X, y, 100 + boundingBox.PMin.Z);
            this.cuadrantObjects.Add(go);

            getTerrain().interpoledHeight(400 + boundingBox.PMin.X, 500 + boundingBox.PMin.Z, out y);
            go = new Racion(400 + boundingBox.PMin.X, y, 500 + boundingBox.PMin.Z);
            this.cuadrantObjects.Add(go);

            getTerrain().interpoledHeight(700 + boundingBox.PMin.X, 700 + boundingBox.PMin.Z, out y);
            go = new ArbolCristalGo(700 + boundingBox.PMin.X, y, 700 + boundingBox.PMin.Z);
            this.cuadrantObjects.Add(go);

            getTerrain().interpoledHeight(550 + boundingBox.PMin.X, 550 + boundingBox.PMin.Z, out y);
            go = new ArbolCalorGo(550 + boundingBox.PMin.X, y, 550 + boundingBox.PMin.Z);
            this.cuadrantObjects.Add(go);

            getTerrain().interpoledHeight(700 + boundingBox.PMin.X, 550 + boundingBox.PMin.Z, out y);
            go = new ArbolHongoGo(700 + boundingBox.PMin.X, y, 550 + boundingBox.PMin.Z);
            this.cuadrantObjects.Add(go);

            
            getTerrain().interpoledHeight(300 + boundingBox.PMin.X, 350 + boundingBox.PMin.Z, out y);
            go = new PinitosGo(300 + boundingBox.PMin.X, y, 350 + boundingBox.PMin.Z);
            this.cuadrantObjects.Add(go);

            getTerrain().interpoledHeight(300 + boundingBox.PMin.X, 350 + boundingBox.PMin.Z, out y);
            go = new Dispencer(350 + boundingBox.PMin.X, y, 650 + boundingBox.PMin.Z);
            this.cuadrantObjects.Add(go);

            
            getTerrain().interpoledHeight(800 + boundingBox.PMin.X, 500 + boundingBox.PMin.Z, out y);
            go = new ParteNave1(800 + boundingBox.PMin.X, y, 500 + boundingBox.PMin.Z);
            this.cuadrantObjects.Add(go);

            getTerrain().interpoledHeight(200 + boundingBox.PMin.X, 800 + boundingBox.PMin.Z, out y);
            go = new ParteNave2(200 + boundingBox.PMin.X, y, 800 + boundingBox.PMin.Z);
            this.cuadrantObjects.Add(go);
            
            //go = new Arbol(440 + latitud * width, y, 300 + longitud * height);
           // this.cuadrantObjects.Add(go);
           /*  mesh = MapaDelJuego.getGameMesh(1).clone("comida_1");
             go = new Racion(mesh, (float)(300), 0, (float)(200));
             go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
             this.cuadrantObjects.Add(go);
            /* mesh = MapaDelJuego.getGameMesh(2).clone("comida_2");
             go = new Obstaculo(mesh, (float)(100), 0, (float)(100));
             go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
             this.cuadrantObjects.Add(go);
             mesh = MapaDelJuego.getGameMesh(2).clone("comida_3");
             go = new Obstaculo(mesh, (float)(900), 0, (float)(900));
             go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
             this.cuadrantObjects.Add(go);
             mesh = MapaDelJuego.getGameMesh(2).clone("comida_4");
             go = new Obstaculo(mesh, (float)(100), 0, (float)(900));
             go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
             this.cuadrantObjects.Add(go);
             mesh = MapaDelJuego.getGameMesh(2).clone("comida_");
             go = new Obstaculo(mesh, (float)(900), 0, (float)(100));
             go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
             this.cuadrantObjects.Add(go);*/

            /*
            for (int inCuadx = 1; inCuadx < 4; inCuadx++)
            {
                for (int inCuadz = 1; inCuadz < 4; inCuadz++)
                {
                        go = null;
                       if (rX.NextDouble() < 0.3)
                        {
                            mesh = MapaDelJuego.getGameMesh(1).clone("comida_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Racion(mesh, inCuadx , 0, inCuadz);
                            go.getMesh().rotateY((float)(Math.PI * (new Random().Next(2))));
                        }
                        else if (rX.NextDouble() < 0.32)
                        {
                            mesh = MapaDelJuego.getGameMesh(2).clone("metal_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Obstaculo(mesh, inCuadx , new Random().Next(-50, 0), inCuadz);
                            go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
                        }
                        else if (rX.NextDouble() < 0.45)
                        {
                            mesh = MapaDelJuego.getGameMesh(5).clone("arbolin_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Arbol(mesh, (float)(inCuadx ), new Random().Next(-50, 0), (float)(inCuadz ));
                            
                        }
                        else if (rX.NextDouble() < 0.48)
                        {
                            mesh = MapaDelJuego.getGameMesh(7).clone("piedra_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Piedra(mesh, inCuadx, new Random().Next(-50, 0), inCuadz);
                            go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
                        }
                       else if (rX.NextDouble() < 0.49)
                        {
                            mesh = MapaDelJuego.getGameMesh(6).clone("ruina_portal" + inCuadx.ToString() + inCuadz.ToString());
                            go = new RuinaPortal(mesh, inCuadx, new Random().Next(-50, 0), inCuadz );
                            go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
                        }
                        else  if (rX.NextDouble() < 0.65)
                        {
                            mesh = MapaDelJuego.getGameMesh(0).clone("agua_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Dispencer(mesh, (float)(inCuadx ), 0, (float)(inCuadz ));
                            
                        }
                       if (go!= null)
                       {
                           float y = 0;
                           this.getTerrain().interpoledHeight(go.getX(), go.getZ(), out y);
                           go.getMesh().Position = new Vector3((go.getX() * width / 6) + (latitud * width), y, (go.getZ() * height / 6) + (longitud * height));
                           this.cuadrantObjects.Add(go);
                       }
                }
            }*/

        }


        public HeightMapTerrain getTerrain()
        {
            return terrain;
        }

        public int getTemperatura()
        {
            return temperatura;
        }
        public void setTemperatura(int temp)
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
            /*
            switch (temperatura)
            {
                case 0:
                    terrain.loadTexture(GuiController.Instance.AlumnoEjemplosDir + "Ramboaxe\\Media\\hm_textures\\muy_frio.jpg");
                    this.color = Color.PaleTurquoise;
                    break;
                case 1:
                    terrain.loadTexture(GuiController.Instance.AlumnoEjemplosDir + "Ramboaxe\\Media\\hm_textures\\frio.jpg");
                    this.color = Color.LightCyan;
                    break;
                case 2:
                    terrain.loadTexture(GuiController.Instance.AlumnoEjemplosDir + "Ramboaxe\\Media\\hm_textures\\normal.jpg");
                    this.color = Color.LightGreen;
                    break;
                case 3:
                    terrain.loadTexture(GuiController.Instance.AlumnoEjemplosDir + "Ramboaxe\\Media\\hm_textures\\caluroso.jpg");
                    this.color = Color.Orange;
                    break;
                case 4:
                    terrain.loadTexture(GuiController.Instance.AlumnoEjemplosDir + "Ramboaxe\\Media\\hm_textures\\muy_caluroso.jpg");
                    this.color = Color.OrangeRed;
                    break;
            }
             */ 
        }

        public void removeMesh(GameObjectAbstract anObject){
            this.cuadrantObjects.Remove(anObject);
        }


        public List<GameObjectAbstract> getObjects()
        {
            return this.cuadrantObjects;
        }
        public void dispose()
        {
            
            foreach (GameObjectAbstract go in cuadrantObjects)
            {
                go.dispose();
            }
            terrain.dispose();
        }
    }
}
