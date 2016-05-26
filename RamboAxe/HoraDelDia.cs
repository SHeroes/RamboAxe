using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.RamboAxe
{
    public class HoraDelDia
    {
        private static int tiempoDelCiclo = 120;
        private static System.Timers.Timer aTimer;
        private static HoraDelDia instancia;
        private float tiempoTranscurridoDelCiclo;
        private float horaDelDia;
        private int momentoDia;
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
            momentoDia = momentoDelDia();
        }
        private float setHoraDia()
        { // mayor a 0.5 es que paso la mitad del dia

            horaDelDia =  (float)tiempoTranscurridoDelCiclo / tiempoDelCiclo;
            return horaDelDia;
        }      
        public float getHoraDia(){ // mayor a 0.5 es que paso la mitad del dia
            return horaDelDia;
        }
        public int getMomentoDelDia() {
            return momentoDia;
        }
        public bool esDeDia() {
            if (tiempoTranscurridoDelCiclo > tiempoDelCiclo / 2)
            {
                return false;
            }
            else {
                return true;
            }

        }
        private int momentoDelDia (){
            if (horaDelDia > 0.66) return -1; //noche 
            else if (horaDelDia > 0.33) return 1; //mediodia
            else return 0;
        }
        public static HoraDelDia getInstance() {
            if (instancia == null) { 
                instancia = new HoraDelDia();
            }
            return instancia;
        }

    }
}
