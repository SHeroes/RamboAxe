using AlumnoEjemplos.RamboAxe.Player;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class Dispencer:GameObjectAbstract
    {
        CharacterSheet pj = CharacterSheet.getInstance();
        float uses = 2;
        public Dispencer(float x, float y,float z):base(x,y,z)
        {
            delayUso = 2f;
            this.loadMeshes(GuiController.Instance.AlumnoEjemplosDir + "\\Ramboaxe\\Media\\dispenser\\DispenserAgua-TgcScene.xml");
        }
        public Dispencer(string mesh, float x, float y, float z)
            : base(mesh, x, y, z)
        {
            delayUso = 2f;
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


             //   EjemploAlumno.getInstance().getBarraHidratacion().agregarPorcentajeABarra(0.2f);
                pj.addLevelSed(-20);
                if (uses == 0)
                {
                    getMesh().Scale = new Vector3(0f, 0f, 0f);
                }
            }
            return null;
        }

 
    }
}
