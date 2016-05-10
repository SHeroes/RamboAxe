using System;
using System.Collections.Generic;
using System.Text;
using TgcViewer.Example;
using TgcViewer;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using TgcViewer.Utils.Modifiers;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils.TgcGeometry;
using System.Windows.Forms;
using AlumnoEjemplos.SurvivalBars;
using AlumnoEjemplos.Inventario;
using TgcViewer.Utils.Input;
using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils;

namespace AlumnoEjemplos.LucasTest
{
    /// <summary>
    /// Ejemplo en Blanco. Ideal para copiar y pegar cuando queres empezar a hacer tu propio ejemplo.
    /// </summary>
    public class EjemploIsla : TgcExample
    {
        TgcBox box;
        string currentHeightmap;
        float currentScaleXZ;
        float currentScaleY;
        MapaIsla isla;
        TgcBoundingBox mainMeshBoundingBox;
        Vector3 TAMANIO_BOX_PRINCIPAL = new Vector3(12, 22, 12);
        Vector3 posAnterior = new Vector3(0, 0, 0);
        bool pause = false;
        Barra barraEjemplo;
        Inventario.Inventario inv;
        List<Objeto> objetos;

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        public override string getName()
        {
            return "Lucas";
        }

        public override string getDescription()
        {
            return "Escenario outdoor con arboles.";
        }

        protected Point mouseCenter;
        SurvivalCamara camara = new SurvivalCamara();
        bool primeraIteracion = true;

        public override void init()
        {
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            isla = new MapaIsla();

            isla.cargar();

            barraEjemplo = new Barra();
            barraEjemplo.init(Barra.RED, false, 120, 150, 10);

            //Path de Heightmap default del terreno y Modifier para cambiarla
            currentHeightmap = GuiController.Instance.AlumnoEjemplosMediaDir + "Heighmaps\\" + "Heighmap3.jpg";
            //GuiController.Instance.Modifiers.addTexture("heightmap", currentHeightmap);
            
            //Modifiers para variar escala del mapa
            currentScaleXZ = 20f;
            GuiController.Instance.Modifiers.addFloat("scaleXZ", 0.1f, 100f, currentScaleXZ);
            currentScaleY = 1.3f;
            GuiController.Instance.Modifiers.addFloat("scaleY", 0.1f, 10f, currentScaleY);

            Control focusWindows = GuiController.Instance.D3dDevice.CreationParameters.FocusWindow;
            mouseCenter = focusWindows.PointToScreen(
                new Point(
                    focusWindows.Width / 2,
                    focusWindows.Height / 2)
                    );

            camara.Enable = true;
            camara.setCamera(new Vector3(0, 100, 0), new Vector3(0, 100, 4000));
            camara.MovementSpeed = 1200f;
            camara.JumpSpeed = 500f;

            Vector3 center = new Vector3(0, 0, 0);
            TgcTexture texture = TgcTexture.createTexture(GuiController.Instance.ExamplesMediaDir + "MeshCreator\\Textures\\Ladrillo\\streetbricks.jpg");
            box = TgcBox.fromSize(center, TAMANIO_BOX_PRINCIPAL, texture);
            box.move(camara.getPosition());

            //Configurar FPS Camara
            //GuiController.Instance.FpsCamera.Enable = true;
            //GuiController.Instance.FpsCamera.MovementSpeed = 1000f;
            //GuiController.Instance.FpsCamera.JumpSpeed = 750f;
            //GuiController.Instance.FpsCamera.setCamera(new Vector3(-1151.339f, 143.0946f, -82.3528f), new Vector3(-1150.342f, 143.0397f, -82.4039f));
            this.initInventario();

        }
        public void initCamera()
        {

        }

        public void initInventario()
        {
            inv = new Inventario.Inventario();
            objetos = new List<Objeto>();
            // TODO: Agregar objetos reales
            Objeto obj1 = new Objeto();
            obj1.nombre = "Piedra";
            objetos.Add(obj1);
            Objeto obj2 = new Objeto();
            obj2.nombre = "Leña";
            objetos.Add(obj2);
            Objeto obj3 = new Objeto();
            obj3.nombre = "Palos";
            objetos.Add(obj3);
            Receta rec1 = new Receta(obj3, 3);
            rec1.agregarIngrediente(obj2, 1);
            rec1.agregarIngrediente(obj1, 2);
            inv.agregarReceta(rec1);
            Objeto casa = new Objeto();
            casa.nombre = "Casa";
            objetos.Add(casa);
            Receta rec2 = new Receta(casa, 1);
            rec2.agregarIngrediente(obj2, 10);
            rec2.agregarIngrediente(obj3, 50);
            inv.agregarReceta(rec2);
            Objeto piedraTallada = new Objeto();
            piedraTallada.nombre = "Piedra Tallada";
            Receta rPiedra = new Receta(piedraTallada, 1);

            rPiedra.agregarIngrediente(obj1, 1);
            inv.agregarReceta(rPiedra);
        }
        public void disposeInventario()
        {
            inv.dispose();
            foreach (Objeto obj in objetos)
            {
                obj.dispose();
            }
        }

