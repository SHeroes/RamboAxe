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
    class ArbolHongoGo:GameObjectAbstract
    {
        float uses = 2;
        
        public ArbolHongoGo(float x,float y, float z): base(x, y-5, z)
        {
            delayUso = 1f;
            loadMeshes(GuiController.Instance.AlumnoEjemplosDir + "\\Ramboaxe\\Media\\escenario\\cristal_verde-TgcScene.xml");
            resize(30, 50, 30);
        }
        public  override InteractuableResponse use()
        {
            uses--;
            if (uses >= 0)
            {
                resize(15, 25, 15);
//                base.getMesh().Scale = new Vector3((float)(uses * 0.), (float)(uses * 0.5), (float)(uses * 0.5));
               // base.getMesh().Position = new Vector3(getMesh().Position.X, 0, getMesh().Position.Z);
               // base.getMesh().updateBoundingBox();
                //haracterSheet.getInstance().getInventario().agregar(obj1);
                if (uses == 0)
                {
                    resize(0,0,0);
                  
                }
            }
            return null;
        }
         public override void place(float x, float y, float z)
         {
             base.place(x, y, z);
             this.mesh = mesh.clone("ArbolHongoGoin_" + x.ToString() + z.ToString());
         }
 
    }
}
