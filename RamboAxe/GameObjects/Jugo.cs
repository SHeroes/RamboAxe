﻿using AlumnoEjemplos.RamboAxe.Player;
using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using AlumnoEjemplos.RamboAxe.Inventario;
using AlumnoEjemplos.RamboAxe.Inventario.Objetos;
using TgcViewer;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class JugoGo:GameObjectAbstract
    {
        float uses = 1;
        public JugoGo(float x, float y,float z):base(x,y+5,z)
        {
            loadMeshes(GuiController.Instance.AlumnoEjemplosDir + "Ramboaxe\\Media\\raciones\\juguito_1-TgcScene.xml");
           // normalizarTamanio();
            resize(2f, 4f, 2f);
        }
         public  override InteractuableResponse use()
        {
            uses--;
             if (uses >= 0)
            {
                CharacterSheet.getInstance().getInventario().agregar(new JugoInventario());
                if (uses == 0)
                {
                    resize(0, 0, 0);
                }
             }
            return null;
        }

 
    }
}