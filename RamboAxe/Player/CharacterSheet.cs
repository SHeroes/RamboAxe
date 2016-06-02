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
        public const string CABEZA = "Cabeza";
        public const string TORSO = "Torso";
        public const string MANO_IZQUIERDA = "Mano Izquierda";
        public const string MANO_DERECHA = "Mano Derecha";
        public const string CINTURA = "Cintura";
        public const string PIERNAS = "Piernas";
        public const string PIES = "Pies";

        public int continueCounter { get; private set; }
        public int vida { get; private set; }
        public int sed { get; private set; }
        public int hambre { get; private set; }
        public int pesoMaximo { get; private set; }
        public bool estaConstruyendo { get { return (construyendo != null); } }
        public GameObjectAbstract construyendo { get; private set; }

        public int maximaSed  { get; private set; }
        public int maximaHambre  { get; private set; }
        public int maximaVida { get; private set; }
        public Dictionary<String,ObjetoInventario> equipoEnUso;

        


        
        ModeloInventario inv;
        private static CharacterSheet singleton;
        private CharacterSheet()
        {
            equipoEnUso = new Dictionary<string,ObjetoInventario>();
            equipoEnUso.Add(CharacterSheet.CABEZA, null);
            equipoEnUso.Add(CharacterSheet.TORSO, null);
            equipoEnUso.Add(CharacterSheet.MANO_IZQUIERDA, null);
            equipoEnUso.Add(CharacterSheet.MANO_DERECHA, null);
            equipoEnUso.Add(CharacterSheet.CINTURA, null);
            equipoEnUso.Add(CharacterSheet.PIERNAS, null);
            equipoEnUso.Add(CharacterSheet.PIES, null);


            continueCounter = 0;
            maximaSed = 40;
            maximaHambre = 80;
            maximaVida = 100;
            pesoMaximo = 100;
            vida = maximaVida;
            hambre = 0; sed = 0;

            inv = new ModeloInventario();
            inv.equipoEnUso = equipoEnUso;
        }



        public ObjetoInventario desequiparObjetoDeParteDelCuerpo(String parteDelCuerpo)        //desequipar
        {
            ObjetoInventario objetoDesequipado = null;
            equipoEnUso.TryGetValue(parteDelCuerpo, out objetoDesequipado);
            if (objetoDesequipado != null)
            {
                equipoEnUso.Remove(parteDelCuerpo);
                equipoEnUso.Add(parteDelCuerpo, null);
            }
            return objetoDesequipado;
        }
        public ObjetoInventario equiparObjetoEnParteDelCuerpo(String parteDelCuerpo, ObjetoInventario objetoAEquipar)        //equipar
        {
            ObjetoInventario objetoDesequipado = null;
            objetoDesequipado = desequiparObjetoDeParteDelCuerpo(parteDelCuerpo);
            equipoEnUso.Remove(parteDelCuerpo);
            equipoEnUso.Add(parteDelCuerpo, objetoAEquipar);
            return objetoDesequipado;
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

        public void reloadContinueStats(){
            //TODO una linda logica que mientras mas continues haya hecho mas choto reinicia
                this.addLevelHambre(-30);
                this.addLevelSed(-50);
                this.afectarNivelVida(50);
        }
        public void incrementContinueCounter(){
            continueCounter++;
        }
        /*
        public void sufrirDanioTermico(int cantidaDaño) {
            vida = vida - cantidaDaño;
            if (vida < 0) vida = 0;
        }
         * */
        public void danioPorCalor(int cantidadDanio) {
            this.addLevelHambre(cantidadDanio);

        }
        public void danioPorFrio(int cantidadDanio)
        {
            this.addLevelSed(cantidadDanio);

        }
    }
}
