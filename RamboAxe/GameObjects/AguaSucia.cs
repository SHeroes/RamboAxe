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
    class AguaSuciaGo:GameObjectAbstract
    {
        
        
        public AguaSuciaGo(): base(0,0,0)
        {
            delayUso = 5f;
            
        }
        public  override InteractuableResponse use()
        {
        

            CharacterSheet pj = CharacterSheet.getInstance();

            if (pj.deBuffes.Contains("Intoxicado Leve"))
            {
                CharacterSheet.getInstance().deBuffes.Remove("Intoxicado Leve");
                CharacterSheet.getInstance().deBuffes.Add("Intoxicado");
                pj.addLevelSed(-10);
            }
            else if (pj.deBuffes.Contains("Intoxicado"))
            {
                CharacterSheet.getInstance().deBuffes.Remove("Intoxicado");
                CharacterSheet.getInstance().deBuffes.Add("Intoxicacion Importante");
                pj.addLevelSed(-5);
            }
            else if (pj.deBuffes.Contains("Intoxicacion Importante"))
            {
                pj.addLevelSed(-3);
            }
            else
            {
                CharacterSheet.getInstance().deBuffes.Add("Intoxicado Leve");
                pj.addLevelSed(-30);
            }
            
            return null;
        }
         public override void place(float x, float y, float z)
         {
         }
 
    }
}
