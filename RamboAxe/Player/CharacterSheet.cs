using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Inventario;
namespace AlumnoEjemplos.RamboAxe.Player
{
    class CharacterSheet
    {
        public float vida { get; private set; }
        public float sed { get; private set; }
        public float hambre { get; private set; }
        public int pesoMaximo { get; private set; }

        ModeloInventario inv;
        private static CharacterSheet singleton;
        private CharacterSheet()
        {
            pesoMaximo = 100;
            vida = 1.0f;
            hambre = 0.001f; sed = 0.001f;

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
        public void addLevelSed(float valor){
            sed = (float)sed + valor;
            if (sed > 1.0f) sed = 1.0f; // Te estas muriendo de Sed Pah!
        }
        public void addLevelHambre(float valor)
        {
            hambre = (float)hambre + valor;
            if (hambre > 1.0f) hambre = 1.0f; // Te estas muriendo de hambre Pah!
        }       
    }
}
