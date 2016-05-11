using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.DirectInput;
using TgcViewer;
using TgcViewer.Example;
using TgcViewer.Utils.Input;

using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;

using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.TgcGeometry;
using Microsoft.DirectX.DirectInput;
using System.Globalization;

namespace AlumnoEjemplos.SurvivalBars
{
    public class BarraExampleTest : TgcExample
    {
        Barra barraEjemplo;
        Barra barraEjemplo2;
        TgcText2d text3;

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }
        public override string getName()
        {
            return "TESTING DE LA BARRA COMO CLASE";
        }
        public override string getDescription()
        {
            return "TESTING DE LA BARRA COMO CLASE";
        }

        public override void init()
        {
            //GuiController.Instance: acceso principal a todas las herramientas del Framework

            //Device de DirectX para crear primitivas
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            barraEjemplo = new Barra();
            barraEjemplo.init(Barra.RED, true ,160,260,4);

            barraEjemplo2 = new Barra();
            barraEjemplo2.init(Barra.RED, false, 100, 360, 50);



            text3 = new TgcText2d();
            text3.Text = "asdfasdf";
            text3.Align = TgcText2d.TextAlign.LEFT;
            text3.Position = new Point(5, 20);
            text3.Size = new Size(310, 100);
            text3.Color = Color.Gold;
        }


        public override void render(float elapsedTime)
        {
            if (barraEjemplo2.getVidaActual() < 0.5) barraEjemplo2.agregarPorcentajeABarra((float)0.35);
            text3.Text = barraEjemplo2.getVidaActual().ToString();
            //Device de DirectX para renderizar



            //Dibujar sprite (si hubiese mas, deberian ir todos aquí)
            
           
            text3.render();
             



             barraEjemplo.render(elapsedTime);

            barraEjemplo2.render(elapsedTime);
           //if (barraEjemplo.isActive()) barraEjemplo2.render(elapsedTime);

            //Finalizar el dibujado de Sprites
          


        }


        public override void close()
        {
            barraEjemplo.dispose();
        }        
    }
}

