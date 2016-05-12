using AlumnoEjemplos.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace AlumnoEjemplos.RamboAxe.Player
{
    class CharacterSheet
    {
        int vida;
        int sed;
        int frio;
        Inventario inv;
        private static CharacterSheet singleton;
        private CharacterSheet()
        {
            inv = new Inventario();
            
        }

        public static CharacterSheet getInstance(){
            if(singleton==null){
                singleton = new CharacterSheet();
            }
            return singleton;
        }

        public Inventario getInventario()
        {
            return inv;
        }

    }
}
