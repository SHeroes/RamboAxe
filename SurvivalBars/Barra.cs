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
        private bool esBarraDeCarga;
        private bool ISACTIVE;
        private float finTiempoBarra;
        private float tiempoActualBarra;
        private float barraAnchoCompleto;
        private float floatDeBarra;

        public void agregarPorcentajeABarra(float porcentajeAAgegar) {
            if (this.esDeCarga()) { return; }
            else { //SOLO SE AGREGAR PORCENTAJES A LA BARRA DE DESCARGA "VIDA, SED HAMBRE"
                //la asigna a disminuye el tiempo que paso en la barra en porcentaje del total del tiempo que la barra tenia que tardar
                tiempoActualBarra = tiempoActualBarra - porcentajeAAgegar * finTiempoBarra;
                if (tiempoActualBarra > 0 )tiempoActualBarra = (float)0.00001;
            }
        }
        public bool esDeCarga() {
            return this.esBarraDeCarga; 
        }
        public bool isActive()
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
                case 3: colorPath = "violet.png"; break;
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
        
        }
        
        public void render(float elapsedTime){
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
            else { // CARGA
                tiempoActualBarra = tiempoActualBarra + elapsedTime;
                vidaActual = barraAnchoCompleto * (tiempoActualBarra / finTiempoBarra);

                if (tiempoActualBarra < finTiempoBarra)
                { //isActive ?
                    barColor.Scaling = new Vector2(vidaActual, (float)0.28);
                    this.ISACTIVE = true;
                }
                else {  //barra Cargada
                    barColor.Scaling = new Vector2((float)0, (float)0.28);
                    this.ISACTIVE = false;
                }          
            }
            barEmpty.render();
            barColor.render();

        }

        public void dispose(){
            if (!this.esBarraDeCarga)  barEmpty.dispose();
            barColor.dispose();
        }
    }


}
