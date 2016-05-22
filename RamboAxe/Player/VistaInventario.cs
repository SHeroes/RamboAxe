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
        Rectangle selectorBorder;
        Rectangle selectorMiddle;
        Vector2 auxPoint;
        Vector2 upperLeftCorner;
        /* Tab render variables */
        TgcSprite tabBackground;
        TgcText2d tabText;
        Point tabTextPosition;
        Rectangle tabBorderStart = new Rectangle(139, 3, 10, 37);
        Rectangle tabBorderMiddle = new Rectangle(149, 3, 10, 37);
        Rectangle tabBorderEnd = new Rectangle(334, 3, 10, 37);
        int tabWidth;
        int currentTab;

        int currentRow;
        /* Indica si se esta recorriendo las recetas o los items */
        public bool esInventario { get { return (currentTab == 0); } }
        public bool esReceta { get { return (currentTab == 1); } }
        public bool esEquipable { get { return (currentTab == 2); } }

        public bool abierto { get; private set; }

        public VistaInventario()
        {

            string basePath = GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\inventario\\";
            /* Inicializacion de los datos */
            inv = new ModeloInventario();
            abierto = false;
            currentRow = -1;
            currentTab = 0;
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
            /* Inicializacion de los tabs */
            tabWidth = (int)Math.Floor(back.SrcRect.Width * 1.5f / 3);
            tabBackground = new TgcSprite();
            tabBackground.Texture = back.Texture;
            tabBackground.SrcRect = new Rectangle(139, 3, 204, 37);
            tabBackground.Position = new Vector2(upperLeftCorner.X, upperLeftCorner.Y - 40);
            tabTextPosition = new Point(0, 0);
            tabText = new TgcText2d();
            tabText.Align = TgcText2d.TextAlign.LEFT;
            tabText.Color = Color.White;
            tabText.Size = new Size(tabWidth, 30);
            tabText.Position = tabTextPosition;
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
            recetasTitle.Position = new Point(((int)upperLeftCorner.X) + 20, ((int)upperLeftCorner.Y) + 20);
            recetasTitle.Align = TgcText2d.TextAlign.LEFT;
            recetasTitle.Size = new Size(back.SrcRect.Width * 3 / 2, back.SrcRect.Height * 3 / 2);
            recetasTitle.Text = "Recetas:";
            recetasTitle.Color = Color.White;
            /* Inicializacion del texto para las recetas */
            recetasText = new TgcText2d();
            recetasText.Position = new Point(((int)upperLeftCorner.X) + 40, ((int)upperLeftCorner.Y) + 47);
            recetasText.Align = TgcText2d.TextAlign.LEFT;
            recetasText.Size = itemsTitle.Size;
            recetasText.Text = "";
            recetasText.Color = Color.White;
            /* Inicializacion del titulo de las recetas */
            ingredientesTitle = new TgcText2d();
            ingredientesTitle.Position = new Point((int)(upperLeftCorner.X + selectorMiddle.Width * 2.0f) + 20, (int)(upperLeftCorner.Y) + 20);
            ingredientesTitle.Align = TgcText2d.TextAlign.LEFT;
            ingredientesTitle.Size = new Size(back.SrcRect.Width * 3 / 2, back.SrcRect.Height * 3 / 2);
            ingredientesTitle.Text = "Ingredientes:";
            ingredientesTitle.Color = Color.White;
            /* Inicializacion del texto para las recetas */
            ingredientesText = new TgcText2d();
            ingredientesText.Position = new Point((int)(upperLeftCorner.X + selectorMiddle.Width * 2.0f) + 40, (int)(upperLeftCorner.Y) + 47);
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
                back.render();
                if(currentRow != -1){
                    renderLine(selector, currentRow);
                }
                GuiController.Instance.Drawer2D.endDrawSprite();
                renderTab("Inventario", 0, tabWidth);
                renderTab("Recetas", 1, tabWidth);
                renderTab("Equipamiento", 2, tabWidth);
                if(esInventario){
                    itemsTitle.render();
                    items.render();
                } else if(esReceta){
                    recetasTitle.render();
                    recetasText.render();
                    ingredientesText.render();
                    ingredientesTitle.render();
                }
            }
        }

        private void renderTab(string tabName, int tabIndex, int tabWidth)
        {
            float startPosition = back.Position.X + tabWidth * tabIndex;
            GuiController.Instance.Drawer2D.beginDrawSprite();
            auxPoint.X = startPosition;
            /* Start height - Tab Height */
            auxPoint.Y = back.Position.Y - 40;
            /* Render Tab Start */
            int actualWidth = tabBorderMiddle.Width;
            tabBackground.Position = auxPoint;
            tabBackground.SrcRect = tabBorderStart;
            tabBackground.render();
            /* Render Tab Middles until Full Width */
            tabBackground.SrcRect = tabBorderMiddle;
            while (actualWidth < (tabWidth - tabBorderMiddle.Width))
            {
                actualWidth += tabBorderMiddle.Width;
                auxPoint.X += tabBorderMiddle.Width;
                tabBackground.Position = auxPoint;
                tabBackground.render();
            }
            /* Render Tab End */
            tabBackground.SrcRect = tabBorderEnd;
            auxPoint.X = startPosition + tabWidth - tabBorderMiddle.Width;
            tabBackground.Position = auxPoint;
            tabBackground.render();
            GuiController.Instance.Drawer2D.endDrawSprite();
            /* Render Tab Name */
            if(currentTab == tabIndex){
                tabText.Text = "> " + tabName;
            }
            else
            {
                tabText.Text = "   " + tabName;
            }
            tabTextPosition.X = (int)startPosition + tabBorderMiddle.Width;
            tabTextPosition.Y = (int)back.Position.Y - 40 + tabBorderMiddle.Width;
            tabText.Position = tabTextPosition;
            tabText.render();
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
            if (esReceta)
            {
                ingredientesText.Text = generateIngredientesText();
            }
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
            if(esReceta){
                ingredientesText.Text = generateIngredientesText();
            }
        }

        private int seleccionCount()
        {
            if(esReceta){
                return inv.contarRecetas();
            }
            else if(esInventario)
            {
                return inv.contarObjetos();
            }
            else
            {
                return 0;
            }
        }

        public void cambiarTab()
        {
            currentTab++;
            if(currentTab > 2){
                currentTab = 0;
            }
            int currentSelectionCount = seleccionCount();
            if (currentRow > (currentSelectionCount - 1))
            {
                currentRow = (currentSelectionCount - 1);
            }
            if (currentSelectionCount > 0 && currentRow == -1)
            {
                currentRow = 0;
            }
            if(esReceta){
                ingredientesText.Text = generateIngredientesText();
            }
        }

        public string consumirActual()
        {
            if (esInventario)
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