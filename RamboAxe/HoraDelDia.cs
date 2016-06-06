using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;

namespace AlumnoEjemplos.RamboAxe
{
    public class HoraDelDia
    {
        private static int tiempoDelCiclo = 120;
        private static System.Timers.Timer aTimer;
        private static HoraDelDia instancia;
        private float tiempoTranscurridoDelCiclo;
        private float horaDelDia;
        private int momentoDelDia;

        CharacterSheet pj = CharacterSheet.getInstance();
        private HoraDelDia() {
            tiempoTranscurridoDelCiclo = 0;
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(1000);
            // Start the timer
            aTimer.Enabled = true;
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent; 
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            tiempoTranscurridoDelCiclo++;
            if (tiempoTranscurridoDelCiclo > tiempoDelCiclo)
            {
                tiempoTranscurridoDelCiclo = 0;
            }
            setHoraDia();
            this.momentoDelDia = setMomentoDelDia();

            // test de que aumente la sed.... /// recordar que se esta ejecutando cada 1 segundo  aTimer = new System.Timers.Timer(1000);
           // pj.addLevelSed(1); 
            //
            // test de que aumente la sed.... /// recordar que se esta ejecutando cada 1 segundo  aTimer = new System.Timers.Timer(1000);
          //  pj.addLevelHambre(1);
            //
           
        }
        private float setHoraDia()
        { // mayor a 0.5 es que paso la mitad del dia
            horaDelDia =  (float)tiempoTranscurridoDelCiclo / tiempoDelCiclo;
            return horaDelDia;
        }      
        public float getHoraDelDia(){ // mayor a 0.5 es que paso la mitad del dia
            return horaDelDia;
        }

        public bool isAM() {
            if (tiempoTranscurridoDelCiclo > tiempoDelCiclo / 2)
            {
                return false;
            }
            else {
                return true;
            }
        }

        public static HoraDelDia getInstance() {
            if (instancia == null) { 
                instancia = new HoraDelDia();
            }
            return instancia;
        }
        public string getHoraEnString() {
            float cantHorasTranscurridas = this.horaDelDia*24;
            string horaDiaString = "";
            if (cantHorasTranscurridas < 4)         horaDiaString = "NOCHE";
            else if (cantHorasTranscurridas < 7)    horaDiaString = "AMANECER";
            else if (cantHorasTranscurridas < 11)   horaDiaString = "MAÑANA";
            else if (cantHorasTranscurridas < 14)   horaDiaString = "MEDIODÍA";
            else if (cantHorasTranscurridas < 17)   horaDiaString = "TARDE";
            else if (cantHorasTranscurridas < 19)   horaDiaString = "OCASO";
            else                                    horaDiaString = "NOCHE";
            return horaDiaString;
        }

        private int setMomentoDelDia()
        {
            if (horaDelDia > 0.66) return -1; //noche 
            else if (horaDelDia > 0.33) return 1; //mediodia
            else return 0;
        }
        public int getMomentoDelDia()
        {
            return momentoDelDia;
        }

    }
}
