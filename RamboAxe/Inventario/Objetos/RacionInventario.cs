using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;

namespace AlumnoEjemplos.RamboAxe.Inventario.Objetos
{
    public class RacionInventario: ObjetoInventarioConsumible
    {
        public RacionInventario()
            : base("Racion")
        {

        }

        protected override void consumir()
        {
            CharacterSheet.getInstance().addLevelHambre(-20);
        }
    }
}
