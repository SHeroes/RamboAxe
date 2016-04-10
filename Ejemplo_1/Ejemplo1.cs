using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.Collision.ElipsoidCollision;
using System.Windows.Forms;

namespace AlumnoEjemplos.Ejemplo_1
{

    public class Ejemplo1 : TgcExample
    {
        GameCamera gameCamera;
        TgcScene tgcScene;
        TgcBox box;
        List<Collider> objetosColisionables = new List<Collider>();
        ElipsoidCollisionManager collisionManager;
        Vector3 movement;
        TgcText2d text3;
        TgcElipsoid characterElipsoid;

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        public override string getName()
        {
            return "Ejemplo 1";
        }

        public override string getDescription()
        {
            return "Tratando de armar survival craft, primer intento";
        }
        
        public override void init()
        {
            
            //Version para cargar escena desde carpeta descomprimida
            TgcSceneLoader loader = new TgcSceneLoader();
            tgcScene = loader.loadSceneFromFile(
                GuiController.Instance.AlumnoEjemplosMediaDir + "room1\\room1-TgcScene.xml",
                GuiController.Instance.AlumnoEjemplosMediaDir + "room1\\");

            gameCamera = new GameCamera();
            gameCamera.Enable = true;
            gameCamera.setCamera(new Vector3(249,49,289), new Vector3(250f,49f,290f));
            gameCamera.MovementSpeed = 200f;            
            gameCamera.JumpSpeed = 200f;
            gameCamera.rotateCamera = false;

            TgcD3dInput d3dInput = GuiController.Instance.D3dInput;


            //UserVar para contar la cantidad de meshes que se renderizan
            GuiController.Instance.UserVars.addVar("Texto de ejemplo");
            GuiController.Instance.UserVars.addVar("CAMERAX");
            GuiController.Instance.UserVars.addVar("CAMERAY");
            GuiController.Instance.UserVars.addVar("CAMERAZ");
            GuiController.Instance.UserVars.addVar("CAMERAPOSX");
            GuiController.Instance.UserVars.addVar("CAMERAPOSY");
            GuiController.Instance.UserVars.addVar("CAMERAPOSZ");
            GuiController.Instance.UserVars.addVar("Movement");
           



            GuiController.Instance.Modifiers.addBoolean("HabilitarGravedad", "Habilitar Gravedad", true);
            GuiController.Instance.Modifiers.addVertex3f("Gravedad", new Vector3(-50, -50, -50), new Vector3(50, 50, 50), new Vector3(0, -4, 0));
            GuiController.Instance.Modifiers.addFloat("SlideFactor", 0f, 2f, 1f);
            GuiController.Instance.Modifiers.addFloat("Pendiente", 0f, 1f, 0.72f);
            GuiController.Instance.Modifiers.addBoolean("Collisions", "Collisions", true);
           





            Vector3 center = new Vector3(194, 45, 178);
            Vector3 size = new Vector3(10, 10, 10);
            Color color = Color.Red;
            box = TgcBox.fromSize(center, size, color);
            characterElipsoid = new TgcElipsoid(gameCamera.Position, new Vector3(12, 28, 12));

            objetosColisionables.Clear();
            foreach (TgcMesh mesh in tgcScene.Meshes)
            {
                //Los objetos del layer "TriangleCollision" son colisiones a nivel de triangulo
                if (mesh.Layer == "TriangleCollision")
                {
                    objetosColisionables.Add(TriangleMeshCollider.fromMesh(mesh));
                }
                //El resto de los objetos son colisiones de BoundingBox. Las colisiones a nivel de triangulo son muy costosas asi que deben utilizarse solo
                //donde es extremadamente necesario (por ejemplo en el piso). El resto se simplifica con un BoundingBox
                else
                {
                    objetosColisionables.Add(BoundingBoxCollider.fromBoundingBox(mesh.BoundingBox));
                }
            }
            //Crear manejador de colisiones
            collisionManager = new ElipsoidCollisionManager();
            collisionManager.GravityEnabled = true;



            movement = new Vector3(gameCamera.getPosition().X,gameCamera.getPosition().Y,gameCamera.getPosition().Z);
         
            this.hud();

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

        public override void render(float elapsedTime)
        {
            tgcScene.renderAll();
            box.render();
            GuiController.Instance.UserVars.setValue("CAMERAPOSX", gameCamera.getPosition().X);
            GuiController.Instance.UserVars.setValue("CAMERAPOSY", gameCamera.getPosition().Y);
            GuiController.Instance.UserVars.setValue("CAMERAPOSZ", gameCamera.getPosition().Z);

            GuiController.Instance.UserVars.setValue("CAMERAX", gameCamera.getLookAt().X);
            GuiController.Instance.UserVars.setValue("CAMERAY", gameCamera.getLookAt().Y);
            GuiController.Instance.UserVars.setValue("CAMERAZ", gameCamera.getLookAt().Z);
            
            characterElipsoid.render();            
            text3.render();
            if (GuiController.Instance.D3dInput.keyPressed(Microsoft.DirectX.DirectInput.Key.P))
            {
                gameCamera.rotateCamera = !gameCamera.rotateCamera;
                if (gameCamera.rotateCamera)
                {
                    text3.Text = "Presiona P para ver el mouse.";
                    Cursor.Hide();
                }
                else
                {
                    text3.Text = "Presiona P para rotar la camara con el mouse.";
                    Cursor.Show();
                }
            }
            if (gameCamera.rotateCamera)
            {
                Cursor.Position = new Point(400, 400);
            }

            movement.Subtract(gameCamera.getPosition());
            //Actualizar valores de gravedad
            collisionManager.GravityEnabled = (bool)GuiController.Instance.Modifiers["HabilitarGravedad"];
            collisionManager.GravityForce = (Vector3)GuiController.Instance.Modifiers["Gravedad"] /** elapsedTime*/;
            collisionManager.SlideFactor = (float)GuiController.Instance.Modifiers["SlideFactor"];
            collisionManager.OnGroundMinDotValue = (float)GuiController.Instance.Modifiers["Pendiente"];
            
            if ((bool)GuiController.Instance.Modifiers["Collisions"])
            {
                //Aca se aplica toda la lógica de detección de colisiones del CollisionManager. Intenta mover el Elipsoide
                //del personaje a la posición deseada. Retorna la verdadera posicion (realMovement) a la que se pudo mover
                Vector3 realMovement = collisionManager.moveCharacter(characterElipsoid, movement, objetosColisionables);
                characterElipsoid.moveCenter(realMovement);

                //Cargar desplazamiento realizar en UserVar
                GuiController.Instance.UserVars.setValue("Movement", TgcParserUtils.printVector3(realMovement));
            }
            else
            {
                characterElipsoid.moveCenter(gameCamera.getPosition());
                gameCamera.setCamera(characterElipsoid.Position,new Vector3(characterElipsoid.Position.X,characterElipsoid.Position.Y,characterElipsoid.Position.Z));
            }


        }

        public override void close()
        {
        }

    }
}
