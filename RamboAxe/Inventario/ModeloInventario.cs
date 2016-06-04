using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;

namespace AlumnoEjemplos.RamboAxe.Inventario
{
    public class ModeloInventario: Observable
    {
        private List<string> ordenObjetos;
        public Dictionary<string, ObjetoInventario> equipoEnUso;
        private Dictionary<string, int> cantidadObjetos;
        private List<string> recetas;
        private int pesoMaximo { get { return CharacterSheet.getInstance().pesoMaximo; } }
        public int pesoActual { get; private set; }

        public ModeloInventario()
        {
            ordenObjetos = new List<string>();
            cantidadObjetos = new Dictionary<string, int>();
            recetas = new List<string>();
            pesoActual = 0;
        }

        # region Cambiar objetos y recetas

        /// <summary>
        /// Agrega una receta al inventario
        /// </summary>
        /// <param name="receta">Receta a agregar</param>
        public void agregar(Receta receta)
        {
            string nombre = receta.resultado.nombre;
            if(recetas.IndexOf(nombre) == -1){
                recetas.Add(nombre);
            }
            huboCambios();
        }

        /// <summary>
        /// Agrega un objeto al inventario
        /// </summary>
        /// <param name="objeto">Objeto a agregar</param>
        /// <param name="cantidad">Cantidad de objeto a agregar</param>
        /// <returns>Si se agrego o no el objeto</returns>
        public bool agregar(ObjetoInventario objeto, int cantidad = 1)
        {
            int pesoAgregado = objeto.peso * cantidad;
            if(pesoAgregado + pesoActual > pesoMaximo){
                return false;
            }
            pesoActual += pesoAgregado;
            int cantidadActual;
            if(!cantidadObjetos.TryGetValue(objeto.nombre, out cantidadActual)){
                ordenObjetos.Add(objeto.nombre);
                cantidadObjetos.Add(objeto.nombre, cantidad);
            }
            else
            {
                cantidadActual += cantidad;
                cantidadObjetos[objeto.nombre] = cantidadActual;
            }
            huboCambios();
            return true;
        }

        /// <summary>
        /// Saca una cantidad de items del inventario
        /// </summary>
        /// <param name="objeto">Objeto a sacar</param>
        /// <param name="cantidad">Cantidad a sacar</param>
        /// <returns>Si se saco o no el objeto</returns>
        public bool consumir(ObjetoInventario objeto, int cantidad = 1)
        {
            bool consumido = false;
            int cantidadActual;
            if (cantidadObjetos.TryGetValue(objeto.nombre, out cantidadActual))
            {
                if(cantidadActual >= cantidad){
                    consumido = true;
                    cantidadActual -= cantidad;
                    pesoActual -= objeto.peso * cantidad;
                    if(cantidadActual == 0){
                        ordenObjetos.Remove(objeto.nombre);
                        cantidadObjetos.Remove(objeto.nombre);
                    }
                    else
                    {
                        cantidadObjetos[objeto.nombre] = cantidadActual;
                    }
                    while(cantidad > 0){
                        objeto.alConsumir();
                        cantidad--;
                    }
                    huboCambios();
                }
            }
            return consumido;
        }

        /// <summary>
        /// Saca una cantidad de items del inventario
        /// </summary>
        /// <param name="posicion">Posicion del objeto en el inventario</param>
        /// <param name="cantidad">Cantidad a sacar</param>
        /// <returns>Si se saco o no el objeto</returns>
        public bool consumir(int posicion, int cantidad = 1)
        {
            ObjetoInventario objeto = obtenerObjetoEnPosicion(posicion);
            if(objeto == null){
                return false;
            }
            return consumir(objeto, cantidad);
        }

        /// <summary>
        /// Fabrica una receta del inventario
        /// </summary>
        /// <param name="receta">Receta del inventario</param>
        /// <returns>Si pudo o no fabricar</returns>
        public bool fabricar(Receta receta)
        {
            bool seFabrico = false;
            int indiceReceta = recetas.IndexOf(receta.resultado.nombre);
            if(indiceReceta != -1){
                int cantidad = receta.fabricar(cantidadObjetos);
                if(cantidad > 0){
                    seFabrico = true;
                    /* Calculo el peso de los ingredientes y se lo saco al peso actual */
                    int pesoIngredientes = 0;
                    foreach (string name in receta.ingredientes)
                    {
                        ObjetoInventario obj = InventarioManager.obtenerObjetoPorNombre(name);
                        int count;
                        if (!receta.cantidadIngrediente.TryGetValue(name, out count))
                        {
                            count = 0;
                        }
                        pesoIngredientes += obj.peso * count;
                    }
                    pesoActual -= pesoIngredientes;

                    ObjetoInventario objeto = receta.resultado;
                    if(!objeto.esConstruible){
                        agregar(objeto, cantidad);
                    }
                    else
                    {
                        CharacterSheet.getInstance().empezarConstruccion(objeto);
                    }
                    huboCambios();
                }
            }
            return seFabrico;
        }

        /// <summary>
        /// Fabrica una receta del inventario
        /// </summary>
        /// <param name="posicion">Posicion de la Receta en el Inventario</param>
        /// <returns>Si pudo o no fabricar</returns>
        public bool fabricar(int posicion)
        {
            Receta receta = obtenerRecetaEnPosicion(posicion);
            if (receta == null)
            {
                return false;
            }
            return fabricar(receta);
        }

        # endregion

        # region Obtener objetos y recetas

        /// <summary>
        /// Cantidad de objetos en el inventario
        /// </summary>
        /// <returns></returns>
        public int contarObjetos()
        {
            return ordenObjetos.Count;
        }

        /// <summary>
        /// Obtiene un objeto por su posicion en el inventario
        /// </summary>
        /// <param name="posicion">Posicion del Objeto</param>
        /// <returns>Objeto en la posicion</returns>
        public ObjetoInventario obtenerObjetoEnPosicion(int posicion)
        {
            ObjetoInventario objeto = null;
            if(posicion >= 0 && posicion < ordenObjetos.Count){
                string nombre = ordenObjetos[posicion];
                objeto = InventarioManager.obtenerObjetoPorNombre(nombre);
            }
            return objeto;
        }

        /// <summary>
        /// Obtiene la cantidad de un objeto en el inventario
        /// </summary>
        /// <param name="objeto">Objeto</param>
        /// <returns>Cantidad en el inventario</returns>
        public int cantidadPorObjeto(ObjetoInventario objeto)
        {
            int cantidad;
            if(!cantidadObjetos.TryGetValue(objeto.nombre, out cantidad)){
                cantidad = 0;
            }
            return cantidad;
        }

        /// <summary>
        /// Cantidad de recetas en el inventario
        /// </summary>
        /// <returns></returns>
        public int contarRecetas()
        {
            return recetas.Count;
        }

        /// <summary>
        /// Obtiene una receta por su posicion en el inventario
        /// </summary>
        /// <param name="posicion">Posicion del Objeto</param>
        /// <returns>Receta en la posicion</returns>
        public Receta obtenerRecetaEnPosicion(int posicion) {
            Receta receta = null;
            if (posicion >= 0 && posicion < recetas.Count)
            {
                string nombre = recetas[posicion];
                receta = InventarioManager.obtenerRecetaPorNombre(nombre);
            }
            return receta;
        }

        # endregion
    }
}
