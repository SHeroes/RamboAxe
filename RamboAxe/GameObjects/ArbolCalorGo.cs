using AlumnoEjemplos.RamboAxe.Player;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using AlumnoEjemplos.RamboAxe.Inventario;
using TgcViewer;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class ArbolCalorGo:GameObjectAbstract
    {
        float uses = 3;
        
        public ArbolCalorGo(float x,float y, float z): base(x, y-10, z)
        {
            delayUso = 7f;
            loadMeshes(GuiController.Instance.AlumnoEjemplosDir + "\\Ramboaxe\\Media\\escenario\\cristal_calor-TgcScene.xml");
            resize(30,70,30);
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

                resize(10*uses,10*uses*2.5f,10*uses);
    
                if (uses == 0)
                {
          
                  
                }
            }
            return null;
        }
         public override void place(float x, float y, float z)
         {
             base.place(x, y, z);
             this.mesh = mesh.clone("ArbolCalorGoin_" + x.ToString() + z.ToString());
         }
 
    }
}
