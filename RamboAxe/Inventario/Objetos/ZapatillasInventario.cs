using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;

namespace AlumnoEjemplos.RamboAxe.Inventario.Objetos
{
    public class ZapatillasInventario: ObjetoInventarioEquipable
    {
        public ZapatillasInventario(): base("Zapatillas", CharacterSheet.PIES, 4)
        {
        }
    }
}
