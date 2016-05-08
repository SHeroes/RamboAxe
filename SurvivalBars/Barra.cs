using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;

namespace AlumnoEjemplos.SurvivalBars
{
    class Barra
    {
        TgcSprite barEmpty;
        TgcSprite barColor;
        const float SELECTION_BOX_HEIGHT = 50;
        public static  int RED = 1;
        public static  int YELLOW = 2;
        public static  int VIOLET = 3;
        private static bool esBarraDeCarga;
        private static bool ISACTIVE;
        float finTiempoBarra;
        float tiempoActualBarra;
        float barraAnchoCompleto;
        

        public bool esDeCarga() {
            return Barra.esBarraDeCarga; 
        }
        public bool isActive()
        {
            return Barra.ISACTIVE;
        }


        public void init(int color, bool carga, float barraVaciaPosX = (float)60.0, float barraVaciaPosY = (float)460.0, float duracion = (float)3.00)
        {
            Barra.esBarraDeCarga = carga;
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
                case 3: colorPath = "violet.png"; break;
                default: colorPath = "redBar.png"; break;
            }            
            if (carga) {
            
                finTiempoBarra = duracion;
                tiempoActualBarra = (float)0;
            
            }else {

                finTiempoBarra = (float)0.000001;
                tiempoActualBarra =  duracion;
                barEmpty.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\BarEmpty.png");
                barEmpty.Position = new Vector2(barraVaciaPosX, barraVaciaPosY);
                barEmpty.Scaling = new Vector2(barraVaciaAnchoScaling, barraVaciaAlturaScaling);
                        
            }
            /*
            float vidaPorcentaje = (float)1;
            float vidaInicial = bararAnchoCompleto * vidaPorcentaje;
            */
            barColor.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\" + colorPath);
            barColor.Position = new Vector2(barraPosX, barraPosY);
            barColor.Scaling = new Vector2(barraAnchoCompleto, barraAlto);
        
        }
        
        public void render(float elapsedTime){
            float vidaActual;

            if (!Barra.esBarraDeCarga)  //Barra DESCARGA
            {

                vidaActual = barraAnchoCompleto * ((finTiempoBarra - tiempoActualBarra) / finTiempoBarra);
                tiempoActualBarra = tiempoActualBarra + elapsedTime;

                if (tiempoActualBarra < finTiempoBarra)
                {
                    //isActive ?
                    barColor.Scaling = new Vector2(vidaActual, (float)0.28);
                }
                barEmpty.render();
                barColor.render();

            }
            else {
                vidaActual = barraAnchoCompleto * (tiempoActualBarra / finTiempoBarra);
                tiempoActualBarra = tiempoActualBarra + elapsedTime;

                if (tiempoActualBarra < finTiempoBarra)
                {
                    //isActive ?
                    barColor.Scaling = new Vector2(vidaActual, (float)0.28);
                    Barra.ISACTIVE = true;
                }
                else { 
                //barra Cargada
                    barColor.Scaling = new Vector2((float)0, (float)0.28);
                    Barra.ISACTIVE = false;
                }
                barColor.render();            
            }
        }

        public void dispose(){
            if (!Barra.esBarraDeCarga)  barEmpty.dispose();
            barColor.dispose();
        }
    }


}
