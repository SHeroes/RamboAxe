using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils._2D;

namespace AlumnoEjemplos.Inventario
{
    public class Inventario
    {

        TgcText2d text;
        TgcSprite back;

        List<string> itemName;
        Dictionary<string, int> itemCount;

        public bool abierto { get; private set; }

        public Inventario()
        {
            /* Inicializacion de los datos */
            abierto = false;
            itemName = new List<string>();
            itemCount = new Dictionary<string, int>();
            /* Inicializacion del fondo */
            back = new TgcSprite();
            back.Position = new Vector2(0, 0);
            back.Texture = TgcTexture.createTexture(GuiController.Instance.D3dDevice, GuiController.Instance.AlumnoEjemplosMediaDir + "inventario\\" + "FFIandFFIIGBAFontsSheet.png");
            back.SrcRect = new Rectangle(0, back.Texture.Height / 2, back.Texture.Width * 2 / 3, back.Texture.Height / 2);
            back.Scaling = new Vector2(1.5f, 1.5f);
            /* Inicializacion del texto */
            text = new TgcText2d();
            text.Position = new Point(10, 10);
            text.Align = TgcText2d.TextAlign.LEFT;
            text.Size = new Size(back.SrcRect.Width * 3 / 2, back.SrcRect.Height * 3 / 2);
            text.Text = "Presione I para cerrar el inventario. Presione numeros del 1 al 3 para agregar objetos.";
            text.Color = Color.White;
        }

        public void render()
        {
            // Si esta abierto dibuja el fondo primero y despues el texto
            if(abierto){
                GuiController.Instance.Drawer2D.beginDrawSprite();
                back.render();
                GuiController.Instance.Drawer2D.endDrawSprite();
                text.render();
            }
        }

        public void dispose()
        {
            back.Texture.dispose();
            back.dispose();
            text.dispose();
        }

        public void abrir()
        {
            abierto = true;
        }

        public void cerrar()
        {
            abierto = false;
        }

        /// <summary>
        /// Agrega un nuevo elemento al inventario
        /// </summary>
        /// <param name="obj"></param>
        public void agregar(Objeto obj)
        {
            int currentCount;
            if(itemCount.TryGetValue(obj.nombre, out currentCount)){
                currentCount++;
                itemCount[obj.nombre] = currentCount;
            }
            else
            {
                currentCount = 1;
                itemName.Add(obj.nombre);
                itemCount.Add(obj.nombre, currentCount);
            }
            text.Text = generateItemText();
        }

        /// <summary>
        /// Devuelve la cantitad del objeto dado en el inventario
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int contar(Objeto obj)
        {
            int currentCount;
            if(!itemCount.TryGetValue(obj.nombre, out currentCount)){
                currentCount = 0;
            }
            return currentCount;
        }

        /// <summary>
        /// Genera una representacion textual del inventario
        /// </summary>
        /// <returns></returns>
        private string generateItemText()
        {
            string itemList = "";
            foreach(string name in itemName){
                int count;
                if(!itemCount.TryGetValue(name, out count)){
                    count = 0;
                }
                itemList += name + " x" + count + "\n";
            }
            return itemList;
        }
    }
}
