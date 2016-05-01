using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Input;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.Collision.ElipsoidCollision;


namespace AlumnoEjemplos.Fps2
{
    /// <summary>
    /// Ejemplo EjemploMeshLoader:
    /// Unidades Involucradas:
    ///     # Unidad 3 - Conceptos Básicos de 3D - Mesh
    /// 
    /// Permite cargar una malla estática de formato TGC desde el FileSystem.
    /// Utiliza la herramienta TgcMeshLoader.
    /// Esta herramienta crea un objeto TgcScene, compuesto a su vez por N TgcMesh
    /// Cada uno representa una malla estática.
    /// La escena es cargada desde un archivo XML de formato TGC
    /// 
    /// Autor: Matías Leone, Leandro Barbagallo
    /// 
    /// </summary>
    public class Fps2 : TgcExample
    {
        TgcScene currentScene;
        string currentPath;
        TgcText2d text3;
        public bool falling = false;
        TgcBox box;
        TgcBox box2;
        private List<TgcMesh> vegetation;
        private SmartTerrain terrain;
        GameCamera camera;
        List<Collider> objetosColisionables = new List<Collider>();
        ElipsoidCollisionManager collisionManager;
        TgcElipsoid characterElipsoid;
        public List<TgcMesh> Vegetation { get { return vegetation; } }
        /// <summary>
        /// Obtiene o configura el terreno a editar
        /// </summary>
        public SmartTerrain Terrain { get { return terrain; } set { terrain = value; } }
        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        public override string getName()
        {
            return "Fps2";
        }

        public override string getDescription()
        {
            return "Ejemplo de como cargar una Malla estática en formato TGC";
        }
        TgcD3dInput d3dInput;
        
        public override void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            Terrain = new SmartTerrain();
            //Armo la escena.
            
            //Malla default
            string initialMeshFile = GuiController.Instance.AlumnoEjemplosMediaDir + "SelvaLoca\\" + "selva_4_game.xml";
            string terrainHm = GuiController.Instance.AlumnoEjemplosMediaDir + "fps2\\" + "hm.jpg";
            loadMesh(initialMeshFile);
            //loadHeightmap(terrainHm,100f,6.5f);
           
         
            Terrain.loadTexture(terrainHm);


            d3dInput = GuiController.Instance.D3dInput;
            //Vector3 center = new Vector3(-115, 80, -250);
            Vector3 center = new Vector3(-2775, 80, -4187);
            Vector3 size = new Vector3(10, 10, 10);
            Color color = Color.Red;
            box = TgcBox.fromSize(center, size, color);

            
            characterElipsoid = new TgcElipsoid(box.BoundingBox.calculateBoxCenter() + new Vector3(0, 0, 0), new Vector3(12, 48, 12));
             

            
            //Pongo la camara


            camera = new GameCamera(this);
            //Configurar FPS Camara
            camera.Enable = true;
            //            camera.setCamera(new Vector3(-722.6171f, 495.0046f, -31.2611f), new Vector3(164.9481f, 35.3185f, -61.5394f));
            camera.setCamera(new Vector3(box.BoundingBox.calculateBoxCenter().X, box.BoundingBox.calculateBoxCenter().Y+25, box.BoundingBox.calculateBoxCenter().Z),new Vector3(box.BoundingBox.calculateBoxCenter().X+50, box.BoundingBox.calculateBoxCenter().Y, box.BoundingBox.calculateBoxCenter().Z));

            this.hud();
            this.userVars();
            this.initCollisions();

            
            
        }

        /// <summary>
        /// Carga el heightmap a partir de la textura del path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="scaleXZ"></param>
        /// <param name="scaleY"></param>
        public void loadHeightmap(string path, float scaleXZ, float scaleY)
        {
            Terrain.loadHeightmap(path, scaleXZ, scaleY, new Vector3(0, 0, 0));
           
          //  clearVegetation();
        }

        public void initCollisions()
        {
            //Busco todo lo colisionable en la scene

            objetosColisionables.Clear();
            foreach (TgcMesh mesh in currentScene.Meshes)
            {
                //Los objetos del layer "TriangleCollision" son colisiones a nivel de triangulo
             //   if (mesh.Layer == "TriangleCollision")
              //  {
               //     objetosColisionables.Add(TriangleMeshCollider.fromMesh(mesh));
               // }
                //El resto de los objetos son colisiones de BoundingBox. Las colisiones a nivel de triangulo son muy costosas asi que deben utilizarse solo
                //donde es extremadamente necesario (por ejemplo en el piso). El resto se simplifica con un BoundingBox
                //else
                if(mesh.Layer == "Suelo" || mesh.Layer == "PAREDES" || (mesh.Name.IndexOf("tronco")>=0))
                {
                    objetosColisionables.Add(BoundingBoxCollider.fromBoundingBox(mesh.BoundingBox));
                }
            }

            collisionManager = new ElipsoidCollisionManager();
            collisionManager.GravityEnabled = true;
            collisionManager.GravityForce = new Vector3(0,-3f,0);
        }

