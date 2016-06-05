using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;

namespace AlumnoEjemplos.RamboAxe.Inventario
{
    public abstract class ObjetoInventarioEquipable: ObjetoInventario
    {
        protected String posicionCuerpo;

        protected ObjetoInventarioEquipable(String nombre, String posicionCuerpo, int peso = 0)
            : base(nombre, TipoObjetoInventario.Equipable, peso)
        {
            this.posicionCuerpo = posicionCuerpo;
        }

        public override void usar()
        {
            CharacterSheet personaje = CharacterSheet.getInstance();
            if (personaje.estaEquipadaParteDelCuerpo(posicionCuerpo))
            {
                personaje.desequiparObjetoDeParteDelCuerpo(posicionCuerpo);
            }
            personaje.equiparObjetoEnParteDelCuerpo(posicionCuerpo, this);
        }
    }
}
