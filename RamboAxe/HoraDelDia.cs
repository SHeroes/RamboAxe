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
        }
        
        public float getHoraDia(){ // mayor a 0.5 es que paso la mitad del dia
            return (float)tiempoTranscurridoDelCiclo / tiempoDelCiclo;
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

        public static HoraDelDia getInstance() {
            if (instancia == null) { 
                instancia = new HoraDelDia();
            }
            return instancia;
        }

    }
}
