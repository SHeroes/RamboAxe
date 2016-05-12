using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    class Obstaculo:GameObjectAbstract
    {
        public Obstaculo(TgcMesh mesh, int x, int y, int z): base(mesh, x, y, z)
        {
            this.delayUso = 0;
        }
        public override InteractuableResponse use(){
            return null;

        }
    }
}
