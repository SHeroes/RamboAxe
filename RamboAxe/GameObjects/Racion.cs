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
        float uses = 1;
        public Racion(TgcMesh mesh,int x, int y,int z):base(mesh,x,y,z)
        {
            
        }
         public  override InteractuableResponse use()
        {
            uses--;
             if (uses >= 0)
            {
                Objeto obj1 = new Objeto();
                obj1.nombre = "Racion";
                CharacterSheet.getInstance().getInventario().agregar(obj1);
                if (uses == 0)
                {
                    getMesh().Scale = new Vector3(0f, 0f, 0f);
                }
             }
            return null;
        }

 
    }
}
