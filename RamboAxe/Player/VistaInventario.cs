using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils._2D;
using AlumnoEjemplos.RamboAxe.Inventario;

namespace AlumnoEjemplos.RamboAxe.Player
{
    public class VistaInventario
    {
        ModeloInventario inv;

        TgcText2d itemsTitle;
        TgcText2d items;
        TgcText2d recetasTitle;
        TgcText2d recetasText;
        TgcText2d ingredientesTitle;
        TgcText2d ingredientesText;
        TgcSprite back;
        TgcSprite selector;
        TgcSprite tabBackground;
        TgcText2d tabText;
        Rectangle borderStart = new Rectangle(139, 3, 10, 37);
        Rectangle borderMiddle = new Rectangle(149, 3, 10, 37);
        Rectangle borderEnd = new Rectangle(334, 3, 10, 37);
        Rectangle selectorBorder;
        Rectangle selectorMiddle;
        Vector2 auxPoint;
        Vector2 upperLeftCorner;

        int currentRow;
        /* Indica si se esta recorriendo las recetas o los items */
        public bool esReceta { get; private set; }

        public bool abierto { get; private set; }

        public VistaInventario()
        {

            string basePath = GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\inventario\\";
            /* Inicializacion de los datos */
            inv = new ModeloInventario();
            esReceta = true;
            abierto = false;
            currentRow = -1;
            /* Inicializacion del fondo y calculo de la posicion centrada */
            back = new TgcSprite();
            back.Texture = TgcTexture.createTexture(GuiController.Instance.D3dDevice, basePath + "FFIandFFIIGBAFontsSheet.png");
            back.SrcRect = new Rectangle(0, back.Texture.Height / 2, back.Texture.Width * 2 / 3, back.Texture.Height / 2);
            back.Scaling = new Vector2(1.5f, 1.5f);
            int ScreenWidth = GuiController.Instance.D3dDevice.Viewport.Width;
            int ScreenHeight = GuiController.Instance.D3dDevice.Viewport.Height;
            upperLeftCorner = new Vector2((ScreenWidth - back.SrcRect.Width * back.Scaling.X) / 2, (ScreenHeight - back.SrcRect.Height * back.Scaling.Y) / 2);
            upperLeftCorner.X = (float)Math.Floor(upperLeftCorner.X);
            upperLeftCorner.Y = (float)Math.Floor(upperLeftCorner.Y);
            back.Position = upperLeftCorner;
            tabBackground = new TgcSprite();
            tabBackground.Texture = back.Texture;
            tabBackground.SrcRect = new Rectangle(139, 3, 204, 37);
            tabBackground.Position = new Vector2(upperLeftCorner.X, upperLeftCorner.Y - 40);
            /* Inicializacion del titulo de los items */
            itemsTitle = new TgcText2d();
            itemsTitle.Position = new Point(((int)upperLeftCorner.X) + 20, ((int)upperLeftCorner.Y) + 20);
            itemsTitle.Align = TgcText2d.TextAlign.LEFT;
            itemsTitle.Size = new Size(back.SrcRect.Width * 3 / 2, back.SrcRect.Height * 3 / 2);
            itemsTitle.Text = "Inventario:";
            itemsTitle.Color = Color.White;
            /* Inicializacion del texto para los items */
            items = new TgcText2d();
            items.Position = new Point(((int)upperLeftCorner.X) + 40, ((int)upperLeftCorner.Y) + 47);
            items.Align = TgcText2d.TextAlign.LEFT;
            items.Size = itemsTitle.Size;
            items.Text = "";
            items.Color = Color.White;
            /* Inicializacion del selector */
            auxPoint = new Vector2(0, 0);
            selector = new TgcSprite();
            selector.Position = auxPoint;
            selector.Texture = TgcTexture.createTexture(GuiController.Instance.D3dDevice, basePath + "borde.png");
            selector.Scaling = new Vector2(0.5f, 0.5f);
            int selectorBorderSize = 4 * (selector.Texture.Width / 50);
            selectorMiddle = new Rectangle(selectorBorderSize, 0, selector.Texture.Width - 2 * selectorBorderSize, selector.Texture.Height);
            selectorBorder = new Rectangle(0, 0, selectorBorderSize, selector.Texture.Height);
            /* Inicializacion del titulo de las recetas */
            recetasTitle = new TgcText2d();
            recetasTitle.Position = new Point((int)(upperLeftCorner.X + selectorMiddle.Width * 2.0f) + 20, (int)(upperLeftCorner.Y) + 20);
            recetasTitle.Align = TgcText2d.TextAlign.LEFT;
            recetasTitle.Size = new Size(back.SrcRect.Width * 3 / 2, back.SrcRect.Height * 3 / 2);
            recetasTitle.Text = "Recetas:";
            recetasTitle.Color = Color.White;
            /* Inicializacion del texto para las recetas */
            recetasText = new TgcText2d();
            recetasText.Position = new Point((int)(upperLeftCorner.X + selectorMiddle.Width * 2.0f) + 40, (int)(upperLeftCorner.Y) + 47);
            recetasText.Align = TgcText2d.TextAlign.LEFT;
            recetasText.Size = itemsTitle.Size;
            recetasText.Text = "";
            recetasText.Color = Color.White;
            /* Inicializacion del titulo de las recetas */
            ingredientesTitle = new TgcText2d();
            ingredientesTitle.Position = new Point((int)(upperLeftCorner.X + selectorMiddle.Width * 4.0f) + 20, (int)(upperLeftCorner.Y) + 20);
            ingredientesTitle.Align = TgcText2d.TextAlign.LEFT;
            ingredientesTitle.Size = new Size(back.SrcRect.Width * 3 / 2, back.SrcRect.Height * 3 / 2);
            ingredientesTitle.Text = "Ingredientes:";
            ingredientesTitle.Color = Color.White;
            /* Inicializacion del texto para las recetas */
            ingredientesText = new TgcText2d();
            ingredientesText.Position = new Point((int)(upperLeftCorner.X + selectorMiddle.Width * 4.0f) + 40, (int)(upperLeftCorner.Y) + 47);
            ingredientesText.Align = TgcText2d.TextAlign.LEFT;
            ingredientesText.Size = itemsTitle.Size;
            ingredientesText.Text = "";
            ingredientesText.Color = Color.White;
        }

