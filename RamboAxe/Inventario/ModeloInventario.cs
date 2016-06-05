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
        private List<string> accesosRapidos;
        private int pesoMaximo { get { return CharacterSheet.getInstance().pesoMaximo; } }
        public int pesoActual { get; private set; }

        public ModeloInventario()
        {
            ordenObjetos = new List<string>();
            cantidadObjetos = new Dictionary<string, int>();
            recetas = new List<string>();
            accesosRapidos = new List<string>();
            pesoActual = 0;
        }

        /// <summary>
        /// Calcula que items del inventario pueden ser accesos rapidos
        /// </summary>
        private void recalcularAccesosRapidos()
        {
            List<string> nuevosAccesos = new List<string>();
            foreach(string nombreObjeto in ordenObjetos){
                ObjetoInventario obj = InventarioManager.obtenerObjetoPorNombre(nombreObjeto);
                if(obj != null && obj.esConsumible){
                    nuevosAccesos.Add(nombreObjeto);
                }
            }
            foreach(string nombreReceta in recetas){
                Receta receta = InventarioManager.obtenerRecetaPorNombre(nombreReceta);
                if(receta != null && receta.resultado.esConstruible && receta.puedeFabricar(cantidadObjetos)){
                    nuevosAccesos.Add(receta.resultado.nombre);
                }
            }
            accesosRapidos = nuevosAccesos;
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
            recalcularAccesosRapidos();
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
            recalcularAccesosRapidos();
            huboCambios();
            return true;
        }

        /// <summary>
        /// Saca una cantidad de items del inventario
        /// </summary>
        /// <param name="objeto">Objeto a sacar</param>
        /// <param name="cantidad">Cantidad a sacar</param>
        /// <returns>Si se saco o no el objeto</returns>
        public bool sacar(ObjetoInventario objeto, int cantidad = 1)
        {
            bool usado = false;
            int cantidadActual;
            if (cantidadObjetos.TryGetValue(objeto.nombre, out cantidadActual))
            {
                if(cantidadActual >= cantidad){
                    usado = true;
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
                    recalcularAccesosRapidos();
                    huboCambios();
                }
            }
            return usado;
        }

        /// <summary>
        /// Saca una cantidad de items del inventario
        /// </summary>
        /// <param name="posicion">Posicion del objeto en el inventario</param>
        /// <param name="cantidad">Cantidad a sacar</param>
        /// <returns>Si se saco o no el objeto</returns>
        public bool sacar(int posicion, int cantidad = 1)
        {
            ObjetoInventario objeto = obtenerObjetoEnPosicion(posicion);
            if(objeto == null){
                return false;
            }
            return sacar(objeto, cantidad);
        }

        /// <summary>
        /// Fabrica una receta del inventario
        /// </summary>
        /// <param name="receta">Receta del inventario</param>
        /// <returns>Si pudo o no fabricar</returns>
        public bool fabricar(Receta receta)
        {
            if(receta == null){
                return false;
            }
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
                        objeto.usar();
                    }
                    recalcularAccesosRapidos();
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

        /// <summary>
        /// Cantidad de accesos rapidos en el inventario
        /// </summary>
        /// <returns></returns>
        public int contarAccesosRapidos()
        {
            return accesosRapidos.Count;
        }

        /// <summary>
        /// Obtiene un acceso rapido por su posicion en el inventario
        /// </summary>
        /// <param name="posicion">Posicion del Acceso Rapido</param>
        /// <returns>Acceso Rapido en la posicion</returns>
        public ObjetoInventario obtenerAccesoRapidoEnPosicion(int posicion)
        {
            ObjetoInventario objeto = null;
            if (posicion >= 0 && posicion < accesosRapidos.Count)
            {
                string nombre = accesosRapidos[posicion];
                objeto = InventarioManager.obtenerObjetoPorNombre(nombre);
            }
            return objeto;
        }

        /// <summary>
        /// Consume o Fabrica el Acceso Rapido segun corresponda
        /// </summary>
        /// <param name="posicion">Posicion del Acceso Rapido en el Inventario</param>
        /// <returns>Si pudo o no usarse</returns>
        public bool usarAccesoRapidoEnPosicion(int posicion)
        {
            ObjetoInventario objeto = obtenerAccesoRapidoEnPosicion(posicion);
            bool pudoUsarse = false;
            if(objeto != null){
                if(objeto.esConsumible){
                    pudoUsarse = sacar(objeto);
                }
                else
                {
                    Receta receta = InventarioManager.obtenerRecetaPorNombre(objeto.nombre);
                    pudoUsarse = fabricar(receta);
                }
            }
            return pudoUsarse;
        }

        # endregion
    }
}
