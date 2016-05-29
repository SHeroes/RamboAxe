using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Inventario;
using AlumnoEjemplos.RamboAxe.GameObjects;
namespace AlumnoEjemplos.RamboAxe.Player
{
    class CharacterSheet: Observable
    {
        public int vida { get; private set; }
        public int sed { get; private set; }
        public int hambre { get; private set; }
        public int pesoMaximo { get; private set; }
        public bool estaConstruyendo { get { return (construyendo != null); } }
        public GameObjectAbstract construyendo { get; private set; }

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

        /// <summary>
        /// Se empieza un plan de construir un objeto
        /// </summary>
        /// <param name="aConstruir"></param>
        public void empezarConstruccion(ObjetoInventario aConstruir)
        {
            GameObjectAbstract go;
            if(aConstruir == InventarioManager.Arbol){
                go = new Arbol();
            }
            else
            {
                go = null;
            }
            if(go != null){
                construyendo = go;
                huboCambios();
            }
        }

        /// <summary>
        /// Se construye el objeto en la posicion actual
        /// </summary>
        public void construir()
        {
            if (!estaConstruyendo)
            {
                return;
            }
            GameObjectAbstract go = construyendo;
            construyendo = null;
            EjemploAlumno.getInstance().mapa.placeObject(go);
            huboCambios();
        }

        /// <summary>
        /// Se cancela la construccion actual
        /// </summary>
        public void cancelarConstruccion()
        {
            if(estaConstruyendo){
                construyendo = null;
                huboCambios();
            }
        }

    }
}
