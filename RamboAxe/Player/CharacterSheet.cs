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
        int vida;
        int sed;
        int frio;
        public int pesoMaximo { get; private set; }
        public bool estaConstruyendo { get { return (construyendo != null); } }
        public GameObjectAbstract construyendo { get; private set; }

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
    }
}
