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
    class ParteNave2:GameObjectAbstract
    {
        float uses = 3;
        
        public ParteNave2(float x,float y, float z): base(x, y-10, z)
        {
            delayUso = 20f;
            loadMeshes(GuiController.Instance.AlumnoEjemplosDir + "\\Ramboaxe\\Media\\escenario\\parte_nave_2-TgcScene.xml");
            resize(90,120,120);
        }
        public  override InteractuableResponse use()
        {
           
            return null;
        }
        
 
    }
}
