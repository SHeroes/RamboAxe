using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;
using AlumnoEjemplos.RamboAxe.GameObjects;

namespace AlumnoEjemplos.RamboAxe.Inventario
{
    public abstract class ObjetoInventarioConstruible: ObjetoInventario
    {
        protected ObjetoInventarioConstruible(String nombre, int peso = 0)
            : base(nombre, TipoObjetoInventario.Construible, peso)
        {

        }

        public override void usar()
        {
            CharacterSheet.getInstance().empezarConstruccion(
                crearConstruible()    
            );
        }

        protected abstract GameObjectAbstract crearConstruible();
    }
}
