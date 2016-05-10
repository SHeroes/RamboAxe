using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using System.Windows.Forms;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Input;
using TgcViewer.Utils.TgcSkeletalAnimation;
using TgcViewer.Utils.Shaders;


namespace AlumnoEjemplos.LucasTest
{
    class Arbol
    {
        TgcMesh mesh;
        TgcScene scene = null;
        TgcBox box;
        Vector3 tamanioBox = new Vector3(25, 600, 25);
        Vector3 TAMANIO_ARBOL = new Vector3(2, 2, 2);
        float contAnim;
        const float AXIS_ROTATION_SPEED = 0.1f;
        float PI = Convert.ToSingle(Math.PI);
        float acumuladorTiempo = 0;
        float contadorGiro = 0;
        float constanteSumaGiro = 1;
        const int TOPE_GIRO = 10;
        const float TIEMPO_GIRO = 0.0001f;
        float velocidadGiro = 0.0005f * (Convert.ToSingle(Math.PI));

        public Arbol()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            TgcSceneLoader loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Pino\\Pino-TgcScene.xml");
            mesh = scene.Meshes[0];
            mesh.Scale = TAMANIO_ARBOL;

            TgcTexture pasto = TgcTexture.createTexture(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Textures\\Vegetacion\\pasto.jpg");
            box = TgcBox.fromSize(new Vector3(0, 0, 0), tamanioBox, pasto);
            box.Scale = tamanioBox;
            contAnim = 0;

        }

        public void move(long posX, long posZ)
        {
            mesh.move(posX, 0, posZ);
            box.move(posX, 0, posZ);

        }

        public TgcBoundingBox boundingBox()
        {
            return box.BoundingBox;
        }

        public TgcBoundingBox BoundingBox
        {
            get { return box.BoundingBox; }
        }

        public void renderBoundingBox()
        {
            boundingBox().render();
        }

        public void scale(Vector3 tamanio)
        {
            mesh.Scale = tamanio;

        }

        public Vector3 Position
        {
            get { return mesh.Position; }
        }

        public void render(float elapsedTime)
        {

            // Esto es lo de la posicion
            acumuladorTiempo = acumuladorTiempo + elapsedTime;

            efectoViento();

            mesh.render();
        }

        public void efectoViento()
        {
            if (acumuladorTiempo >= TIEMPO_GIRO)
            {
                float anguloDeGiro = Convert.ToSingle(Math.Sin(contAnim * Math.PI / 180));

                mesh.rotateX(velocidadGiro);

                contadorGiro = contadorGiro + constanteSumaGiro;

                if (contadorGiro == TOPE_GIRO || contadorGiro == (TOPE_GIRO * (-1)))
                {
                    constanteSumaGiro = constanteSumaGiro * (-1);
                    velocidadGiro = velocidadGiro * (-1);
                }

                acumuladorTiempo = 0;
            }

        }
        public void dispose()
        {
            mesh.dispose();
            box.dispose();
        }

        public TgcMesh getMesh()
        {
            return mesh;
        }

    }
}
