using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using System.Drawing;

namespace AlumnoEjemplos.RamboAxe
{
    public class BarraEstatica
    {
        TgcText2d barTitle;
        TgcSprite barIcon;
        TgcSprite barEmpty;
        TgcSprite barColor;
        public string barTitleText = "";
        public static int RED = 1;
        public static int YELLOW = 2;
        public static int VIOLET = 3;
        public int valorActual; // { get; public set; }
        private int amplitudTerminaBarra;
        private float barraAnchoCompleto;
        private float max;

        //Color, Posicion X, Posicion Y, ValorMinimo ºC, ValorMaximo ºC
        public void init(int color, string texturePath,float barraVaciaPosX = (float)60.0, float barraVaciaPosY = (float)460.0, int min = 0, int _max = 100){         
            barraAnchoCompleto = (float)0.23;
            max = _max;
            valorActual = 1;
            barEmpty = new TgcSprite();
            barColor = new TgcSprite();
            string colorPath = "";
            float barraVaciaAlturaScaling = (float)0.26;
            float barraVaciaAnchoScaling = (float)0.26;

            float barraAlto = (float)0.28;
            float barraPosX = barraVaciaPosX + (float)19;
            float barraPosY = barraVaciaPosY + (float)3.5;
            

            switch (color)
            {
                case 1: colorPath = "redBar.png"; break;
                case 2: colorPath = "yellowBar.png"; break;
                case 3: colorPath = "violetBar.png"; break;
                default: colorPath = "redBar.png"; break;
            }
            barEmpty.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\BarEmpty.png");
            barEmpty.Position = new Vector2(barraVaciaPosX, barraVaciaPosY);
            barEmpty.Scaling = new Vector2(barraVaciaAnchoScaling, barraVaciaAlturaScaling);
            barColor.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\" + colorPath);
            barColor.Position = new Vector2(barraPosX, barraPosY);
            barColor.Scaling = new Vector2(barraAnchoCompleto, barraAlto);

            barIcon = new TgcSprite();
            barIcon.Texture = TgcTexture.createTexture(texturePath);
            barIcon.Position = new Vector2(barraVaciaPosX -5, barraVaciaPosY -100);
            if (texturePath.Contains("vidaicon"))
            {
                barIcon.Scaling = new Vector2(0.2f, 0.2f);
            }
            else{
                barIcon.Scaling = new Vector2(0.15f, 0.15f);
            }

            barTitle = new TgcText2d();
            barTitle.Position = new Point((int)barraVaciaPosX-5, (int)barraVaciaPosY-20);
            barTitle.Size = new Size(310, 100);
            System.Drawing.Font font1 = new System.Drawing.Font("Arial", 14);
          //barTitle.Graphics.DrawString("Arial Font", font1, Brushes.Red, new PointF(10, 10));
            barTitle.changeFont(font1);
            barTitle.Color = Color.Red;
        }

        public void render(float elapsedTime)
        {
            GuiController.Instance.Drawer2D.beginDrawSprite();

            float porcentajeVisible = barraAnchoCompleto * valorActual / max;

           barColor.Scaling = new Vector2(porcentajeVisible, (float)0.28);

            barColor.render();
            barEmpty.render();
            barTitle.Text = barTitleText;
            barTitle.render();
            barIcon.render();
            GuiController.Instance.Drawer2D.endDrawSprite();

        }

        public void dispose()
        {
            barEmpty.dispose();
            barColor.dispose();
        }
    }
}