        public void handleInput()
        {
            TgcD3dInput input = GuiController.Instance.D3dInput;

            bool abierto = inv.abierto;
            bool selected = false;
                if (input.keyPressed(Key.I))
                {
                    if (inv.abierto)
                    {
                        GuiController.Instance.Logger.log("Cerrar inventario");
                        inv.cerrar();
                    }
                    else
                    {
                        GuiController.Instance.Logger.log("Abrir inventario");
                        inv.abrir();
                    }
                }

                if (abierto)
                {
                    if (input.keyPressed(Key.LeftArrow) || input.keyPressed(Key.RightArrow))
                    {
                        inv.invertirSeleccion();
                    }
                    else if (input.keyPressed(Key.NumPad1) || input.keyPressed(Key.D1))
                    {
                        // TODO: desharcodear estos 3 agregar
                        inv.agregar(objetos[0]);
                    }
                    else if (input.keyPressed(Key.NumPad2) || input.keyPressed(Key.D2))
                    {
                        inv.agregar(objetos[1]);
                    }
                    else if (input.keyPressed(Key.NumPad3) || input.keyPressed(Key.D3))
                    {
                        inv.agregar(objetos[2]);
                    }
                    else if (input.keyPressed(Key.DownArrow))
                    {
                        inv.siguienteItem();
                    }
                    else if (input.keyPressed(Key.UpArrow))
                    {
                        inv.anteriorItem();
                    }
                    else if (input.keyPressed(Key.Return))
                    {
                        if (!inv.esReceta)
                        {
                            string consumido = inv.consumirActual();
                            if (consumido == "Piedra Tallada")
                            {
                                //agregarPiedraTallada();
                            }
                            // TODO: hacer algo al consumir
                            Console.WriteLine("Item consumido: {0}", consumido);
                        }
                        else
                        {
                            inv.fabricarActual();
                        }
                    }
                }
                if (selected)
                {
                    //Render de AABB


                    //Dibujar caja que representa el punto de colision
                    // collisionPointMesh.Position = collisionPoint;
                    //  collisionPointMesh.render();
                }
            }

        public override void render(float elapsedTime)
        {
            this.handleInput();
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            bool collisionFound = false;
            inv.render();
            //Ver si cambio alguno de los valores de escala
            float selectedScaleXZ = (float)GuiController.Instance.Modifiers["scaleXZ"];
            float selectedScaleY = (float)GuiController.Instance.Modifiers["scaleY"];

            if (currentScaleXZ != selectedScaleXZ || currentScaleY != selectedScaleY)
            {
                //Volver a cargar el Heightmap
                currentScaleXZ = selectedScaleXZ;
                currentScaleY = selectedScaleY;
            }

            Cursor.Position = mouseCenter;
            Cursor.Hide();
            camara.capturarMouse(true);

            Vector3 originalPos = box.Position;

            box.setPositionSize(camara.getPosition(), TAMANIO_BOX_PRINCIPAL);

            //QUILOMBO DE LA CAMARA

            if (camara.getMouseCapturado())
            {
                Cursor.Position = mouseCenter;
            }

            mainMeshBoundingBox = box.BoundingBox;

            TgcBox contorno = isla.getContorno();
            contorno.BoundingBox.render();

            TgcCollisionUtils.BoxBoxResult collisionResult1 = TgcCollisionUtils.classifyBoxBox(mainMeshBoundingBox, contorno.BoundingBox);
            if ((collisionResult1 != TgcCollisionUtils.BoxBoxResult.Encerrando))
            {
                //collisionFound = true;
                //MessageBox.Show("Hay colision!!!");
                camara.setPosicion(posAnterior);

            }


            foreach (ZonaArboles zona in isla.zonas())
            {
                TgcBoundingBox zonaBoundingBox = zona.BoundingBox;
                TgcCollisionUtils.BoxBoxResult collisionResult = TgcCollisionUtils.classifyBoxBox(mainMeshBoundingBox, zonaBoundingBox);
                if ((collisionResult != TgcCollisionUtils.BoxBoxResult.Afuera))
                {
                    foreach (Arbol arbol in zona.getListaArboles())
                    {
                        TgcBoundingBox sceneMeshBoundingBox = arbol.BoundingBox;

                        TgcCollisionUtils.BoxBoxResult collisionResult2 = TgcCollisionUtils.classifyBoxBox(mainMeshBoundingBox, sceneMeshBoundingBox);
                        if ((collisionResult2 != TgcCollisionUtils.BoxBoxResult.Afuera))
                        {
                            collisionFound = true;
                            camara.setPosicion(posAnterior);
                            break;
                        }
                    }
                    foreach (Piedra piedra in zona.getListaPiedras())
                    {
                        TgcBoundingBox piedraMeshBoundingBox = piedra.getBoundingBox();
                        TgcCollisionUtils.BoxBoxResult collisionResult3 = TgcCollisionUtils.classifyBoxBox(mainMeshBoundingBox, piedraMeshBoundingBox);
                        if ((collisionResult3 != TgcCollisionUtils.BoxBoxResult.Afuera))
                        {
                            collisionFound = true;
                            isla.sacarPiedra(piedra);
                            Objeto piedraObjeto;
                            piedraObjeto = new Objeto();
                            piedraObjeto.nombre = "Piedra";
                            inv.agregar(piedraObjeto);
                            camara.setPosicion(posAnterior);
                            break;
                        }
                    }
                }
            }

            isla.dibujar(box, elapsedTime, pause);

            if (!collisionFound)
            {
                posAnterior = camara.getPosition();
            }

            GuiController.Instance.Drawer2D.beginDrawSprite();

            //Dibujar sprite (si hubiese mas, deberian ir todos aquí)

            barraEjemplo.render(elapsedTime);


            //Finalizar el dibujado de Sprites
            GuiController.Instance.Drawer2D.endDrawSprite();


        }

        public override void close()
        {

        }

    }
}
