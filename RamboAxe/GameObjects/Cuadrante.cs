﻿using Microsoft.DirectX;
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
                    //TgcMesh arbolMesh;
                    //GameObjectAbstract arbolObject;
                    //int offset = 350;
                    /*for (int i = 1; i < 2; i++)
                    {
                        arbolMesh = MapaDelJuego.getGameMesh(5).clone("arbolin_" + inCuadx.ToString() + inCuadz.ToString() + i.ToString());
                        //arbolObject = new Arbol(arbolMesh, (inCuadx+(i-1)) * (300), new Random().Next(-50, 0), (inCuadz +(i-1)) * (300));
                        //arbolObject = new Arbol(arbolMesh, (inCuadx * (450) +(200 *i) ), new Random().Next(-50, 0), (inCuadz *(450) + (200*i)));
                        arbolObject = new Arbol(arbolMesh, inCuadx * offset, new Random().Next(-50, 0), inCuadz * offset);
                        arbolObject.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
                        /*if (!this.colisionaConOtroObjeto(arbolObject))
                        {
                            this.cuadrantObjects.Add(arbolObject);
                        }*/
                    //    this.cuadrantObjects.Add(arbolObject);*/
                    //}
                        //Crear cierta cantidad de arboles en cada zona. Y en lo posible, dar la sensación de pegados, como si fuese zona de arboles.
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
                            else if (rX.NextDouble() < 0.51)
                            {
                                mesh = MapaDelJuego.getGameMesh(8).clone("lenias_" + inCuadx.ToString() + inCuadz.ToString());
                                go = new Lenia(mesh, inCuadx * 500, 0, inCuadz * 500);
                                go.getMesh().rotateY((float)(Math.PI * (new Random().Next(2))));
                            }       

                        else if (rX.NextDouble() < 0.55)
                        {
                            mesh = MapaDelJuego.getGameMesh(7).clone("piedra_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Piedra(mesh, inCuadx * 500, 0, inCuadz * 500);
                            go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
                        }

                        else if (rX.NextDouble() < 0.56)
                            {
                                mesh = MapaDelJuego.getGameMesh(9).clone("kitsalud_" + inCuadx.ToString() + inCuadz.ToString());
                                go = new KitSalud(mesh, inCuadx * 500, 0, inCuadz * 500);
                                go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
                            }
                        else if (rX.NextDouble() < 0.6)
                        {
                            mesh = MapaDelJuego.getGameMesh(3).clone("hacha_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Hacha(mesh, inCuadx * 500, 0, inCuadz * 500);
                            go.getMesh().rotateY((float)(Math.PI * (new Random().Next(2))));
                        }
                        else if (rX.NextDouble() < 0.61)
                        {
                            mesh = MapaDelJuego.getGameMesh(6).clone("ruina_portal" + inCuadx.ToString() + inCuadz.ToString());
                            go = new RuinaPortal(mesh, 0, new Random().Next(-50, 0), inCuadz * 500);
                            go.getMesh().rotateY((float)(FastMath.ToRad(new Random().Next(-20, 20))));
                        }
                        else
                        {
                            mesh = MapaDelJuego.getGameMesh(0).clone("agua_" + inCuadx.ToString() + inCuadz.ToString());
                            go = new Dispencer(mesh, inCuadx * 500, 0, inCuadz * 500);
                            go.getMesh().rotateY((float)(Math.PI * (new Random().Next(2))));
                        }
                            /*if (!this.colisionaConOtroObjeto(go))
                            {
                                this.cuadrantObjects.Add(go);
                            }*/
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

        private bool colisionaConOtroObjeto(GameObjectAbstract go)
        {
            bool colisiona;
            colisiona = false;
            if (this.cuadrantObjects.Count > 0){
                foreach(GameObjectAbstract objetoCuadrante in this.cuadrantObjects){
                    TgcCollisionUtils.BoxBoxResult result = TgcCollisionUtils.classifyBoxBox(go.getMesh().BoundingBox, objetoCuadrante.getMesh().BoundingBox);
                    if (result == TgcCollisionUtils.BoxBoxResult.Atravesando || result == TgcCollisionUtils.BoxBoxResult.Adentro)
                    //if (result != TgcCollisionUtils.BoxBoxResult.Afuera)
                    {
                        colisiona = true;
                        go.getMesh().Scale = new Vector3(0.0f, 0.0f, 0.0f);
                        break;
                    }
                    /*if (TgcCollisionUtils.testAABBAABB(go.getMesh().BoundingBox,objetoCuadrante.getMesh().BoundingBox) && (go.getMesh().Name != objetoCuadrante.getMesh().Name)){
                        colisiona = true;
                    }*/
                    else{
                        colisiona = false;
                    }
                }
            }
            return colisiona;
            
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
