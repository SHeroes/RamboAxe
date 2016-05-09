using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.Game
{
    public class Receta
    {
        public List<string> ingredientes { get; private set; }
        public Dictionary<string, int> cantidadIngrediente { get; private set; }

        public Objeto resultado { get; private set; }
        public int cantidadResultado { get; private set; }

        public Receta(Objeto resultado, int cantidad)
        {
            this.resultado = resultado;
            this.cantidadResultado = cantidad;
            this.ingredientes = new List<string>();
            this.cantidadIngrediente = new Dictionary<string, int>();
        }

        public void agregarIngrediente(Objeto ingrediente, int cantidad)
        {
            int currentCount;
            if (cantidadIngrediente.TryGetValue(ingrediente.nombre, out currentCount))
            {
                cantidadIngrediente[ingrediente.nombre] = cantidad;
            }
            else
            {
                ingredientes.Add(ingrediente.nombre);
                cantidadIngrediente.Add(ingrediente.nombre, cantidad);
            }
        }

        public int fabricar(Dictionary<string, int> cantidadPorIngrediente)
        {
            if(puedeFabricar(cantidadPorIngrediente)){
                /* Sacar los ingredientes */
                foreach(string ingrediente in ingredientes){
                    cantidadPorIngrediente[ingrediente] -= cantidadIngrediente[ingrediente];
                }
                return cantidadResultado;
            }
            return 0;
        }

        public bool puedeFabricar(Dictionary<string, int> cantidadPorIngrediente)
        {
            bool puedeFabricar = true;
            int index = 0;
            while(puedeFabricar && index < ingredientes.Count){
                string ingrediente = ingredientes[index];
                int cantidadNecesaria = cantidadIngrediente[ingrediente];
                int cantidadActual;
                if(cantidadPorIngrediente.TryGetValue(ingrediente, out cantidadActual)){
                    puedeFabricar = (cantidadActual >= cantidadNecesaria);
                }
                else
                {
                    puedeFabricar = false;
                }
                index++;
            }
            return puedeFabricar;
        }
    }
}