        public override void render(float elapsedTime)
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            //Renderizar escena entera
            currentScene.renderAll();
            //box.render();
            if (camera.rotateCameraWithMouse)
            {
                text3.Text = "Presiona P para ver el mouse.";
            }
            else
            {
                text3.Text = "Presiona P para rotar la camara con el mouse.";
            }
            text3.render();
            box.render();
            characterElipsoid.render();
            userVarUpdates();
          //  Terrain.render();
          
        
        }
        public void userVars()
        {
            GuiController.Instance.UserVars.addVar("MouseCapture", false);
            GuiController.Instance.UserVars.addVar("falling", false);

            GuiController.Instance.UserVars.addVar("Cam Pos X", camera.Position.X);
            GuiController.Instance.UserVars.addVar("Cam Pos Y", camera.Position.Y);
            GuiController.Instance.UserVars.addVar("Cam Pos Z", camera.Position.Z);
            GuiController.Instance.UserVars.addVar("Cam Look X", camera.getLookAt().X);
            GuiController.Instance.UserVars.addVar("Cam Look Y", camera.getLookAt().Y);
            GuiController.Instance.UserVars.addVar("Cam Look Z", camera.getLookAt().Z);
            GuiController.Instance.UserVars.addVar("Dir X");
            GuiController.Instance.UserVars.addVar("Dir Y");
            GuiController.Instance.UserVars.addVar("Dir Z");
            GuiController.Instance.UserVars.addVar("Real X");
            GuiController.Instance.UserVars.addVar("Real Y");
            GuiController.Instance.UserVars.addVar("Real Z");
            GuiController.Instance.UserVars.addVar("Raro", "normal");
        }
        public void userVarUpdates()
        {
            GuiController.Instance.UserVars.setValue("MouseCapture", camera.rotateCameraWithMouse);
            GuiController.Instance.UserVars.setValue("Cam Pos X", camera.Position.X);
            GuiController.Instance.UserVars.setValue("Cam Pos Y", camera.Position.Y);
            GuiController.Instance.UserVars.setValue("Cam Pos Z", camera.Position.Z);
            GuiController.Instance.UserVars.setValue("Cam Look X", camera.getLookAt().X);
            GuiController.Instance.UserVars.setValue("Cam Look Y", camera.getLookAt().Y);
            GuiController.Instance.UserVars.setValue("Cam Look Z", camera.getLookAt().Z);
        }
     

        public Vector3[]  tryToMovePlayer(Vector3 from, Vector3 direction){
            Vector3 elipsoidCorrection = new Vector3(from.X - characterElipsoid.Position.X, from.Y - characterElipsoid.Position.Y, from.Z - characterElipsoid.Position.Z);
            
            Vector3 realMovement = collisionManager.moveCharacter(characterElipsoid, elipsoidCorrection, objetosColisionables);
           
            //characterElipsoid.setCenter(from);
            Vector3 prevPos = new Vector3(characterElipsoid.Position.X, characterElipsoid.Position.Y, characterElipsoid.Position.Z);

            
            realMovement = collisionManager.moveCharacter(characterElipsoid, direction, objetosColisionables);
            box.Position = new Vector3(characterElipsoid.Position.X-20,characterElipsoid.Position.Y,characterElipsoid.Position.Z);
                        
            GuiController.Instance.UserVars.setValue("Dir X",direction.X);
            GuiController.Instance.UserVars.setValue("Dir Y",direction.Y);
            GuiController.Instance.UserVars.setValue("Dir Z",direction.Z);
            GuiController.Instance.UserVars.setValue("Real X",realMovement.X);
            GuiController.Instance.UserVars.setValue("Real Y",realMovement.Y);
            GuiController.Instance.UserVars.setValue("Real Z",realMovement.Z);
            Vector3[] returnValues = new Vector3[2];


            returnValues[0] =  prevPos;
            returnValues[1] = realMovement;
            return returnValues;
        }


        public void hud()
        {
            text3 = new TgcText2d();
            text3.Text = "Presiona P para rotar la camara con el mouse.";
            text3.Align = TgcText2d.TextAlign.LEFT;
            text3.Position = new Point(5, 20);
            text3.Size = new Size(310, 100);
            text3.Color = Color.Gold;
            text3.Position = new Point(5, 20);

        }


        /// <summary>
        /// Carga una malla estatica de formato TGC
        /// </summary>
        private void loadMesh(string path)
        {
            currentPath = path;

            //Dispose de escena anterior
            if (currentScene != null)
            {
                currentScene.disposeAll();
            }

            //Cargar escena con herramienta TgcSceneLoader
            TgcSceneLoader loader = new TgcSceneLoader();
            currentScene = loader.loadSceneFromFile(path);
            
        }
        public void playerCrouchs()
        {
            characterElipsoid.setValues(characterElipsoid.Center + new Vector3(0, 20, 0), new Vector3(12, 28, 12));

        }
        public void playerStands()
        {
            characterElipsoid.setValues(characterElipsoid.Center + new Vector3(0, +20, 0), new Vector3(12, 48, 12));
        }

        public override void close()
        {
            
        }
    }
}
