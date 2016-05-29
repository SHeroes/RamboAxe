using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Inventario;
namespace AlumnoEjemplos.RamboAxe.Player
{
    class CharacterSheet
    {
        public int vida { get; private set; }
        public int sed { get; private set; }
        public int hambre { get; private set; }
        public int pesoMaximo { get; private set; }

        public int maximaSed  { get; private set; }
        public int maximaHambre  { get; private set; }
        public int maximaVida { get; private set; }

        ModeloInventario inv;
        private static CharacterSheet singleton;
        private CharacterSheet()
        {
            maximaSed = 40;
            maximaHambre = 80;
            maximaVida = 100;
            pesoMaximo = 100;
            vida = maximaVida;
            hambre = 0; sed = 0;

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
        public void addLevelSed(int valor){
            sed = sed + valor;
            if (sed >= maximaSed)
            {
                sed = maximaSed; // Te estas muriendo de Sed Pah!
                afectarNivelVida(-5);
            }
            else if (sed <= 0) sed = 0; // Te estas muriendo de Sed Pah!
        }

        public void addLevelHambre(int valor)
        {
            hambre = hambre  + valor;
            if (hambre >= maximaHambre) {
                hambre = maximaHambre; // Te estas muriendo de hambre Pah!
                afectarNivelVida(-5);
            }
            else if (hambre <= 0) hambre = 0; // Te estas muriendo de Sed Pah!
        }

        public void afectarNivelVida(int valor)
        {
            vida = vida + valor;
            if (vida >= maximaVida)
            {
                vida = maximaVida;
            }
            else if (vida <= 0) vida = 0; // GAME OVER!!

        }      
    }
}
