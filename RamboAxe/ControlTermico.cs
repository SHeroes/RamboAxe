using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.RamboAxe
{
    class ControlTermico
    {
        /*
        private static System.Timers.Timer aTimer;
        private static BarraEstatica barraTermica;
        public void init(BarraEstatica barraTermicaGet)
        {
            barraTermica = barraTermicaGet;
             // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(2000);
            // Start the timer
            aTimer.Enabled = true;
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;          
        }
        private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
          //  barraTermica.valorActual--;

            //testing que baje la temperatura cada 2 segundos en 20 grados
            barraTermica.valorActual = barraTermica.valorActual - 20;
        }
        public void render(double currentCuadrantX,double currentCuadrantZ){
            if (currentCuadrantX > 0) currentCuadrantX = -currentCuadrantX;
            barraTermica.valorActual = (float)(20 - currentCuadrantX - currentCuadrantZ);
        }
        */


    }
}
