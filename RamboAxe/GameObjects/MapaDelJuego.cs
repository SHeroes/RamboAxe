﻿using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class MapaDelJuego
    {
        
        Dictionary<String,Dictionary<String,Cuadrante>> cuadrantes;
        int widthCuadrante, heightCuadrante;
        TgcSceneLoader loader = new TgcSceneLoader();
        static List<TgcMesh> gameMeshes=new List<TgcMesh>();
        public MapaDelJuego(int _widthCuadrante,int _heightCuadrante){
             cuadrantes = new Dictionary<String,Dictionary<String,Cuadrante>>();
             
             this.widthCuadrante = _widthCuadrante;
             this.heightCuadrante = _heightCuadrante;//Group_237
             string meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\dispenser\\DispenserAgua-TgcScene.xml";
             //Dispose de escena anterior
             //Cargar escena con herramienta TgcSceneLoader
             TgcScene scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                 mesh.Scale = new Vector3(0.66f, 0.66f, 0.66f);
                 mesh.updateBoundingBox();
                 gameMeshes.Add(mesh);
             }
             meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\Energy-TgcScene.xml";
             scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                 mesh.Scale = new Vector3(0.3f, 0.3f, 0.3f);
                 mesh.updateBoundingBox();
                 gameMeshes.Add(mesh);
             }
             meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\metal-TgcScene.xml";
             scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                 mesh.Scale = new Vector3(2.0f, 2.0f, 2.0f);
                 mesh.updateBoundingBox();
                 gameMeshes.Add(mesh);
             //TODO 



            //DESDE ACÁ
            //Esto se puede copiar y pegar con otro xml para tener mas objetos que crear en el cuadrante.
       
             }
             meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\axefloor2-TgcScene.xml";
             scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                 mesh.Scale = new Vector3(0.50f, 0.50f, 0.50f);
                 mesh.updateBoundingBox();
                 gameMeshes.Add(mesh);
             }

             meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\ball-TgcScene.xml";
             scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                 mesh.Scale = new Vector3(2.0f, 2.0f, 2.0f);
                 mesh.updateBoundingBox();
                 gameMeshes.Add(mesh);
             }

             meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\ArbolBosque-TgcScene.xml";
             scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                 mesh.Scale = new Vector3(0.4f, 0.4f, 0.4f);
                 mesh.updateBoundingBox();
                 gameMeshes.Add(mesh);
             }
             meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\ruins_portal\\ruins_portal-TgcScene.xml";
             scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                 mesh.Scale = new Vector3(3f,3f, 3f);
                 mesh.updateBoundingBox();
                 gameMeshes.Add(mesh);
             }
             meshFile = TgcViewer.GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Roca\\Roca-TgcScene.xml";
             scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                 mesh.Scale = new Vector3(1.0f, 1.0f, 1.0f);
                 mesh.updateBoundingBox();
                 gameMeshes.Add(mesh);
             }
             meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\lenias-TgcScene.xml";
             scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                 mesh.Scale = new Vector3(1.0f, 1.0f, 1.0f);
                 mesh.updateBoundingBox();
                 gameMeshes.Add(mesh);
             }
             meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\HealthKit-TgcScene.xml";
             scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                mesh.Scale = new Vector3(0.6f, 0.6f, 0.6f);
                mesh.updateBoundingBox();
                gameMeshes.Add(mesh);
             }

            //meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\cabin-TgcScene.xml";
            //Mesh bueno para cuando se cree una casa a través de una receta.

             //Lenias prendidas, se podria hacer algo como:
                // Se forma receta con Leñas + algun elemento x y se puede poner tirar al escenario
                // Si estas cerca -> Te baja el frio.
             /*meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\leniasencendidas-TgcScene.xml";
             scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                 mesh.Scale = new Vector3(1.0f, 1.0f, 1.0f);
                 mesh.updateBoundingBox();
                 gameMeshes.Add(mesh);
             }*/

            //Hasta ACA

        }
        public static TgcMesh getGameMesh(int number)
        {
            return gameMeshes[number]; 
        }
        public Cuadrante getCuadrante(int x, int z)
        {
            
            if (cuadrantes.Any(posX => posX.Key == x.ToString()))
            {

                if (cuadrantes[x.ToString()].Any(posZ => posZ.Key == z.ToString()))
                {
                    return cuadrantes[x.ToString()][z.ToString()];
                }
                else
                {
                    cuadrantes[x.ToString()][z.ToString()]= new Cuadrante(true,widthCuadrante,heightCuadrante,(int)x,(int)z);
                }
            }
            else
            {
                cuadrantes[x.ToString()]=new Dictionary<string,Cuadrante>();
                cuadrantes[x.ToString()][z.ToString()] = new Cuadrante(true,widthCuadrante,heightCuadrante,(int)x,(int)z);
            }
            return cuadrantes[x.ToString()][z.ToString()];
       }
    }
}