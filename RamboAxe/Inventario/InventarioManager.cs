using AlumnoEjemplos.RamboAxe.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.RamboAxe.Inventario
{
    public abstract class InventarioManager
    {
        private static Dictionary<String, ObjetoInventario> objetos;
        private static Dictionary<String, Receta> recetas;

        # region Objetos
        public static ObjetoInventario Piedra { get { return objetos["Piedra"]; } }
        public static ObjetoInventario Leña { get { return objetos["Leña"]; } }
        public static ObjetoInventario Palos { get { return objetos["Palos"]; } }
        public static ObjetoInventario Casa { get { return objetos["Casa"]; } }
        public static ObjetoInventario Ramita { get { return objetos["Ramita"]; } }
        public static ObjetoInventario Racion { get { return objetos["Racion"]; } }
        # endregion

        # region Recetas
        public static Receta RecetaCasa { get { return recetas["Casa"]; } }
        # endregion

        public static void init()
        {
            objetos = new Dictionary<String, ObjetoInventario>();
            recetas = new Dictionary<String, Receta>();
            /* Listado de Objetos */
            agregarObjeto("Piedra");
            agregarObjeto("Leña");
            agregarObjeto("Palos");
            agregarObjeto("Casa", TipoObjetoInventario.Construible);
            agregarObjeto("Ramita");
            agregarObjeto("Racion", TipoObjetoInventario.Consumible);
            /* Listado de Recetas */
            agregarReceta(
                Casa, 
                1, 
                new Dictionary<ObjetoInventario, int>{
                    { Leña, 10 },
                    { Palos, 50 }
                }
            );
        }

        public static void dispose()
        {
            objetos.Clear();
            objetos = null;
            recetas.Clear();
            recetas = null;
        }

        # region Metodos de Creacion

        /// <summary>
        /// Agrega un nuevo objeto
        /// </summary>
        /// <param name="nombre">Nombre del objeto</param>
        private static void agregarObjeto(String nombre, TipoObjetoInventario tipo = TipoObjetoInventario.Ninguno)
        {
            ObjetoInventario obj;
            if(!objetos.TryGetValue(nombre, out obj)){
                obj = new ObjetoInventario();
                obj.nombre = nombre;
                obj.tipo = tipo;
                objetos.Add(nombre, obj);
            }
            else
            {
                throw new Exception("Creado objeto repetido: " + nombre);
            }
        }

        /// <summary>
        /// Agrega una nueva receta
        /// </summary>
        /// <param name="resultado">Objeto Resultado</param>
        /// <param name="cantidad">Cantidad del Objeto Resultado</param>
        /// <param name="ingredientes">Ingredientes</param>
        private static void agregarReceta(ObjetoInventario resultado, int cantidad = 1, Dictionary<ObjetoInventario, int> ingredientes = null)
        {
            Receta receta;
            if (!recetas.TryGetValue(resultado.nombre, out receta))
            {
                receta = new Receta(resultado, cantidad);
                if(ingredientes != null){
                    foreach (ObjetoInventario objeto in ingredientes.Keys)
                    {
                        receta.agregarIngrediente(objeto, ingredientes[objeto]);
                    }
                }
                recetas.Add(resultado.nombre, receta);
            }
            else
            {
                throw new Exception("Creada receta repetida: " + resultado.nombre);
            }
        }

        # endregion

        # region Funciones Helper

        public static ObjetoInventario obtenerObjetoPorNombre(string nombre)
        {
            ObjetoInventario obj;
            if(!objetos.TryGetValue(nombre, out obj)){
                obj = null;
            }
            return obj;
        }

        public static Receta obtenerRecetaPorNombre(string nombre)
        {
            Receta receta;
            if (!recetas.TryGetValue(nombre, out receta))
            {
                receta = null;
            }
            return receta;
        }

        # endregion
    }
  
    
}
