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

namespace AlumnoEjemplos.Game
{
    class Barra
    {
        TgcText2d barTitle;
        TgcSprite barEmpty;
        TgcSprite barColor;
        public string barTitleText = "";
        public static int RED = 1;
        public static int YELLOW = 2;
        public static int VIOLET = 3;
        private bool esBarraDeCarga;
        private bool ISACTIVE;
        private float finTiempoBarra;
        private float tiempoActualBarra;
        private float barraAnchoCompleto;
        private float floatDeBarra;

        public void agregarPorcentajeABarra(float porcentajeAAgegar)
        {
            if (this.esDeCarga()) { return; }
            else
            { //SOLO SE AGREGAR PORCENTAJES A LA BARRA DE DESCARGA "VIDA, SED HAMBRE"
                //la asigna a disminuye el tiempo que paso en la barra en porcentaje del total del tiempo que la barra tenia que tardar
                tiempoActualBarra = tiempoActualBarra - porcentajeAAgegar * finTiempoBarra;
                if (tiempoActualBarra < 0) tiempoActualBarra = (float)0.00001;
            }
        }

        public float getVidaActual()
        {
            return this.floatDeBarra;
        }
        public bool esDeCarga()
        {
            return this.esBarraDeCarga;
        }
        public bool isActive() //SOLO PARA BARRAS DE CARGA
        {
            return this.ISACTIVE;
        }


        public void init(int color, bool carga, float barraVaciaPosX = (float)60.0, float barraVaciaPosY = (float)460.0, float duracion = (float)3.00)
        {
            this.esBarraDeCarga = carga;
            barEmpty = new TgcSprite();
            barColor = new TgcSprite();
            string colorPath = "";
            float barraVaciaAlturaScaling = (float)0.26;
            float barraVaciaAnchoScaling = (float)0.26;

            float barraAlto = (float)0.28;
            float barraPosX = barraVaciaPosX + (float)19;
            float barraPosY = barraVaciaPosY + (float)3.5;
            float barrasWidth = 280;
            barraAnchoCompleto = (float)0.23;
            switch (color)
            {
                case 1: colorPath = "redBar.png"; break;
                case 2: colorPath = "yellowBar.png"; break;
                case 3: colorPath = "violetBar.png"; break;
                default: colorPath = "redBar.png"; break;
            }
            finTiempoBarra = duracion;
            tiempoActualBarra = (float)0.00001;

            /*
            float vidaPorcentaje = (float)1;
            float vidaInicial = bararAnchoCompleto * vidaPorcentaje;
            */
            barEmpty.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\BarEmpty.png");
            barEmpty.Position = new Vector2(barraVaciaPosX, barraVaciaPosY);
            barEmpty.Scaling = new Vector2(barraVaciaAnchoScaling, barraVaciaAlturaScaling);
            barColor.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\" + colorPath);
            barColor.Position = new Vector2(barraPosX, barraPosY);
            barColor.Scaling = new Vector2(barraAnchoCompleto, barraAlto);

            barTitle = new TgcText2d();
            barTitle.Position = new Point((int)barraVaciaPosX - 15, (int)barraVaciaPosY - 20);
            barTitle.Size = new Size(310, 100);
            System.Drawing.Font font1 = new System.Drawing.Font("Arial", 14);
            //barTitle.Graphics.DrawString("Arial Font", font1, Brushes.Red, new PointF(10, 10));
            barTitle.changeFont(font1);
            barTitle.Color = Color.Red;
        }

        public void render(float elapsedTime)
        {
            GuiController.Instance.Drawer2D.beginDrawSprite();
            float vidaActual;
            if (!this.esBarraDeCarga)  //Barra DESCARGA
            {
                floatDeBarra = ((finTiempoBarra - tiempoActualBarra) / finTiempoBarra); // 0.9999
                vidaActual = barraAnchoCompleto * floatDeBarra;
                tiempoActualBarra = tiempoActualBarra + elapsedTime; // para saber si llego al final sino sigue pal otro lado
                if (tiempoActualBarra < finTiempoBarra)
                {
                    //isActive ?
                    barColor.Scaling = new Vector2(vidaActual, (float)0.28);
                }
            }
            else
            { // CARGA
                tiempoActualBarra = tiempoActualBarra + elapsedTime;
                vidaActual = barraAnchoCompleto * (tiempoActualBarra / finTiempoBarra);

                if (tiempoActualBarra < finTiempoBarra)
                { //isActive ?
                    barColor.Scaling = new Vector2(vidaActual, (float)0.28);
                    this.ISACTIVE = true;
                }
                else
                {  //barra Cargada
                    barColor.Scaling = new Vector2((float)0, (float)0.28);
                    this.ISACTIVE = false;
                }
            }
            barColor.render();
            barEmpty.render();
            barTitle.Text = barTitleText;
            barTitle.render();
            GuiController.Instance.Drawer2D.endDrawSprite();

        }

        public void dispose()
        {
            barEmpty.dispose();
            barColor.dispose();
        }
    }
}