        public void render()
        {
            // Si esta abierto dibuja el fondo primero y despues el texto
            if(abierto){
                GuiController.Instance.Drawer2D.beginDrawSprite();
                /*Rectangle rectangle = new Rectangle(
                    (int)GuiController.Instance.Modifiers["x"],
                    (int)GuiController.Instance.Modifiers["y"],
                    (int)GuiController.Instance.Modifiers["width"],
                    (int)GuiController.Instance.Modifiers["height"]
                );
                back.SrcRect = rectangle;*/
                back.render();
                /* Start tab render */
                int realWidth = (int) Math.Floor(back.SrcRect.Width * 1.5f);
                int actualWidth = 10;
                auxPoint.X = back.Position.X;
                auxPoint.Y = tabBackground.Position.Y;
                tabBackground.Position = auxPoint;
                tabBackground.SrcRect = borderStart;
                tabBackground.render();
                tabBackground.SrcRect = borderMiddle;
                while(actualWidth < (realWidth - 10)){
                    actualWidth += 10;
                    auxPoint.X += 10;
                    tabBackground.Position = auxPoint;
                    tabBackground.render();
                }
                tabBackground.SrcRect = borderEnd;
                auxPoint.X = back.Position.X + realWidth - 10;
                tabBackground.Position = auxPoint;
                tabBackground.render();
                //Console.WriteLine("Actual Width: {0} Start Width: {1}", actualWidth, realWidth);
                /* End tab render */
                if(currentRow != -1){
                    int leftPadding = 0;
                    if(esReceta){
                        leftPadding = selectorMiddle.Width * 2;
                    }
                    renderLine(selector, currentRow, leftPadding);
                }
                GuiController.Instance.Drawer2D.endDrawSprite();
                itemsTitle.render();
                items.render();
                recetasTitle.render();
                recetasText.render();
                ingredientesText.render();
                ingredientesTitle.render();
            }
        }

        private void renderTab(string tabName, int tabIndex, int tabWidth)
        {
            /* Start position + Tab padding */
            auxPoint.X = back.Position.X + tabWidth * tabIndex;
            /* Start height - Tab Height */
            auxPoint.Y = back.Position.Y - 40;
            /* Render Tab Start */
            int actualWidth = borderMiddle.Width;
            tabBackground.Position = auxPoint;
            tabBackground.SrcRect = borderStart;
            tabBackground.render();
            /* Render Tab Middles until Full Width */
            tabBackground.SrcRect = borderMiddle;
            while (actualWidth < (tabWidth - borderMiddle.Width))
            {
                actualWidth += borderMiddle.Width;
                auxPoint.X += borderMiddle.Width;
                tabBackground.Position = auxPoint;
                tabBackground.render();
            }
            /* Render Tab End */
            tabBackground.SrcRect = borderEnd;
            auxPoint.X = back.Position.X + tabWidth - borderMiddle.Width;
            tabBackground.Position = auxPoint;
            tabBackground.render();
        }

