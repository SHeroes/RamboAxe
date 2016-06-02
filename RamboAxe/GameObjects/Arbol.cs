using AlumnoEjemplos.RamboAxe.Player;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using AlumnoEjemplos.RamboAxe.Inventario;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class Arbol:GameObjectAbstract
    {
        float uses = 2;
        
        public Arbol(TgcMesh mesh,float x,float y, float z): base(mesh, x, y, z)
        {
            delayUso = 7f;   
        }
        public  override InteractuableResponse use()
        {
            uses--;
            if (uses >= 0)
            {
//                base.getMesh().Scale = new Vector3((float)(uses * 0.), (float)(uses * 0.5), (float)(uses * 0.5));
               // base.getMesh().Position = new Vector3(getMesh().Position.X, 0, getMesh().Position.Z);
               // base.getMesh().updateBoundingBox();
                //haracterSheet.getInstance().getInventario().agregar(obj1);

                CharacterSheet.getInstance().getInventario().agregar(InventarioManager.Ramita);

    
                if (uses == 0)
                {
          
                    getMesh().Scale = new Vector3(0f, 0f, 0f);
                }
            }
            return null;
        }
         public override void place(int x, int y, int z)
         {
             base.place(x, y, z);
             this.mesh = mesh.clone("arbolin_" + x.ToString() + z.ToString());
         }
 
    }
}
