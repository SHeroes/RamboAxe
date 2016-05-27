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
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils;
namespace AlumnoEjemplos.Fps3
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
    public class Fps3 : TgcExample
    {
       
        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        public override string getName()
        {
            return "Fps3";
        }

        public override string getDescription()
        {
            return "Entendiendo como crear el mundo 3d.";
        }
        TgcD3dInput d3dInput;
        TgcText2d text;
        TgcSceneLoader loader = new TgcSceneLoader();
        SkyBox skyBox;
        GameCamera camera;
        TgcPlaneWall ground;
        public bool falling = false;
        List<Collider> objetosColisionables = new List<Collider>();
        ElipsoidCollisionManager collisionManager;
        

        TgcElipsoid characterElipsoid;
        double prevCuadrantX = 1;
        double prevCuadrantZ = 1;
        float width = 2000;
        List<TgcScene> scenes;
        string currentPath;
        List<Collider> meshes = new List<Collider>();
        TgcPlaneWall[][] floors = new TgcPlaneWall [9][];
        public override void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
           
            TgcTexture texture =  TgcTexture.createTexture(d3dDevice,GuiController.Instance.AlumnoEjemplosMediaDir + "resources\\" + "piso.png");
            string initialMeshFile = GuiController.Instance.AlumnoEjemplosMediaDir + "ball-TgcScene.xml";
           string terrainHm = GuiController.Instance.AlumnoEjemplosMediaDir + "fps2\\" + "hm.jpg";
            loadMesh(initialMeshFile);
            
            ground = new TgcPlaneWall(new Vector3(0,0,0),new Vector3(width,0,width),TgcPlaneWall.Orientations.XZplane,texture);
       

            for(int i = 0;i<3;i++){
                floors[i] = new TgcPlaneWall[9];
                for(int x = 0;x<3;x++){
                    floors[i][x]= ground.clone();

                }
            }
            

            for(int i = 0;i<3;i++){
                for(int x = 0;x<3;x++){
                    floors[i][x].setExtremes(new Vector3((1 + i) * width, 0, (1 + x) * width), new Vector3((1 + i) * width + width, 0, (1 + x) * width+width));
                    floors[i][x].updateValues();
                }
            }
            

            d3dInput = GuiController.Instance.D3dInput;


            characterElipsoid = new TgcElipsoid(new Vector3(0, 100, 0), new Vector3(12, 48, 12));
            this.initCollisions();
            this.initCamera();
            this.hud();
            this.skyboxInit();
            
            
        }
        public void initCamera()
        {
            camera = new GameCamera(this);
           
            //Configurar FPS Camara
            camera.Enable = true;
            //            camera.setCamera(new Vector3(-722.6171f, 495.0046f, -31.2611f), new Vector3(164.9481f, 35.3185f, -61.5394f));
            camera.setCamera(new Vector3(floors[1][1].BoundingBox.calculateBoxCenter().X, floors[1][1].BoundingBox.calculateBoxCenter().Y + 125, floors[1][1].BoundingBox.calculateBoxCenter().Z), new Vector3(floors[1][1].BoundingBox.calculateBoxCenter().X + 50, floors[1][1].BoundingBox.calculateBoxCenter().Y, floors[1][1].BoundingBox.calculateBoxCenter().Z));
        }
        public void initCollisions()
        {
            objetosColisionables.Clear();
            for (int i = 0; i < 3; i++)
            {
                for (int x = 0; x < 3; x++)
                {
                    objetosColisionables.Add(BoundingBoxCollider.fromBoundingBox(floors[i][x].BoundingBox));
                }
            }

            collisionManager = new ElipsoidCollisionManager();
            collisionManager.GravityEnabled = true;
            collisionManager.GravityForce = new Vector3(0, -1.2f, 0);
        }
        public override void render(float elapsedTime)
        {
          
            Device d3dDevice = GuiController.Instance.D3dDevice;
            //box.render();
            text.render();
            skyBox.Center = camera.Position;
            skyBox.updateValues();
            skyBox.render();
           
          //  currentScene.renderAll();
            String floorCords ="";
            for (int i = 0; i < 3; i++)
            {
                for (int x = 0; x < 3; x++)
                {
                    floors[i][x].render();
                    floorCords+= "\n["+i.ToString()+"/"+x.ToString()+"]"+floors[i][x].Position.X+floors[i][x].Position.Z;
                }
            }
            double currentCuadrantX = (Math.Floor(characterElipsoid.Position.X / width)-1);
            double currentCuadrantZ = (Math.Floor(characterElipsoid.Position.Z / width)-1);
            if (currentCuadrantX != prevCuadrantX)
            {
                collisionManager.GravityEnabled = false;
                
                for (int z = 0; z < 3; z++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                       TgcPlaneWall f = floors[z][x];
                       float nx =   ((float)(width*(currentCuadrantX-prevCuadrantX)))+f.Position.X ;
                        f.setExtremes(new Vector3(nx, 0, f.Position.Z), new Vector3(nx+width,0, f.Position.Z + width));
                        f.updateValues();
                    }
                }
                objetosColisionables.Clear();
                for (int i = 0; i < 3; i++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        objetosColisionables.Add(BoundingBoxCollider.fromBoundingBox(floors[i][x].BoundingBox));
                    }
                }
                collisionManager.GravityEnabled = true;
                prevCuadrantX = currentCuadrantX;
            }
            
            if (currentCuadrantZ != prevCuadrantZ)
            {
                collisionManager.GravityEnabled = false;
                for (int z = 0; z < 3; z++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        TgcPlaneWall f = floors[z][x];
                        float nz = ((float)(width * (currentCuadrantZ - prevCuadrantZ))) + f.Position.Z;
                        f.setExtremes(new Vector3(f.Position.X,0, nz), new Vector3(f.Position.X+width, 0, nz + width));
                        f.updateValues();
                      
                    }
                }
                objetosColisionables.Clear();
                for (int i = 0; i < 3; i++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        objetosColisionables.Add(BoundingBoxCollider.fromBoundingBox(floors[i][x].BoundingBox));
                    }
                }
                collisionManager.GravityEnabled = true;
                prevCuadrantZ = currentCuadrantZ;
            }
            foreach (Collider collider in meshes)
            {
                
                objetosColisionables.Add(collider);
            }
           
           foreach(TgcScene scene in scenes){
               scene.renderAll();
               
           }
           text.Text = floorCords + "\nCharacter: "+ characterElipsoid.Position.Z.ToString()+  " "  + characterElipsoid.Position.X.ToString() + "\n" + currentCuadrantZ.ToString() + " " + currentCuadrantX.ToString()+"\n"+characterElipsoid.Center.Y;
        }


        private void loadMesh(string path)
        {
            currentPath = path;

            //Dispose de escena anterior
            if (scenes == null)
            {
                scenes = new List<TgcScene>();
            }
            //Cargar escena con herramienta TgcSceneLoader
            
            
            TgcScene scene = loader.loadSceneFromFile(path);
            scenes.Add(scene);
            foreach (TgcMesh mesh in scene.Meshes)
            {
               
                mesh.Scale = new Vector3(2.5f,2.2f,2.2f);
               
                mesh.Position = new Vector3(2400, 1, 2500);
                mesh.updateBoundingBox();
                // meshes.Add(BoundingBoxCollider.fromBoundingBox(mesh.BoundingBox));
                TriangleMeshCollider collider = (TriangleMeshCollider.fromMesh(mesh));
                //collider.BoundingSphere.Position = mesh.Position;
                meshes.Add(collider);
                
            }
            
        }

        public void userVars()
        {
        
       /*     GuiController.Instance.UserVars.addVar("Cam Pos X", camera.Position.X);
            GuiController.Instance.UserVars.addVar("Cam Pos Y", camera.Position.Y);
            GuiController.Instance.UserVars.addVar("Cam Pos Z", camera.Position.Z);
            GuiController.Instance.UserVars.addVar("Cam Look X", camera.getLookAt().X);
            GuiController.Instance.UserVars.addVar("Cam Look Y", camera.getLookAt().Y);
            GuiController.Instance.UserVars.addVar("Cam Look Z", camera.getLookAt().Z);
        }
        public void userVarUpdates()
        {
            GuiController.Instance.UserVars.setValue("Cam Pos X", camera.Position.X);
            GuiController.Instance.UserVars.setValue("Cam Pos Y", camera.Position.Y);
            GuiController.Instance.UserVars.setValue("Cam Pos Z", camera.Position.Z);
            GuiController.Instance.UserVars.setValue("Cam Look X", camera.getLookAt().X);
            GuiController.Instance.UserVars.setValue("Cam Look Y", camera.getLookAt().Y);
            GuiController.Instance.UserVars.setValue("Cam Look Z", camera.getLookAt().Z);*/
        }
        public void skyboxInit()
        {

            skyBox = new SkyBox();
            skyBox.Center = new Vector3(0, 500, 0);
            skyBox.Size = new Vector3(10000, 10000, 10000);
            string texturesPath = GuiController.Instance.ExamplesMediaDir + "Texturas\\Quake\\SkyBox LostAtSeaDay\\";
            skyBox.setFaceTexture(SkyBox.SkyFaces.Up, texturesPath + "lostatseaday_up.jpg");
            skyBox.setFaceTexture(SkyBox.SkyFaces.Down, texturesPath + "lostatseaday_dn.jpg");
            skyBox.setFaceTexture(SkyBox.SkyFaces.Left, texturesPath + "lostatseaday_lf.jpg");
            skyBox.setFaceTexture(SkyBox.SkyFaces.Right, texturesPath + "lostatseaday_rt.jpg");
            skyBox.setFaceTexture(SkyBox.SkyFaces.Front, texturesPath + "lostatseaday_bk.jpg");
            skyBox.setFaceTexture(SkyBox.SkyFaces.Back, texturesPath + "lostatseaday_ft.jpg");
            skyBox.updateValues();
        }
     
           public void hud()
        {
            text = new TgcText2d();
            text.Text = "Texto del hud.";
            text.Align = TgcText2d.TextAlign.LEFT;
            text.Position = new Point(5, 20);
            text.Size = new Size(310, 100);
            text.Color = Color.Gold;
            text.Position = new Point(5, 20);

        }


           public Vector3[] tryToMovePlayer(Vector3 from, Vector3 direction)
           {
               Vector3 elipsoidCorrection = new Vector3(from.X - characterElipsoid.Position.X, 0, from.Z - characterElipsoid.Position.Z);

               Vector3 realMovement = collisionManager.moveCharacter(characterElipsoid, elipsoidCorrection, objetosColisionables);

               Vector3 prevPos = new Vector3(characterElipsoid.Position.X, characterElipsoid.Position.Y, characterElipsoid.Position.Z);


               realMovement = collisionManager.moveCharacter(characterElipsoid, direction, objetosColisionables);

               Vector3[] returnValues = new Vector3[2];


               returnValues[0] = prevPos;
               returnValues[1] = realMovement;
               return returnValues;
           }

           public void playerCrouchs()
           {
               characterElipsoid.setValues(characterElipsoid.Center + new Vector3(0, 0, 0), new Vector3(12, 28, 12));
             //  camera.setPosition(characterElipsoid.Position);
           }
           public void playerStands()
           {
               characterElipsoid.setValues(characterElipsoid.Center + new Vector3(0, +20, 0), new Vector3(12, 48, 12));
              // camera.setPosition(characterElipsoid.Position);
           }
           public void playerPrevJump()
           {
               characterElipsoid.setValues(characterElipsoid.Center + new Vector3(0, 0, 0), new Vector3(12, 38, 12));
               
           }
           public void playerJumps()
           {
               characterElipsoid.setValues(characterElipsoid.Center + new Vector3(0, +120, 0), new Vector3(12, 48, 12));
               camera.setPosition(characterElipsoid.Position);
           }

        public override void close()
        {
            
        }
    }
}
