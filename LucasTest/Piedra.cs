using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.LucasTest
{
    class Piedra
    {
        int id;
        ZonaArboles zona;
        TgcMesh mesh;
        TgcScene scene = null;
        TgcBox box;

        Vector3 tamanioBox = new Vector3(40, 230, 20);
        Vector3 TAMANIO_PIEDRA = new Vector3(0.8f, 0.8f, 0.8f);

        public Piedra()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            TgcSceneLoader loader = new TgcSceneLoader();
            scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vegetacion\\Roca\\Roca-TgcScene.xml");
            mesh = scene.Meshes[0];
            mesh.Scale = TAMANIO_PIEDRA;

            TgcTexture pasto = TgcTexture.createTexture(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Textures\\Vegetacion\\pasto.jpg");
            box = TgcBox.fromSize(new Vector3(0,15,0), tamanioBox, null);
            box.Scale = tamanioBox;
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
        public void renderBoundingBox()
        {
            boundingBox().render();
        }

        public void scale(Vector3 tamanio)
        {
            mesh.Scale = tamanio;

        }

        public bool render(float elapsedTime)
        {
            mesh.render();
            this.renderBoundingBox();
            return true;
        }
        public Vector3 Position
        {
            get { return mesh.Position; }
            /*set
            {
                translation = value;
                updateBoundingBox();
            }
            */
        }

        public TgcBoundingBox getBoundingBox()
        {
            return box.BoundingBox;
        }

        public TgcBox getBox()
        {
            return box;
        }
        public void setId(int unId)
        {
            id = unId;
        }
        public int getId()
        {
            return id;
        }
        public void setZona(ZonaArboles unaZona)
        {
            zona = unaZona;
        }
        public ZonaArboles getZona()
        {
            return zona;
        }
        public void dispose()
        {
            mesh.dispose();
            box.dispose();
        }


    }
}

