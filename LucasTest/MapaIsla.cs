using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.LucasTest
{
    class MapaIsla
    {
        string currentHeightmap;
        string currentTexture;
        float currentScaleXZ;
        float currentScaleY;
        TgcPlaneWall piso;
        TgcSkyBox skyBox;
        TgcBox contorno;
        int indZonas;
        int indArboles;
        long maxArboles = 500;
        long cantArboles;
        long cantZonas;
        TgcScene scene = null;
        int CTE_ZOOM = 1;
        Vector3 tamanioArbol = new Vector3(2, 2, 2);
        long DESPLAZAMIENTO = 4900;
        List<Arbol> listaArboles;
        List<ZonaArboles> listaZonas;

        public List<Arbol> arboles()
        {
            return listaArboles;
        }

        public List<ZonaArboles> zonas()
        {
            return listaZonas;
        }

        public void cargar()
        {
            long indX = 0;
            long indZ = 0;
            long DISTANCIA_ENTRE_SECTORES = 2000;
            long MAX_SECTORES = 32;

            //Path de Heightmap default del terreno y Modifier para cambiarla
            currentHeightmap = GuiController.Instance.AlumnoEjemplosMediaDir + "Heighmaps\\" + "Heighmap3.jpg";
            GuiController.Instance.Modifiers.addTexture("heightmap", currentHeightmap);

            currentScaleXZ = 55f;
            currentScaleY = 55f;

            //Path de Textura default del terreno y Modifier para cambiarla
            currentTexture = GuiController.Instance.ExamplesMediaDir + "Heighmaps\\" + "TerrainTexture2.jpg";

            //Crear SkyBox
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 2400, 0);
            skyBox.Size = new Vector3(12000, 5000, 12000);
            string texturesPath = GuiController.Instance.AlumnoEjemplosMediaDir + "isla\\SkyBox\\elbrus\\";
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "elbrus_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "elbrus_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "elbrus_lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "elbrus_rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "elbrus_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "elbrus_ft.jpg");
            skyBox.SkyEpsilon = 50f;
            skyBox.updateValues();


            //Creo un plano XY (en realidad es una caja con altura 0)
            TgcTexture pasto = TgcTexture.createTexture(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Textures\\Vegetacion\\moss_rock60_512.jpg");

            Vector3 sizePiso = new Vector3(10000, 0, 10000);
            sizePiso = sizePiso * CTE_ZOOM;
            piso = new TgcPlaneWall();
            piso.setTexture(pasto);
            piso.Origin = new Vector3(-5000, 0, -5000);
            piso.Size = sizePiso;
            TgcPlaneWall.Orientations or = TgcPlaneWall.Orientations.XZplane;
            piso.Orientation = or;
            piso.AutoAdjustUv = false;
            piso.UTile = 1000;
            piso.VTile = 1000;
            piso.updateValues();

            TgcTexture texture = TgcTexture.createTexture(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Textures\\Concreto\\clang_floor2.jpg");
            contorno = TgcBox.fromSize(new Vector3(0, 1000, 0), new Vector3(10000, 2500, 10000), texture);

            TgcSceneLoader loader = new TgcSceneLoader();

            listaArboles = new List<Arbol>();
            listaZonas = new List<ZonaArboles>();

            for (indZ = 0; (indZ < 10000) && (indArboles <= MAX_SECTORES); indZ = indZ + (DISTANCIA_ENTRE_SECTORES))
            {

                for (indX = 0; (indX < 10000) && (indArboles <= MAX_SECTORES); indX = indX + (DISTANCIA_ENTRE_SECTORES))
                {
                    ZonaArboles zona = new ZonaArboles();
                    zona.cargar(indX, indZ, DESPLAZAMIENTO);
                    listaZonas.Add(zona);
                    indZonas++;

                }
            }

            cantZonas = indZonas - 1;

        }
        public void dibujar(TgcBox personajeBox, float elapsedTime, bool pause)
        {
            piso.render();
            skyBox.render();

            TgcBoundingBox personajeBoundingBox = personajeBox.BoundingBox;

            for (indZonas = 0; indZonas <= cantZonas; indZonas++)
            {
                ZonaArboles zona = listaZonas[indZonas];

                if (revisarColisionConElFrustum(zona))
                {
                    zona.render(elapsedTime);
                }
            }
            
        }

        public bool revisarColisionConElFrustum(ZonaArboles zona)
        {
            TgcBoundingBox meshBoundingBox = zona.BoundingBox;

            TgcCollisionUtils.FrustumResult collisionResult1 = TgcCollisionUtils.classifyFrustumAABB(GuiController.Instance.Frustum, meshBoundingBox);
            if (collisionResult1 != TgcCollisionUtils.FrustumResult.OUTSIDE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

