using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using AlumnoEjemplos.RamboAxe.Player;
using AlumnoEjemplos.RamboAxe.Inventario;

namespace AlumnoEjemplos.RamboAxe.GameObjects
{
    public class Dropbox: GameObjectAbstract
    {
        private Dictionary<ObjetoInventario, int> cantidadPorObjeto;
        bool recuperando;
        public bool esVisible;

        public Dropbox(float x, float y, float z)
            : base(x, y-10, z)
        {
            delayUso = 7f;
            loadMeshes(GuiController.Instance.AlumnoEjemplosDir + "\\Ramboaxe\\Media\\dropbox\\CardboardBox-TgcScene.xml");
            cantidadPorObjeto = new Dictionary<ObjetoInventario, int>();
            recuperando = false;
            esVisible = false;
            resize(0, 0, 0);
        }
        public  override InteractuableResponse use()
        {
            if(recuperando){
                return null;
            }
            if(cantidadPorObjeto.Count <= 0){
                return null;
            }
            recuperando = true;
            bool agregoTodos = true;
            for (int index = 0; index < cantidadPorObjeto.Count; index++ )
            {
                KeyValuePair<ObjetoInventario, int> keyPair = cantidadPorObjeto.ElementAt(index);
                if (agregoTodos)
                {
                    agregoTodos = CharacterSheet.getInstance().getInventario().agregar(keyPair.Key, keyPair.Value);
                    if (agregoTodos)
                    {
                        cantidadPorObjeto[keyPair.Key] = 0;
                    }
                }
            }
            if(agregoTodos){
                cantidadPorObjeto.Clear();
                esVisible = false;
                resize(0, 0, 0);
            }
            recuperando = false;
            return null;
        }

        public void agregar(ObjetoInventario obj)
        {
            if(!esVisible){
                esVisible = true;
                resize(20, 20, 20);
            }
            int cantidad = 0;
            if(cantidadPorObjeto.TryGetValue(obj, out cantidad)){
                cantidad++;
                cantidadPorObjeto[obj] = cantidad;
            }
            else
            {
                cantidad = 1;
                cantidadPorObjeto.Add(obj, cantidad);
            }
        }
    }
}
