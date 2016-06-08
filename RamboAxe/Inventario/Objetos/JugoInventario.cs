using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;

namespace AlumnoEjemplos.RamboAxe.Inventario.Objetos
{
    public class JugoInventario: ObjetoInventarioConsumible
    {
        public JugoInventario()
            : base("Juguito")
        {
            tipo = TipoObjetoInventario.Consumible;
            peso = 1;
        }

        protected override void consumir()
        {
            CharacterSheet.getInstance().addLevelSed(-20);
        }
    }
}
