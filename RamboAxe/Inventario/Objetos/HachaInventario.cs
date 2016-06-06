using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Player;

namespace AlumnoEjemplos.RamboAxe.Inventario.Objetos
{
    public class HachaInventario: ObjetoInventarioEquipable
    {
        public HachaInventario():base("Hacha", CharacterSheet.MANO_DERECHA)
        {

        }

        public override void usar()
        {
            /*
            if (!hachaEquipada)
            {
                hachaEquipada = true;
                GuiController.Instance.Drawer2D.beginDrawSprite();
                spriteHacha.render();
                GuiController.Instance.Drawer2D.endDrawSprite();
            }*/
            base.usar();
        }
    }
}
