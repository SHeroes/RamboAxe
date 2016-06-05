using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.RamboAxe.Inventario.Objetos
{
    public class PiedraInventario: ObjetoInventario
    {
        public PiedraInventario()
            : base("Piedra", TipoObjetoInventario.Ninguno, 2)
        {

        }

        public override void usar()
        {
        }
    }
}
