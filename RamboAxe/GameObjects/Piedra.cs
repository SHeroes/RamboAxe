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
    class Piedra : GameObjectAbstract
    {
        float uses = 1;
        public Piedra(int x, int y, int z)
            : base( x, y, z)
        {
            delayUso = 4f;
        }
        public override InteractuableResponse use()
        {
            uses--;
            if (uses >= 0)
            {
                //                base.getMesh().Scale = new Vector3((float)(uses * 0.), (float)(uses * 0.5), (float)(uses * 0.5));
                // base.getMesh().Position = new Vector3(getMesh().Position.X, 0, getMesh().Position.Z);
                // base.getMesh().updateBoundingBox();
                //haracterSheet.getInstance().getInventario().agregar(obj1);

                CharacterSheet.getInstance().getInventario().agregar(InventarioManager.Piedra);


                if (uses == 0)
                {

                    getMesh().Scale = new Vector3(0f, 0f, 0f);
                }
            }
            return null;
        }


    }
}
