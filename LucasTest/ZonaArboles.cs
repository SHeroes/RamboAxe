using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.LucasTest
{
    class ZonaArboles
    {

        List<Arbol> listaArboles;
        List<Pasto> listaPasto;
        int indArboles;
        long cantArboles;
        int indPasto;
        TgcBox box;

        public ZonaArboles()
        {
            listaArboles = new List<Arbol>();
            listaPasto = new List<Pasto>();
        }

        public void cargar(long posX, long posZ, long desplazamiento)
        {
            indArboles = 0;
            long tamanioBox = 2000;
            long desplazamientoBox = desplazamiento - (tamanioBox / 2);
            desplazamientoBox = desplazamientoBox + 100;
            Vector3 posicion = new Vector3(posX - (desplazamientoBox), 0, posZ - (desplazamientoBox));
            Vector3 tamanio = new Vector3(tamanioBox, 500, tamanioBox);

            TgcTexture horizonte = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "isla\\horizonteNorte.jpg");
            box = TgcBox.fromSize(posicion, tamanio, horizonte);

            agregarArbol(posX, posZ, desplazamiento, listaArboles, 200, 200);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 600, 200);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 800, 200);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 1400, 200);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 1800, 200);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 400, 600);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 1000, 600);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 1400, 600);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 1600, 600);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 1200, 800);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 200, 1000);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 800, 1000);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 1800, 1000);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 200, 1200);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 600, 1200);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 800, 1200);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 1400, 1200);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 1800, 1400);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 400, 1600);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 1200, 1600);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 200, 1800);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 800, 1800);
            agregarArbol(posX, posZ, desplazamiento, listaArboles, 1600, 1800);

            agregarPasto(posX, posZ, desplazamiento, listaPasto, 750, 200);
            agregarPasto(posX, posZ, desplazamiento, listaPasto, 1050, 400);
            agregarPasto(posX, posZ, desplazamiento, listaPasto, 1250, 600);
            agregarPasto(posX, posZ, desplazamiento, listaPasto, 550, 1000);
            agregarPasto(posX, posZ, desplazamiento, listaPasto, 1350, 1200);
            agregarPasto(posX, posZ, desplazamiento, listaPasto, 150, 1400);
            agregarPasto(posX, posZ, desplazamiento, listaPasto, 950, 1600);
            agregarPasto(posX, posZ, desplazamiento, listaPasto, 1450, 1800);
            agregarPasto(posX, posZ, desplazamiento, listaPasto, 450, 1800);

            cantArboles = indArboles - 1;
        }
        public void render(float elapsedTime)
        {

            foreach (Arbol arbol in listaArboles)
            {
				arbol.render(elapsedTime);
			}
            foreach (Pasto pasto in listaPasto)
            {
                pasto.render(elapsedTime);
            }

        }

        public void agregarArbol(long posX, long posZ, long desplazamiento, List<Arbol> listaArboles, long indX, long indZ)
        {
            long convX;
            long convZ;

            Arbol arbol = new Arbol();
            convX = posX + indX - (desplazamiento);
            convZ = posZ + indZ - (desplazamiento);
            arbol.move(convX, convZ);
            indArboles++;
            listaArboles.Add(arbol);

        }
        public void agregarPasto(long posX, long posZ, long desplazamiento, List<Pasto> listaPasto, long indX, long indZ)
        {
            long convX;
            long convZ;

            Pasto pasto = new Pasto();

            pasto.inicializar();
            pasto.scale(new Vector3(1, 1, 1));
            convX = posX + indX - (desplazamiento);
            convZ = posZ + indZ - (desplazamiento);
            pasto.move(convX, convZ);
            indPasto++;
            listaPasto.Add(pasto);

        }

        public TgcBoundingBox BoundingBox
        {
            get { return box.BoundingBox; }
        }

        public List<Arbol> getListaArboles()
        {
            return listaArboles;
        }
    }
}