        private void renderLine(TgcSprite lineSprite, int position, int leftPadding = 0)
        {
            int widthTick = selectorMiddle.Width / 2;
            auxPoint.X = 30 + leftPadding + upperLeftCorner.X;
            auxPoint.Y = 40 + upperLeftCorner.Y + position * (selectorBorder.Height / 2);
            lineSprite.Position = auxPoint;
            lineSprite.SrcRect = selectorBorder;
            lineSprite.render();
            lineSprite.SrcRect = selectorMiddle;
            lineSprite.render();
            auxPoint.X += widthTick;
            lineSprite.Position = auxPoint;
            lineSprite.render();
            auxPoint.X += widthTick;
            lineSprite.Position = auxPoint;
            lineSprite.render();
            lineSprite.SrcRect = selectorBorder;
            auxPoint.X += widthTick;
            lineSprite.Position = auxPoint;
            lineSprite.render();
        }

        public void dispose()
        {
            selector.Texture.dispose();
            selector.dispose();
            back.Texture.dispose();
            back.dispose();
            items.dispose();
            itemsTitle.dispose();
            recetasTitle.dispose();
            recetasText.dispose();
            ingredientesText.dispose();
            ingredientesTitle.dispose();
           
        }

        public void abrir()
        {
            abierto = true;
        }

        public void cerrar()
        {
            abierto = false;
        }

        public void agregarReceta(Receta receta)
        {
            inv.agregar(receta);
            recetasText.Text = generateRecetasText();
        }

        /// <summary>
        /// Agrega un nuevo elemento al inventario
        /// </summary>
        /// <param name="obj"></param>
        public void agregar(ObjetoInventario obj)
        {
            inv.agregar(obj);
            items.Text = generateItemText();
        }

        /// <summary>
        /// Devuelve la cantitad del objeto dado en el inventario
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int contar(ObjetoInventario obj)
        {
            return inv.cantidadPorObjeto(obj);
        }

        /// <summary>
        /// Genera una representacion textual del inventario
        /// </summary>
        /// <returns></returns>
        private string generateItemText()
        {
            string itemList = "";
            for (int posicion = 0; posicion < inv.contarObjetos(); posicion++)
            {
                ObjetoInventario objeto = inv.obtenerObjetoEnPosicion(posicion);
                int cantidad = inv.cantidadPorObjeto(objeto);
                itemList += objeto.nombre + " x" + cantidad + "\n\n";
            }
            return itemList;
        }

        /// <summary>
        /// Genera una representacion textual de las recetas
        /// </summary>
        /// <returns></returns>
        private string generateRecetasText()
        {
            string itemList = "";
            for (int posicion = 0; posicion < inv.contarRecetas(); posicion ++ )
            {
                Receta receta = inv.obtenerRecetaEnPosicion(posicion);
                itemList += receta.resultado.nombre + " x" + receta.cantidadResultado + "\n\n";
            }
            return itemList;
        }

        /// <summary>
        /// Genera una representacion textual del inventario
        /// </summary>
        /// <returns></returns>
        private string generateIngredientesText()
        {
            if(!esReceta){
                return ingredientesText.Text;
            }
            if(currentRow == -1){
                return "";
            }
            Receta receta = inv.obtenerRecetaEnPosicion(currentRow);
            string itemList = "";
            foreach (string name in receta.ingredientes)
            {
                int count;
                if (!receta.cantidadIngrediente.TryGetValue(name, out count))
                {
                    count = 0;
                }
                itemList += name + " x" + count + "\n\n";
            }
            return itemList;
        }

        public void siguienteItem()
        {
            if (currentRow >= (seleccionCount() - 1))
            {
                if (seleccionCount() == 0)
                {
                    currentRow = -1;
                }
                else
                {
                    currentRow = 0;
                }
            }
            else
            {
                currentRow++;
            }
            ingredientesText.Text = generateIngredientesText();
        }

        public void anteriorItem()
        {
            if (currentRow <= 0)
            {
                currentRow = seleccionCount() - 1;
            }
            else
            {
                currentRow--;
            }
            ingredientesText.Text = generateIngredientesText();
        }

        private int seleccionCount()
        {
            if(esReceta){
                return inv.contarRecetas();
            }
            else
            {
                return inv.contarObjetos();
            }
        }

        public void invertirSeleccion()
        {
            esReceta = !esReceta;
            if (currentRow > (seleccionCount() - 1))
            {
                currentRow = (seleccionCount() - 1);
            }
        }

        public string consumirActual()
        {
            if (!esReceta)
            {
                ObjetoInventario objeto = inv.obtenerObjetoEnPosicion(currentRow);
                if (inv.consumir(currentRow))
                {
                    items.Text = generateItemText();
                    return objeto.nombre;
                }
            }
            return null;
        }

        public void fabricarActual()
        {
            if (esReceta)
            {
                inv.fabricar(currentRow);
            }
        }
    }
}