using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.RamboAxe.Inventario.Objetos
{
    public class RamitaInventario: ObjetoInventario
    {
        public RamitaInventario()
            : base("Ramita", TipoObjetoInventario.Ninguno, 1)
        {

        }

        public override void usar()
        {
        }
    }
}
