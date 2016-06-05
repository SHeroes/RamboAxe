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
    class RuinaPared:GameObjectAbstract
    {
        CharacterSheet pj = CharacterSheet.getInstance();
        float uses = 2;
        
        public RuinaPared(float x, float y,float z):base(x,y,z)
        {
            delayUso = 0f;
            loadMeshes(GuiController.Instance.AlumnoEjemplosDir + "\\Ramboaxe\\Media\\ruinas_con_bound-TgcScene.xml");
            
        }
         public  override InteractuableResponse use()
        {
          //  Objeto obj1 = new Objeto();
          //  obj1.nombre = "Agua";
         
         
            return null;
        }

 
    }
}
