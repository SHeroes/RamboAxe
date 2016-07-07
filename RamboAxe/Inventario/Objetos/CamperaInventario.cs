using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;

namespace AlumnoEjemplos.RamboAxe.Inventario.Objetos
{
    public class CamperaInventario: ObjetoInventarioEquipable
    {
        public CamperaInventario(): base("Campera", CharacterSheet.TORSO, 3)
        {
        }
    }
}
