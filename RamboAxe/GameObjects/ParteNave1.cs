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
    class ParteNave1:GameObjectAbstract
    {
        float uses = 3;
        
        public ParteNave1(float x,float y, float z): base(x, y-10, z)
        {
            delayUso = 20f;
            loadMeshes(GuiController.Instance.AlumnoEjemplosDir + "\\Ramboaxe\\Media\\escenario\\parte_nave_1-TgcScene.xml");
            resize(65,65,65);
        }
        public  override InteractuableResponse use()
        {
           
            return null;
        }
        
 
    }
}
