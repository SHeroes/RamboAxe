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
            
        }
         public  override InteractuableResponse use()
        {
            Objeto obj1 = new Objeto();
            obj1.nombre = "Agua";
            if (uses > 0)
            {
                base.getMesh().Scale = new Vector3(--uses, uses, uses);
                base.getMesh().Position = new Vector3(base.getMesh().Position.X, 50, base.getMesh().Position.Z);
                base.getMesh().updateBoundingBox();
                CharacterSheet.getInstance().getInventario().agregar(obj1);
                Game.Game.getInstance().getBarraHidratacion().agregarPorcentajeABarra(0.2f);
            }
            return null;
        }

 
    }
}
