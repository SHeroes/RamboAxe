﻿using AlumnoEjemplos.RamboAxe.Player;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using AlumnoEjemplos.RamboAxe.Inventario;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class KitSalud : GameObjectAbstract
    {
        float uses = 1;
        public KitSalud(TgcMesh mesh, int x, int y, int z)
            : base(mesh, x, y, z)
        {

        }
        public override InteractuableResponse use()
        {
            uses--;
            if (uses >= 0)
            {
                CharacterSheet.getInstance().getInventario().agregar(InventarioManager.KitSalud);
                if (uses == 0)
                {
                    getMesh().Scale = new Vector3(0f, 0f, 0f);
                }
            }
            return null;
        }


    }
}
