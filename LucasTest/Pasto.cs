using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Shaders;

namespace AlumnoEjemplos.LucasTest
{
    /// <summary>
    /// Ejemplo EjemploInstanciasPalmeras
    /// Unidades Involucradas:
    ///     # Unidad 3 - Conceptos Básicos de 3D - Mesh
    ///     # Unidad 7 - Técnicas de Optimización - Instancias de Modelos
    /// 
    /// Muestra como crear varias instancias de un mismo TgcMesh.
    /// De esta forma se reutiliza su información gráfica (triángulos, vértices, textura, etc).
    /// 
    /// Autor: Matías Leone, Leandro Barbagallo
    /// 
    /// </summary>
    public class Pasto
    {
        TgcMesh pasto;
        //TgcMesh pastoSombra;
        float timeOut = 0;
        //Effect effect;

        float contadorGiro = 0;
        float constanteSumaGiro = 1;
        const int TOPE_GIRO = 10;
        const float TIEMPO_GIRO = 0.0001f;
        float velocidadGiro = 0.0005f * (Convert.ToSingle(Math.PI));
        float anguloDeGiro = Convert.ToSingle(1 * Math.PI / 180);


        public void inicializar()
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.AlumnoEjemplosMediaDir + "isla\\Pasto-TgcScene.xml");
            pasto = scene.Meshes[0];
        }

        public void move(long posX, long posZ)
        {
            pasto.move(posX, 0, posZ);
        }

        public void scale(Vector3 tamanio)
        {
            pasto.Scale = tamanio;
        }

        public Vector3 Position
        {
            get { return pasto.Position; }
        }


        public void render(float elapsedTime)
        {
            Device d3dDevice = GuiController.Instance.D3dDevice;

            timeOut = timeOut + elapsedTime;

            if (timeOut >= TIEMPO_GIRO)
            {

                pasto.rotateY(velocidadGiro);

                contadorGiro = contadorGiro + constanteSumaGiro;

                if (contadorGiro == TOPE_GIRO || contadorGiro == (TOPE_GIRO * (-1)))
                {
                    constanteSumaGiro = constanteSumaGiro * (-1);
                    velocidadGiro = velocidadGiro * (-1);
                }

                timeOut = 0;
            }
            //Renderizamos
            pasto.render();
            //pastoSombra.render();
        }

        public void close()
        {
            pasto.dispose();
        }

        public TgcMesh getMesh()
        {
            return pasto;
        }

    }
}
