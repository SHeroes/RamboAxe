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
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.TgcGeometry;
using System.Windows.Forms;

namespace AlumnoEjemplos.LucasTest
{
    /// <summary>
    /// Ejemplo en Blanco. Ideal para copiar y pegar cuando queres empezar a hacer tu propio ejemplo.
    /// </summary>
    public class EjemploIsla : TgcExample
    {
        TgcBox box;
        string currentHeightmap;
        float currentScaleXZ;
        float currentScaleY;
        MapaIsla isla;
        TgcBoundingBox mainMeshBoundingBox;
        Vector3 TAMANIO_BOX_PRINCIPAL = new Vector3(12, 22, 12);
        Vector3 posAnterior = new Vector3(0, 0, 0);
        bool pause = false;

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        public override string getName()
        {
            return "Lucas";
        }

        public override string getDescription()
        {
            return "Escenario outdoor con arboles.";
        }

        protected Point mouseCenter;
        SurvivalCamara camara = new SurvivalCamara();
        bool primeraIteracion = true;

        public override void init()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            isla = new MapaIsla();

            isla.cargar();  

            //Path de Heightmap default del terreno y Modifier para cambiarla
            currentHeightmap = GuiController.Instance.AlumnoEjemplosMediaDir + "Heighmaps\\" + "Heighmap3.jpg";
            //GuiController.Instance.Modifiers.addTexture("heightmap", currentHeightmap);
            
            //Modifiers para variar escala del mapa
            currentScaleXZ = 20f;
            GuiController.Instance.Modifiers.addFloat("scaleXZ", 0.1f, 100f, currentScaleXZ);
            currentScaleY = 1.3f;
            GuiController.Instance.Modifiers.addFloat("scaleY", 0.1f, 10f, currentScaleY);

            Control focusWindows = GuiController.Instance.D3dDevice.CreationParameters.FocusWindow;
            mouseCenter = focusWindows.PointToScreen(
                new Point(
                    focusWindows.Width / 2,
                    focusWindows.Height / 2)
                    );

            camara.Enable = true;
            camara.setCamera(new Vector3(0, 100, 0), new Vector3(0, 100, 4000));
            camara.MovementSpeed = 200f;
            camara.JumpSpeed = 500f;

            Vector3 center = new Vector3(0, 0, 0);
            TgcTexture texture = TgcTexture.createTexture(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Textures\\Ladrillo\\streetbricks.jpg");
            box = TgcBox.fromSize(center, TAMANIO_BOX_PRINCIPAL, texture);
            box.move(camara.getPosition());

            //Configurar FPS Camara
            //GuiController.Instance.FpsCamera.Enable = true;
            //GuiController.Instance.FpsCamera.MovementSpeed = 1000f;
            //GuiController.Instance.FpsCamera.JumpSpeed = 750f;
            //GuiController.Instance.FpsCamera.setCamera(new Vector3(-1151.339f, 143.0946f, -82.3528f), new Vector3(-1150.342f, 143.0397f, -82.4039f));


        }
        public void initCamera()
        {

        }


        public override void render(float elapsedTime)
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;
            bool collisionFound = false;

            //Ver si cambio alguno de los valores de escala
            float selectedScaleXZ = (float)GuiController.Instance.Modifiers["scaleXZ"];
            float selectedScaleY = (float)GuiController.Instance.Modifiers["scaleY"];

            if (currentScaleXZ != selectedScaleXZ || currentScaleY != selectedScaleY)
            {
                //Volver a cargar el Heightmap
                currentScaleXZ = selectedScaleXZ;
                currentScaleY = selectedScaleY;
            }

            Cursor.Position = mouseCenter;
            Cursor.Hide();
            camara.capturarMouse(true);

            Vector3 originalPos = box.Position;

            box.setPositionSize(camara.getPosition(), TAMANIO_BOX_PRINCIPAL);


            //QUILOMBO DE LA CAMARA

            if (camara.getMouseCapturado())
            {
                Cursor.Position = mouseCenter;
            }

            mainMeshBoundingBox = box.BoundingBox;

            TgcBox contorno = isla.getContorno();
            TgcCollisionUtils.BoxBoxResult collisionResult1 = TgcCollisionUtils.classifyBoxBox(mainMeshBoundingBox, contorno.BoundingBox);
            if ((collisionResult1 != TgcCollisionUtils.BoxBoxResult.Encerrando))
            {
                //collisionFound = true;
                //MessageBox.Show("Hay colision!!!");
                camara.setPosicion(posAnterior);

            }


            foreach (ZonaArboles zona in isla.zonas())
            {
                TgcBoundingBox zonaBoundingBox = zona.BoundingBox;
                TgcCollisionUtils.BoxBoxResult collisionResult = TgcCollisionUtils.classifyBoxBox(mainMeshBoundingBox, zonaBoundingBox);
                if ((collisionResult != TgcCollisionUtils.BoxBoxResult.Afuera))
                {
                    foreach (Arbol arbol in zona.getListaArboles())
                    {
                        TgcBoundingBox sceneMeshBoundingBox = arbol.BoundingBox;

                        TgcCollisionUtils.BoxBoxResult collisionResult2 = TgcCollisionUtils.classifyBoxBox(mainMeshBoundingBox, sceneMeshBoundingBox);
                        if ((collisionResult2 != TgcCollisionUtils.BoxBoxResult.Afuera))
                        {
                            collisionFound = true;
                            camara.setPosicion(posAnterior);
                            break;
                        }
                    }
                }
            }

            isla.dibujar(box, elapsedTime, pause);

            if (!collisionFound)
            {
                posAnterior = camara.getPosition();
            }

        }

        public override void close()
        {

        }

    }
}
