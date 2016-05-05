using System;
using System.Collections.Generic;
using System.Text;
/*
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Input;
using Microsoft.DirectX.DirectInput;
using System.Globalization;
*/
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
       float finTiempoBarra;
       float tiempoActualBarra;
       

       public void init(int color , bool carga,float barraVaciaPosX = (float)60.0, float barraVaciaPosY = (float)460.0, float duracion = (float)3.00)
        {
            tiempoActualBarra = (float)0;
            finTiempoBarra = duracion;
            barEmpty = new TgcSprite();
            barColor = new TgcSprite();
            string colorPath = "";
            switch (color){
                case 1:     colorPath = "redBar.png";   break;
                case 2:     colorPath = "yellowBar.png"; break;
                case 3:     colorPath = "violet.png";   break;
                default:    colorPath = "redBar.png";   break;
            }


            float barraVaciaAlturaScaling = (float)0.26;
            float barraVaciaAnchoScaling = (float)0.26;
           
 


            float bararAnchoCompleto = (float)0.23;
            float barraAlto = (float)0.28;
            float barraPosX = barraVaciaPosX + (float)19;
            float barraPosY = barraVaciaPosY + (float)3.5;

            float barrasWidth = 280;

            float vidaPorcentaje = (float)1;
            float vidaInicial = bararAnchoCompleto * vidaPorcentaje;

            barEmpty.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\BarEmpty.png");
            barColor.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\" + colorPath);

            barEmpty.Position = new Vector2(barraVaciaPosX, barraVaciaPosY);
            barEmpty.Scaling = new Vector2(barraVaciaAnchoScaling, barraVaciaAlturaScaling);
            barColor.Position = new Vector2(barraPosX, barraPosY);
            barColor.Scaling = new Vector2(bararAnchoCompleto, barraAlto);
        
        }
        
        public void render(float elapsedTime){
            float barraAnchoCompleto = (float)0.23;

            float vidaActual = barraAnchoCompleto * ((finTiempoBarra - tiempoActualBarra) / finTiempoBarra);

            tiempoActualBarra = tiempoActualBarra + elapsedTime;

            if (tiempoActualBarra < finTiempoBarra)
            {
                //isActive ?
                barColor.Scaling = new Vector2(vidaActual, (float)0.28);
            }
            barColor.render();
            barEmpty.render();

        }

        public void dispose(){
             barEmpty.dispose();
             barColor.dispose();
        }
    }


}
