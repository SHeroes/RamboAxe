using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.DirectInput;
using TgcViewer;
using TgcViewer.Example;
using TgcViewer.Utils.Input;

namespace AlumnoEjemplos.Inventario
{
    public class InventarioExample: TgcExample
    {
        Inventario inv;
        Objeto obj1;
        Objeto obj2;
        Objeto obj3;

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        public override string getName()
        {
            return "Inventario Test";
        }

        public override string getDescription()
        {
            return "Apreta I para abrir/cerrar Agregar objetos con los botones del 1 al 3 Cambia al inventario con flecha izquierda o derecha Cambiar de receta/item con flecha arriba y abajo Generar receta con espacio Consumir item con espacio";
        }

        public override void init()
        {
            inv = new Inventario();
            inv.abrir();
            obj1 = new Objeto();
            obj1.nombre = "Piedra";
            obj2 = new Objeto();
            obj2.nombre = "Leña";
            obj3 = new Objeto();
            obj3.nombre = "Palos";
            Receta rec1 = new Receta(obj3, 3);
            rec1.agregarIngrediente(obj2, 1);
            rec1.agregarIngrediente(obj1, 2);
            inv.agregarReceta(rec1);
            Objeto casa = new Objeto();
            casa.nombre = "Casa";
            Receta rec2 = new Receta(casa, 1);
            rec2.agregarIngrediente(obj2, 10);
            rec2.agregarIngrediente(obj3, 50);
            inv.agregarReceta(rec2);
        }

        public override void close()
        {
            inv.dispose();
            obj1.dispose();
            obj2.dispose();
            obj3.dispose();
        }

        public override void render(float elapsedTime)
        {
            TgcD3dInput input = GuiController.Instance.D3dInput;
            if (input.keyPressed(Key.I))
            {
                if(inv.abierto){
                    inv.cerrar();
                }
                else
                {
                    inv.abrir();
                }
            }
            else if(input.keyPressed(Key.LeftArrow) || input.keyPressed(Key.RightArrow)){
                inv.invertirSeleccion();
            }
            else if (input.keyPressed(Key.NumPad1) || input.keyPressed(Key.D1))
            {
                inv.agregar(obj1);
            }
            else if (input.keyPressed(Key.NumPad2) || input.keyPressed(Key.D2))
            {
                inv.agregar(obj2);
            }
            else if (input.keyPressed(Key.NumPad3) || input.keyPressed(Key.D3))
            {
                inv.agregar(obj3);
            }
            else if (input.keyPressed(Key.DownArrow))
            {
                inv.siguienteItem();
            } else if(input.keyPressed(Key.UpArrow))
            {
                inv.anteriorItem();
            } else if(input.keyPressed(Key.Space)){
                if(!inv.esReceta){
                    string consumido = inv.consumirActual();
                    Console.WriteLine("Item consumido: {0}", consumido);
                }
                else
                {
                    inv.fabricarActual();
                }
            }

            inv.render();
        }
    }
}
