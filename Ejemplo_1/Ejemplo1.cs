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
        public bool rotateCamera;
        GameCamera gameCamera;
        float rotationSpeed = 2.0f;
        TgcScene tgcScene;
        TgcBox box;
        TgcText2d text3;
        TgcElipsoid characterElipsoid;
        TgcArrow arrow;

        List<Collider> objetosColisionables = new List<Collider>();
        ElipsoidCollisionManager collisionManager;

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
        TgcD3dInput d3dInput;
        public override void init()
        {

            //Version para cargar escena desde carpeta descomprimida
            TgcSceneLoader loader = new TgcSceneLoader();
            tgcScene = loader.loadSceneFromFile(
                GuiController.Instance.AlumnoEjemplosMediaDir + "room1\\room1-TgcScene.xml",
                GuiController.Instance.AlumnoEjemplosMediaDir + "room1\\");

            gameCamera = new GameCamera();
            gameCamera.Enable = true;
            gameCamera.MovementSpeed = 200f;            
            gameCamera.JumpSpeed = 200f;
            rotateCamera = false;
            
            d3dInput = GuiController.Instance.D3dInput;


            //UserVar para contar la cantidad de meshes que se renderizan
            GuiController.Instance.UserVars.addVar("Collision");
           
            GuiController.Instance.UserVars.addVar("CameraLookX");
            GuiController.Instance.UserVars.addVar("CameraLookY");
            GuiController.Instance.UserVars.addVar("CameraLookZ");
            GuiController.Instance.UserVars.addVar("CameraPosX");
            GuiController.Instance.UserVars.addVar("CameraPosY");
            GuiController.Instance.UserVars.addVar("CameraPosZ");
            GuiController.Instance.UserVars.addVar("Movement");
           

            GuiController.Instance.Modifiers.addBoolean("Collisions", "Collisions", true);
           




            Vector3 center = new Vector3(260, 39, 300);
            Vector3 size = new Vector3(10, 10, 10);
            Color color = Color.Red;
            box = TgcBox.fromSize(center, size, color);
            characterElipsoid = new TgcElipsoid(box.BoundingBox.calculateBoxCenter() + new Vector3(0, 0, 0), new Vector3(12, 28, 12));
            
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
            


            arrow = new TgcArrow();


            Vector3 start = box.BoundingBox.calculateBoxCenter();
            Vector3 end = new Vector3(0, 0, 20);
            end.Add(start);
            Vector3 cameraStart = new Vector3(start.X,start.Y,start.Z);
            cameraStart.Add(new Vector3(0,10,-20));
            gameCamera.setCamera(cameraStart,end);
            arrow.Thickness = 0.1f;
            arrow.HeadSize = new Vector2(0.2f, 0.2f);
            arrow.BodyColor = Color.AliceBlue;
            arrow.HeadColor = Color.Black;

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

            float heading = 0.0f;
            float pitch = 0.0f;



            
            //Cargar valores de la flecha

            //Actualizar valores para hacerlos efectivos
            arrow.updateValues();

            //Render
            arrow.render();

            tgcScene.renderAll();
            
            //box.render();
            characterElipsoid.render();           
            GuiController.Instance.UserVars.setValue("CameraPosX", gameCamera.getPosition().X);
            GuiController.Instance.UserVars.setValue("CameraPosY", gameCamera.getPosition().Y);
            GuiController.Instance.UserVars.setValue("CameraPosZ", gameCamera.getPosition().Z);

            GuiController.Instance.UserVars.setValue("CameraLookX", gameCamera.getLookAt().X);
            GuiController.Instance.UserVars.setValue("CameraLookY", gameCamera.getLookAt().Y);
            GuiController.Instance.UserVars.setValue("CameraLookZ", gameCamera.getLookAt().Z);
            text3.render();
            if (GuiController.Instance.D3dInput.keyPressed(Microsoft.DirectX.DirectInput.Key.P))
            {
                rotateCamera = !rotateCamera;
                if (rotateCamera)
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
            if (rotateCamera)
            {
                Cursor.Position = new Point(400, 400);
            }
            
            Vector3 movementVector = Vector3.Empty;
            movementVector = getMovementDirection(d3dInput);
            Vector3 realMovement = gameCamera.getLookAt();
            
            
            //Collision manager.
            realMovement = collisionManager.moveCharacter(characterElipsoid, movementVector, objetosColisionables);
            GuiController.Instance.UserVars.setValue("Movement", TgcParserUtils.printVector3(realMovement));

            box.Position = characterElipsoid.Position;
            gameCamera.setPosition(characterElipsoid.Center);
            
            //box.BoundingBox.calculateBoxCenter());

            pitch = d3dInput.YposRelative * rotationSpeed;
            heading = d3dInput.XposRelative * rotationSpeed;

            if (rotateCamera)
            {
                rotate(heading, pitch, 0.0f);
            }
            characterElipsoid.render();

            /**/
            if (collisionManager.Result.collisionFound)
            {
                GuiController.Instance.UserVars.setValue("Collision", "Colliding!");

            }
            else
            {
                GuiController.Instance.UserVars.setValue("Collision", "not Colliding");
            }

        }
        private void rotate(float headingDegrees, float pitchDegrees, float rollDegrees)
        {
            rollDegrees = -rollDegrees;
            gameCamera.rotateFirstPerson(headingDegrees, pitchDegrees);
        }
        /// <summary>
        /// Obtiene la direccion a moverse por la camara en base a la entrada de teclado
        /// </summary>
        private Vector3 getMovementDirection(TgcD3dInput d3dInput)
        {
            Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 lookingAt = new Vector3(gameCamera.getLookAt().X - box.BoundingBox.calculateBoxCenter().X, 0, gameCamera.getLookAt().Z - box.BoundingBox.calculateBoxCenter().Z);
            
            if (d3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.W))
            {
                direction.Add(lookingAt);
            }
            else if(d3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.S))
            {
                direction.Subtract(lookingAt);
            }
            
            if (d3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.A))
            {
                float z = (lookingAt.X + lookingAt.Y) /((lookingAt.Z)*-1);
                var perpend = new Vector3(-1, -1, -z);

                direction.Add(perpend);
            
            }
            else if (d3dInput.keyDown(Microsoft.DirectX.DirectInput.Key.D))
            {

                float z = (lookingAt.X + lookingAt.Y) / ((lookingAt.Z) * -1);
                var perpend = new Vector3(1, 1, z);
                
                direction.Add(perpend);
            }
            return direction;
        }
        public override void close()
        {
        }

    }
}
