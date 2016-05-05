using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;

namespace AlumnoEjemplos.SurvivalBars
{
    public class BarraExampleTest : TgcExample
    {
        Barra barraEjemplo;
 
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
            Device d3dDevice = GuiController.Instance.D3dDevice;
            barraEjemplo = new Barra();
            barraEjemplo.init(Barra.RED,false,160,360,10);


        }


        public override void render(float elapsedTime)
        {
            //Device de DirectX para renderizar
            Device d3dDevice = GuiController.Instance.D3dDevice;
            GuiController.Instance.Drawer2D.beginDrawSprite();

            //Dibujar sprite (si hubiese mas, deberian ir todos aquí)

            barraEjemplo.render(elapsedTime);


            //Finalizar el dibujado de Sprites
            GuiController.Instance.Drawer2D.endDrawSprite();


        }


        public override void close()
        {
            barraEjemplo.dispose();
        }        
    }
}

