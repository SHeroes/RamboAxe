using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Inventario;
using Microsoft.DirectX;
namespace AlumnoEjemplos.RamboAxe.Player
{
    class CharacterSheet
    {
        int vida;
        int sed;
        int frio;
        public float velocity = 1;
        public Vector3 position = new Vector3(0, 0, 0);

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
