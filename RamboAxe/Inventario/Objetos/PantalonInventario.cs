using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;

namespace AlumnoEjemplos.RamboAxe.Inventario.Objetos
{
    public class PantalonInventario: ObjetoInventarioEquipable
    {
        public PantalonInventario(): base("Pantalon", CharacterSheet.PIERNAS, 2)
        {
        }
    }
}
