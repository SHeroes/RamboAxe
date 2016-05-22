using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.RamboAxe.Inventario
{
    public enum TipoObjetoInventario {
        Ninguno,
        Consumible,
        Equipable,
        Construible
    }

    public class ObjetoInventario
    {
        public string nombre;
        public TipoObjetoInventario tipo;

        public bool esEquipable { get { return (tipo == TipoObjetoInventario.Equipable); } }
        public bool esConsumible { get { return (tipo == TipoObjetoInventario.Consumible); } }
        public bool esConstruible { get { return (tipo == TipoObjetoInventario.Construible); } }

        public ObjetoInventario(){
            tipo = TipoObjetoInventario.Ninguno;
        }

        public void dispose()
        {

        }
    }
}
