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
    class PinitoGo:GameObjectAbstract
    {
        float uses = 7;
        
        public PinitoGo(float x,float y, float z): base(x, y-10, z)
        {
            delayUso = 7f;
            esBailador = true;
            capacidadMovimiento = 0.6f;
            //loadMeshes(GuiController.Instance.AlumnoEjemplosDir + "\\Ramboaxe\\Media\\escenario\\pinitos-TgcScene.xml");
            //resize(140,160,140);
            loadMeshes(GuiController.Instance.AlumnoEjemplosDir + "\\Ramboaxe\\Media\\escenario\\pinito-TgcScene.xml");
            resize(70, 160, 70);
        }
        public  override InteractuableResponse use()
        {
            uses--;
            if (uses >= 0)
            {
                if (uses < 5)
                {
                    resize(70,80,70);
                }
                if (uses == 0)
                {
                    resize(0,0,0);
                  
                }
            }
            return null;
        }
     
 
    }
}
