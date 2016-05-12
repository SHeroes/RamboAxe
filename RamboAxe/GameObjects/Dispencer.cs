using AlumnoEjemplos.RamboAxe.Player;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class Dispencer:GameObjectAbstract
    {
        float uses = 2;
        public Dispencer(TgcMesh mesh,int x, int y,int z):base(mesh,x,y,z)
        {
            delayUso = 3f;   
        }
         public  override InteractuableResponse use()
        {
          //  Objeto obj1 = new Objeto();
          //  obj1.nombre = "Agua";
            uses--;
            if (uses >= 0)
            {
//                base.getMesh().Scale = new Vector3((float)(uses * 0.), (float)(uses * 0.5), (float)(uses * 0.5));
               // base.getMesh().Position = new Vector3(getMesh().Position.X, 0, getMesh().Position.Z);
               // base.getMesh().updateBoundingBox();
                //haracterSheet.getInstance().getInventario().agregar(obj1);
                EjemploAlumno.getInstance().getBarraHidratacion().agregarPorcentajeABarra(0.2f);
                if (uses == 0)
                {
                    getMesh().Scale = new Vector3(0f, 0f, 0f);
                }
            }
            return null;
        }

 
    }
}
