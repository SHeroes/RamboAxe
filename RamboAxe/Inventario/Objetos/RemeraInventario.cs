using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;

namespace AlumnoEjemplos.RamboAxe.Inventario.Objetos
{
    public class RemeraInventario: ObjetoInventarioEquipable
    {
        public RemeraInventario(): base("Remera", CharacterSheet.TORSO, 1)
        {
        }
    }
}
