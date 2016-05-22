using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlumnoEjemplos.RamboAxe
{
    public interface Observador
    {
        void cambioObservable();
    }

    public abstract class Observable
    {
        protected Observador observador;

        public void agregarObservador(Observador obs)
        {
            this.observador = obs;
        }

        public void sacarObservador(Observador obs)
        {
            this.observador = null;
        }

        protected void huboCambios()
        {
            if(observador != null){
                observador.cambioObservable();
            }
        }
    }
}
