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

    public abstract class ObjetoInventario
    {
        public string nombre { get; protected set; }
        public TipoObjetoInventario tipo { get; protected set; }
        public int peso { get; protected set; }

        public bool esEquipable { get { return (tipo == TipoObjetoInventario.Equipable); } }
        public bool esConsumible { get { return (tipo == TipoObjetoInventario.Consumible); } }
        public bool esConstruible { get { return (tipo == TipoObjetoInventario.Construible); } }

        protected ObjetoInventario(
            String nombre,
            TipoObjetoInventario tipo = TipoObjetoInventario.Ninguno,
            int peso = 0
        )
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.peso = peso;
        }

        public void dispose()
        {

        }

        /// <summary>
        /// Usa un objeto
        /// </summary>
        public abstract void usar();
    }
}
