using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using Microsoft.DirectX;

namespace AlumnoEjemplos.RamboAxe
{
    public abstract class MeshManager
    {
        private static String basePath = TgcViewer.GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\";
        private static Dictionary<String, TgcMesh> meshes;

        public static TgcMesh Arbol { get { return meshes["Arbol"]; } }

        public static void init()
        {
            meshes = new Dictionary<String, TgcMesh>();
            TgcSceneLoader loader = new TgcSceneLoader();
            // Cargar meshes
            loadSingleMesh(loader, "bol-TgcScene.xml", "Arbol");
        }

        public static void dispose()
        {
            foreach (TgcMesh mesh in meshes.Values)
            {
                mesh.dispose();
            }
            meshes.Clear();
        }

        /// <summary>
        /// Carga un mesh nuevo en memoria
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="path"></param>
        /// <param name="meshName"></param>
        private static void loadSingleMesh(TgcSceneLoader loader, String path, String meshName)
        {
            loadMeshes(
                loader,
                path, 
                new Dictionary<int, string>{ 
                    { 0, meshName } 
                }
            );
        }

        /// <summary>
        /// Carga varios meshes por nombre dado su posicion
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="path"></param>
        /// <param name="namesByPos"></param>
        private static void loadMeshes(TgcSceneLoader loader, String path, Dictionary<int, String> namesByPos)
        {
            String fullPath = basePath + path;
            TgcScene scene = loader.loadSceneFromFile(fullPath);
            int position = 0;
            foreach (TgcMesh mesh in scene.Meshes)
            {
                if(namesByPos.ContainsKey(position)){
                    String meshName = namesByPos[position];
                    mesh.Scale = new Vector3(2.0f, 2.0f, 2.0f);
                    mesh.updateBoundingBox();
                    meshes.Add(meshName, mesh);
                }
                else
                {
                    mesh.dispose();
                }
                position++;
            }
        }
    }
}
