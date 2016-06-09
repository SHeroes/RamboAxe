using AlumnoEjemplos.RamboAxe.Player;
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
    class MuchosArboles1:GameObjectAbstract
    {
        float uses = 10;
        
        public MuchosArboles1(float x,float y, float z): base(x, y, z)
        {
            loadMeshes(GuiController.Instance.AlumnoEjemplosDir + "Ramboaxe\\Media\\escenarios\\muchosArboles1-TgcScene.xml");
            // normalizarTamanio();
            resize(2f, 2f, 5f);
            delayUso = 2f;   
        }
        public  override InteractuableResponse use()
        {
            uses--;
            if (uses >= 0)
            {
//                base.getMesh().Scale = new Vector3((float)(uses * 0.), (float)(uses * 0.5), (float)(uses * 0.5));
               // base.getMesh().Position = new Vector3(getMesh().Position.X, 0, getMesh().Position.Z);
               // base.getMesh().updateBoundingBox();
                //haracterSheet.getInstance().getInventario().agregar(obj1);

                CharacterSheet.getInstance().getInventario().agregar(InventarioManager.Ramita);

    
                if (uses == 0)
                {
          
                    getMesh().Scale = new Vector3(0f, 0f, 0f);
                }
            }
            return null;
        }
    }
}
