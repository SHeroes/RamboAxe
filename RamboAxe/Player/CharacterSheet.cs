using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Inventario;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcGeometry;
using AlumnoEjemplos.RamboAxe.GameObjects;
using TgcViewer.Utils.TgcSceneLoader;
namespace AlumnoEjemplos.RamboAxe.Player
{
    class CharacterSheet: Observable
    {
        private static System.Timers.Timer aTimer;
        public const string CABEZA = "Cabeza";
        public const string TORSO = "Torso";
        public const string MANO_IZQUIERDA = "Mano Izquierda";
        public const string MANO_DERECHA = "Mano Derecha";
        public const string CINTURA = "Cintura";
        public const string PIERNAS = "Piernas";
        public const string PIES = "Pies";
        public List<string> deBuffes;

        public int continueCounter { get; private set; }
        public int vida { get; private set; }
        public int sed { get; private set; }
        public int hambre { get; private set; }

        public bool crouching = false;
        public float velocity = 3;
        public float terminalVelocity = 10;
        public float fallStrength = 0;
        private float nextFall = 0;
        public float playerHeight = 20;
        private bool canJump = true;
        public void jump()
        {
            if (fallStrength == 0 && canJump)
            {
                canJump = false;
                fallStrength = -100;
                nextFall = 1;
            }
        }
        public void crouch()
        {
            if (fallStrength == 0)
            {
                crouching = true;
                playerHeight = 10;
            }

        }
        public void stand()
        {
            playerHeight = 20;
            crouching = false;
        }
        public void fall()
        {
            if (fallStrength < 10)
            {
                nextFall = nextFall * 1.3f;
                if (nextFall < terminalVelocity)
                {
                    nextFall = terminalVelocity;
                }
                fallStrength = fallStrength + nextFall;
            }

        }
        private void reenableJump(Object source, System.Timers.ElapsedEventArgs e)
        {
            reenableJumpTimer.Dispose();
            canJump = true;
        
        }
        System.Timers.Timer reenableJumpTimer; 
        public void golpearPiso()
        {
            reenableJumpTimer = new System.Timers.Timer(500);
            aTimer.Enabled = true;
            aTimer.Elapsed += reenableJump;
            fallStrength = 0;
            nextFall = 0;
        }
        public Vector3 position = new Vector3(0, 0, 0);
        public int pesoMaximo { get; private set; }
        public bool estaConstruyendo { get { return (construyendo != null); } }
        public GameObjectAbstract construyendo { get; private set; }

        public int maximaSed  { get; private set; }
        public int maximaHambre  { get; private set; }
        public int maximaVida { get; private set; }
        public Dictionary<String,ObjetoInventario> equipoEnUso;

        public int cantDanioPorCalor = 0;
        public int cantDanioPorFrio = 0;


        
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
            maximaSed = 180;
            maximaHambre = 240;
            maximaVida = 100;
            pesoMaximo = 100;
            vida = maximaVida;
            hambre = 0; sed = 0;
            deBuffes = new List<string>();
            inv = new ModeloInventario();

            inv.equipoEnUso = equipoEnUso;
            // Create a timer para el daño por Frio y Calor que sea cada segundo
            aTimer = new System.Timers.Timer(1000);
            aTimer.Enabled = true; 
            aTimer.Elapsed += OnTimedEvent; 
        }
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            danioPorCalor(cantDanioPorCalor);
            danioPorFrio(cantDanioPorFrio);
        }

        public bool estaEquipadaParteDelCuerpo(string parteDelCuerpo)
        {
            ObjetoInventario objeto = null;
            if (equipoEnUso.TryGetValue(parteDelCuerpo, out objeto))
            {
                return (objeto != null);
            }
            return false;
        }

        public ObjetoInventario desequiparObjetoDeParteDelCuerpo(String parteDelCuerpo)        //desequipar
        {
            ObjetoInventario objetoDesequipado = null;
            equipoEnUso.TryGetValue(parteDelCuerpo, out objetoDesequipado);
            if (objetoDesequipado != null)
            {
                equipoEnUso.Remove(parteDelCuerpo);
                equipoEnUso.Add(parteDelCuerpo, null);
                inv.agregar(objetoDesequipado);
            }
            huboCambios();
            return objetoDesequipado;
        }
        public ObjetoInventario equiparObjetoEnParteDelCuerpo(String parteDelCuerpo, ObjetoInventario objetoAEquipar)        //equipar
        {
            ObjetoInventario objetoDesequipado = null;
            objetoDesequipado = desequiparObjetoDeParteDelCuerpo(parteDelCuerpo);
            equipoEnUso.Remove(parteDelCuerpo);
            equipoEnUso.Add(parteDelCuerpo, objetoAEquipar);
            huboCambios();
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
        public void empezarConstruccion(GameObjectAbstract aConstruir)
        {
            if (aConstruir != null)
            {
                construyendo = aConstruir;
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
            int danioFinal = cantidadDanio;
            this.addLevelSed(danioFinal);
        }
        public void danioPorFrio(int cantidadDanio)
        {
            int danioFinal = cantidadDanio;
            if (this.estaEquipadaParteDelCuerpo(TORSO) && cantidadDanio>0) danioFinal = cantidadDanio - 1;
            this.addLevelHambre(danioFinal);
        }
    }
}
