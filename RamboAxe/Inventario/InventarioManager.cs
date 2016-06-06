using AlumnoEjemplos.RamboAxe.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Inventario.Objetos;

namespace AlumnoEjemplos.RamboAxe.Inventario
{
    public abstract class InventarioManager
    {
        private static Dictionary<String, ObjetoInventario> objetos;
        private static Dictionary<String, Receta> recetas;

        # region Objetos
        public static ObjetoInventario Piedra { get { return objetos["Piedra"]; } }
        public static ObjetoInventario Ramita { get { return objetos["Ramita"]; } }
        public static ObjetoInventario Racion { get { return objetos["Racion"]; } }
        public static ObjetoInventario Hacha { get { return objetos["Hacha"]; } }
        public static ObjetoInventario Arbol { get { return objetos["Arbol"]; } }
        public static ObjetoInventario Pantalon { get { return objetos["Pantalon"]; } }
        # endregion

        # region Recetas
        public static Receta RecetaArbol { get { return recetas["Arbol"]; } }
        # endregion

        public static void init()
        {
            objetos = new Dictionary<String, ObjetoInventario>();
            recetas = new Dictionary<String, Receta>();
            /* Listado de Objetos */
            agregarObjeto(new ArbolInventario());
            agregarObjeto(new HachaInventario());
            agregarObjeto(new PantalonInventario());
            agregarObjeto(new PiedraInventario());
            agregarObjeto(new RacionInventario());
            agregarObjeto(new RamitaInventario());
            /* Listado de Recetas */
            agregarReceta(
                Arbol,
                1,
                new Dictionary<ObjetoInventario, int>{
                    { Ramita, 2 }
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
        /// Agregar un objeto
        /// </summary>
        /// <param name="objeto"></param>
        private static void agregarObjeto(ObjetoInventario objeto)
        {
            if (!objetos.ContainsKey(objeto.nombre))
            {
                objetos.Add(objeto.nombre, objeto);
            }
            else
            {
                throw new Exception("Creado objeto repetido: " + objeto.nombre);
            }
        }

        /// <summary>
        /// Crea y Agrega una receta
        /// </summary>
        /// <param name="resultado">Objeto Resultado</param>
        /// <param name="cantidad">Cantidad del Objeto Resultado</param>
        /// <param name="ingredientes">Ingredientes</param>
        private static void agregarReceta(ObjetoInventario resultado, int cantidad = 1, Dictionary<ObjetoInventario, int> ingredientes = null)
        {
            Receta receta = new Receta(resultado, cantidad);
            if(ingredientes != null){
                foreach (ObjetoInventario objeto in ingredientes.Keys)
                {
                    receta.agregarIngrediente(objeto, ingredientes[objeto]);
                }
            }
            agregarReceta(receta);
        }

        /// <summary>
        /// Agrega una receta
        /// </summary>
        /// <param name="receta"></param>
        private static void agregarReceta(Receta receta){
            if (!recetas.ContainsKey(receta.resultado.nombre))
            {
                recetas.Add(receta.resultado.nombre, receta);
            }
            else
            {
                throw new Exception("Creada receta repetida: " + receta.resultado.nombre);
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
