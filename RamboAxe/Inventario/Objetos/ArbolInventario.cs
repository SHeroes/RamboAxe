using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.GameObjects;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.RamboAxe.Inventario.Objetos
{
    public class ArbolInventario: ObjetoInventarioConstruible
    {
        public ArbolInventario():base("Arbol")
        {

        }

        protected override GameObjectAbstract crearConstruible()
        {
            TgcMesh mesh = MapaDelJuego.getGameMesh(5).clone("arbol_const");
            return new Arbol(mesh, 0, 0, 0);
        }
    }
}
