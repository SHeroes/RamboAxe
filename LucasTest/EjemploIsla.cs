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
        Vector3 TAMANIO_BOX_PRINCIPAL = new Vector3(10, 20, 10);
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

            Vector3 center = new Vector3(0, 0, 0);
            TgcTexture texture = TgcTexture.createTexture(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Textures\\Ladrillo\\streetbricks.jpg");
            box = TgcBox.fromSize(center, TAMANIO_BOX_PRINCIPAL, texture);

            //Configurar FPS Camara
            GuiController.Instance.FpsCamera.Enable = true;
            GuiController.Instance.FpsCamera.MovementSpeed = 100f;
            GuiController.Instance.FpsCamera.JumpSpeed = 100f;
            GuiController.Instance.FpsCamera.setCamera(new Vector3(-722.6171f, 495.0046f, -31.2611f), new Vector3(164.9481f, 35.3185f, -61.5394f));


        }
        public void initCamera()
        {

        }


        public override void render(float elapsedTime)
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            //Ver si cambio alguno de los valores de escala
            float selectedScaleXZ = (float)GuiController.Instance.Modifiers["scaleXZ"];
            float selectedScaleY = (float)GuiController.Instance.Modifiers["scaleY"];

            if (currentScaleXZ != selectedScaleXZ || currentScaleY != selectedScaleY)
            {
                //Volver a cargar el Heightmap
                currentScaleXZ = selectedScaleXZ;
                currentScaleY = selectedScaleY;
            }

            mainMeshBoundingBox = box.BoundingBox;

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
                            // MessageBox.Show("Hay colision!!!");
                            //camara.setPosicion(posAnterior);
                            break;
                        }
                    }
                }
            }


            isla.dibujar(box, elapsedTime, pause);

        }

        public override void close()
        {

        }

    }
}
