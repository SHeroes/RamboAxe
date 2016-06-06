using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using System.Drawing;
using TgcViewer.Utils.TgcGeometry;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    public class MapaDelJuego
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

            // TODO: pasar todos a mesh manager

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

             meshFile = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\bol-TgcScene.xml";
             scene = loader.loadSceneFromFile(meshFile);
             foreach (TgcMesh mesh in scene.Meshes)
             {
                 mesh.Scale = new Vector3(2.0f, 2.0f, 2.0f);
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
            //Hasta ACA             
        }
       
        public Vector2 getCuadranteCoordsFor(int x,int z)
        {
            Vector2 cuadrante = new Vector2((int)(Math.Floor((float)x / widthCuadrante) ),(int) (Math.Floor((float)z / heightCuadrante)));
            return cuadrante;

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
        public void placeObject(GameObjectAbstract go)
        {
            if(go != null){
                Vector3 worldPosition = go.getMesh().Position;
                Vector3 relativePosition = getCuadranteRelativePosition(worldPosition);
                go.place(
                    (int)Math.Ceiling(relativePosition.X),
                    (int)Math.Ceiling(relativePosition.Y),
                    (int)Math.Ceiling(relativePosition.Z)
                );
                getCuadranteForPosition(worldPosition).getObjects().Add(go);
                EjemploAlumno.getInstance().forceUpdate = true;
            }
        }

        public Cuadrante getCuadranteForPosition(Vector3 position)
        {
            int x = (int) Math.Floor(position.X / widthCuadrante);
            int z = (int) Math.Floor(position.Z / widthCuadrante);
            return getCuadrante(x, z);
        }
        private Vector3 getCuadranteRelativePosition(Vector3 position)
        {
            Vector3 posiciones = new Vector3(0.0f, 0.0f, 0.0f);
            int cuadranteX = (int) Math.Floor(position.X / widthCuadrante);
            int cuadranteZ = (int) Math.Floor(position.Z / widthCuadrante);
            posiciones.X = Math.Abs(position.X) - Math.Abs(cuadranteX * widthCuadrante);
            posiciones.Y = position.Y;
            posiciones.Z = Math.Abs(position.Z) - Math.Abs(cuadranteZ * widthCuadrante);
            return posiciones;
        }
    }
}