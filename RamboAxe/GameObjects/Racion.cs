using AlumnoEjemplos.RamboAxe.Player;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class Racion:GameObjectAbstract
    {
        float uses = 2;
        public Racion(TgcMesh mesh,int x, int y,int z):base(mesh,x,y,z)
        {
            
        }
         public  override InteractuableResponse use()
        {
            Objeto obj1 = new Objeto();
            obj1.nombre = "Racion";
            if (uses > 0)
            {
                uses--;
                CharacterSheet.getInstance().getInventario().agregar(obj1);
            }
            return null;
        }

 
    }
}
