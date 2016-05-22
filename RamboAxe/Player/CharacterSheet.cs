using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Inventario;
namespace AlumnoEjemplos.RamboAxe.Player
{
    class CharacterSheet
    {
        int vida;
        int sed;
        int frio;
        public int pesoMaximo { get; private set; }

        ModeloInventario inv;
        private static CharacterSheet singleton;
        private CharacterSheet()
        {
            pesoMaximo = 100;
            inv = new ModeloInventario();
        }

        public static CharacterSheet getInstance(){
            if(singleton==null){
                singleton = new CharacterSheet();
            }
            return singleton;
        }

        public ModeloInventario getInventario()
        {
            return inv;
        }


    }
}
