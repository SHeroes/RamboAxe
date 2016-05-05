using System;
using System.Collections.Generic;
using System.Text;
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

namespace AlumnoEjemplos.SurvivalBars
{
    public class SurvivalBarsExample : TgcExample
    {

        TgcSprite barEmpty;
        TgcSprite barEmpty2;
        TgcSprite barEmpty3;
        TgcSprite barYellow;
        TgcSprite barViolet;
        TgcSprite barRed;
        TgcText2d text3;
        TgcText2d GameOver;

        float sumatoriaElapsed = (float)0.24;

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }
        public override string getName()
        {
            return "SurvivalBars Test";
        }
        public override string getDescription()
        {
            return "Test del hud SurvivalBars";
        }


        const float SELECTION_BOX_HEIGHT = 50;

        TgcBox suelo;
        List<TgcMesh> modelos;
        TgcPickingRay pickingRay;
        TgcBox selectionBox;
        bool selecting;
        Vector3 initSelectionPoint;
        List<TgcMesh> modelosSeleccionados;




        public override void init()
        {
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            //Crear Sprite
            barEmpty = new TgcSprite();
            barEmpty2 = new TgcSprite();
            barEmpty3 = new TgcSprite();
            barYellow = new TgcSprite();
            barViolet = new TgcSprite();
            barRed = new TgcSprite();

            float barraVaciaAlturaScaling = (float)0.26;
            float barraVaciaAnchoScaling = (float)0.26;
            float barraVaciaPosX = (float)60.0;
            float barraVaciaPosY = (float)460.0;


            float bararAnchoCompleto = (float)0.23; 
            float barraAlto = (float)0.28;
            float barraPosX = barraVaciaPosX + (float)19;
            float barraPosY = barraVaciaPosY+ (float)3.5;

            float barrasWidth = 280;

            float vidaPorcentaje = (float)1;
            float vidaInicial = bararAnchoCompleto * vidaPorcentaje; 



            barEmpty.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\BarEmpty.png");
            barEmpty2.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\BarEmpty.png");
            barEmpty3.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\BarEmpty.png");
            barYellow.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\yellowBar.png");
            barViolet.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\violetBar.png");
            barRed.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\redBar.png");



            //Ubicarlo centrado en la pantalla
            Size screenSize = GuiController.Instance.Panel3d.Size;
            //Size textureSize = barEmpty.Texture.Size;
            //sprite.SrcRect = new Rectangle(0, sprite.Texture.Height / 2, sprite.Texture.Width * 2 / 3, sprite.Texture.Height / 2);

           // barEmpty.SrcRect = new Rectangle(0, 0, 1088, 544);
           //sprite.Position = new Vector2((float)0.0, FastMath.Max(screenSize.Height / 2 - textureSize.Height / 2, 0));


            barEmpty.Position = new Vector2(barraVaciaPosX, barraVaciaPosY);
            barEmpty.Scaling = new Vector2(barraVaciaAnchoScaling, barraVaciaAlturaScaling);
            barRed.Position = new Vector2(barraPosX, barraPosY);
            barRed.Scaling = new Vector2(bararAnchoCompleto, barraAlto);

            barEmpty2.Position = new Vector2((barraVaciaPosX + barrasWidth*1), barraVaciaPosY);
            barEmpty2.Scaling = new Vector2(barraVaciaAnchoScaling, barraVaciaAlturaScaling);
            barYellow.Position = new Vector2((barraPosX + barrasWidth*1), barraPosY);
            barYellow.Scaling = new Vector2(bararAnchoCompleto, barraAlto);

            barEmpty3.Position = new Vector2((barraVaciaPosX + barrasWidth * 2), barraVaciaPosY);
            barEmpty3.Scaling = new Vector2(barraVaciaAnchoScaling, barraVaciaAlturaScaling);
            barViolet.Position = new Vector2((barraPosX + barrasWidth * 2), barraPosY);
            barViolet.Scaling = new Vector2(bararAnchoCompleto, barraAlto);


            text3 = new TgcText2d();
            text3.Text = "";
            text3.Align = TgcText2d.TextAlign.LEFT;
            text3.Position = new Point(5, 20);
            text3.Size = new Size(310, 100);
            text3.Color = Color.Gold;

            GameOver = new TgcText2d();
            GameOver.Text = "GAME OVER";
            GameOver.Align = TgcText2d.TextAlign.CENTER;
            GameOver.Size = new Size(600, 300);
            GameOver.Color = Color.DarkRed;
            /*
            //Modifiers para variar parametros del sprite
            GuiController.Instance.Modifiers.addVertex2f("position barRed", new Vector2(0, 0), new Vector2(screenSize.Width, screenSize.Height), barRed.Position);
            GuiController.Instance.Modifiers.addVertex2f("scaling barRed", new Vector2(0, 0), new Vector2(1, 1), barRed.Scaling);
            //GuiController.Instance.Modifiers.addFloat("rotation", 0, 360, 0);

            GuiController.Instance.Modifiers.addVertex2f("position barEmpty", new Vector2(0, 0), new Vector2(screenSize.Width, screenSize.Height), barEmpty.Position);
            GuiController.Instance.Modifiers.addVertex2f("scaling barEmpty", new Vector2(0, 0), new Vector2(4, 4), barEmpty.Scaling);
            */
            //Crear suelo
            TgcTexture texture = TgcTexture.createTexture(d3dDevice, GuiController.Instance.ExamplesMediaDir + "Texturas\\Quake\\quakeWall3.jpg");
            suelo = TgcBox.fromSize(new Vector3(0, 0, 0), new Vector3(500, 0.1f, 500), texture);


            //Iniciarlizar PickingRay
            pickingRay = new TgcPickingRay();


            //Cargar modelos que se pueden seleccionar
            modelos = new List<TgcMesh>();
            modelosSeleccionados = new List<TgcMesh>();

            //Modelo 1, original
            TgcSceneLoader loader = new TgcSceneLoader();
            TgcScene scene = loader.loadSceneFromFile(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Meshes\\Vehiculos\\Carretilla\\Carretilla-TgcScene.xml");
            TgcMesh modeloOrignal = scene.Meshes[0];
            modelos.Add(modeloOrignal);

            //Modelos instancias del original
            modelos.Add(modeloOrignal.createMeshInstance("Carretilla2", new Vector3(100, 0, 0), Vector3.Empty, new Vector3(1, 1, 1)));
            modelos.Add(modeloOrignal.createMeshInstance("Carretilla3", new Vector3(50, 0, -70), Vector3.Empty, new Vector3(1, 1, 1)));
            modelos.Add(modeloOrignal.createMeshInstance("Carretilla4", new Vector3(-100, 0, -30), Vector3.Empty, new Vector3(1, 1, 1)));
            modelos.Add(modeloOrignal.createMeshInstance("Carretilla5", new Vector3(-70, 0, -80), Vector3.Empty, new Vector3(1, 1, 1)));

           

            //Crear caja para marcar en que lugar hubo colision
            selectionBox = TgcBox.fromSize(new Vector3(3, SELECTION_BOX_HEIGHT, 3), Color.Red);
            selectionBox.BoundingBox.setRenderColor(Color.Red);
            selecting = false;

            //Camara fija
            GuiController.Instance.RotCamera.Enable = false;
            GuiController.Instance.setCamera(new Vector3(-4.4715f, 239.1167f, 179.248f), new Vector3(-4.4742f, 238.3456f, 178.6113f));


        }


        public override void render(float elapsedTime)
        {
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;


          

            sumatoriaElapsed = sumatoriaElapsed - (elapsedTime * (float)0.01);

           // text3.Text = "El tiempo es:" + sumatoriaElapsed.ToString();

            if (sumatoriaElapsed > 0.001)
            {
                barRed.Scaling = new Vector2(sumatoriaElapsed, (float)0.28);
                barViolet.Scaling = new Vector2(sumatoriaElapsed, (float)0.28);
                barYellow.Scaling = new Vector2(sumatoriaElapsed, (float)0.28);

            }
            else {
                GameOver.render();
            }
            /*
            //Actualizar valores cargados en modifiers
            barRed.Position = (Vector2)GuiController.Instance.Modifiers["position barRed"];
            barRed.Scaling = (Vector2)GuiController.Instance.Modifiers["scaling barRed"];
            //barEmpty.Rotation = FastMath.ToRad((float)GuiController.Instance.Modifiers["rotation"]);

            //Actualizar valores cargados en modifiers
            barEmpty.Position = (Vector2)GuiController.Instance.Modifiers["position barEmpty"];
            barEmpty.Scaling = (Vector2)GuiController.Instance.Modifiers["scaling barEmpty"];
            //barEmpty.Rotation = FastMath.ToRad((float)GuiController.Instance.Modifiers["rotation"]);
            */


            //Iniciar dibujado de todos los Sprites de la escena (en este caso es solo uno)
            GuiController.Instance.Drawer2D.beginDrawSprite();

            //Dibujar sprite (si hubiese mas, deberian ir todos aquí)

            barRed.render();
            barEmpty.render();


            barYellow.render();
            barEmpty2.render();


            barViolet.render();
            barEmpty3.render();

            text3.render();


            //Finalizar el dibujado de Sprites
            GuiController.Instance.Drawer2D.endDrawSprite();



            //Si hacen clic con el mouse, ver si hay colision con el suelo
            if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //primera vez
                if (!selecting)
                {
                    //Actualizar Ray de colisión en base a posición del mouse
                    pickingRay.updateRay();

                    //Detectar colisión Ray-AABB
                    if (TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, suelo.BoundingBox, out initSelectionPoint))
                    {
                        selecting = true;
                        modelosSeleccionados.Clear();
                    }
                }

                //Si se está seleccionado, generar box de seleccion
                else
                {
                    //Detectar nuevo punto de colision con el piso
                    pickingRay.updateRay();
                    Vector3 collisionPoint;
                    if (TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, suelo.BoundingBox, out collisionPoint))
                    {
                        //Obtener extremos del rectángulo de selección
                        Vector3 min = Vector3.Minimize(initSelectionPoint, collisionPoint);
                        Vector3 max = Vector3.Maximize(initSelectionPoint, collisionPoint);
                        min.Y = 0;
                        max.Y = SELECTION_BOX_HEIGHT;

                        //Configurar BOX
                        selectionBox.setExtremes(min, max);
                        selectionBox.updateValues();

                        selectionBox.BoundingBox.render();
                    }
                }
            }




            //Solto el clic del mouse, terminar la selección
            if (GuiController.Instance.D3dInput.buttonUp(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                selecting = false;

                //Ver que modelos quedaron dentro del area de selección seleccionados
               
                //foreach (TgcMesh mesh in modelos)

                modelos.ForEach(delegate(TgcMesh mesh)
                    {
                          
                        
                        //Colisión de AABB entre área de selección y el modelo
                        if (TgcCollisionUtils.testAABBAABB(selectionBox.BoundingBox, mesh.BoundingBox))
                        {
                            modelosSeleccionados.Add(mesh);
                           
                            modelos.Remove(mesh);
                     



                        }
                        text3.Text = mesh.ToString();
                        text3.render();

                    });
               // modelos.Remove(auxMeshADestruir);
            }

            //Render
            suelo.render();
            foreach (TgcMesh mesh in modelos)
            {
                mesh.render();
            }

            //Renderizar BB de los modelos seleccionados
            foreach (TgcMesh mesh in modelosSeleccionados)
            {
                mesh.BoundingBox.render();
            }            


            TgcD3dInput input = GuiController.Instance.D3dInput;

            if (input.keyPressed(Key.Tab))
            {



            }

        }

        public override void close()
        {
            barEmpty.dispose();
            barEmpty2.dispose();
            barEmpty3.dispose();
            barYellow.dispose();
            barViolet.dispose();
            barRed.dispose();
            text3.dispose();
            GameOver.dispose();
            suelo.dispose();
            foreach (TgcMesh mesh in modelos)
            {
                mesh.dispose();
            }
            selectionBox.dispose();

        }
    }
}
