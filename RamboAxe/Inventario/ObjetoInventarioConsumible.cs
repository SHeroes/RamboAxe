using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;

namespace AlumnoEjemplos.RamboAxe.Inventario
{
    public abstract class ObjetoInventarioConsumible: ObjetoInventario
    {
        protected ObjetoInventarioConsumible(String nombre, int peso = 0)
            : base(nombre, TipoObjetoInventario.Consumible, peso)
        {

        }

        public override void usar()
        {
            CharacterSheet.getInstance().getInventario().sacar(this);
            consumir();
        }

        protected abstract void consumir();
    }
}
