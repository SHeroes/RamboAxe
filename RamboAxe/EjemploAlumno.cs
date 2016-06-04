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
using TgcViewer.Utils.TgcGeometry;
using TgcViewer.Utils.Input;
using TgcViewer.Utils._2D;
using TgcViewer.Utils.Collision.ElipsoidCollision;
using TgcViewer.Utils.Terrain;
using TgcViewer.Utils;
using Microsoft.DirectX.DirectInput;
using AlumnoEjemplos.RamboAxe;
using AlumnoEjemplos.RamboAxe.GameObjects;
using AlumnoEjemplos.RamboAxe.Player;
using AlumnoEjemplos.RamboAxe.Inventario;
using AlumnoEjemplos.Ramboaxe;
using System.Windows.Forms;
namespace AlumnoEjemplos.RamboAxe
{
    public class EjemploAlumno : TgcExample
    {

        
        //Banderas de Input
        const float DEFAULT_ROTATION_SPEED = 2f;
        float _lastTime = 0;   
        float rotationSpeed = DEFAULT_ROTATION_SPEED;
        Barra barraInteraccion;
        BarraEstatica barraHambre; BarraEstatica barraVida;
        BarraEstatica barraSed;
        string[] vectorTemperaturas = new string[7];
        int temperaturaCuadranteActual;
        float distanciaObjeto = 0;
        TgcPickingRay pickingRay;
        static EjemploAlumno game;
        Vector3 collisionPoint;
        GameObjectAbstract selectedGameObject;
        int currentCuadrantX, currentCuadrantZ = 1;
        CharacterSheet pj = CharacterSheet.getInstance();
        VistaInventario vistaInventario;
        VistaConstruyendo vistaConstruyendo;
        float tiempoAcumuladoParaContinue = 0;
          
        TgcPlaneWall piso;
        bool gameOver = false;
        public bool forceUpdate = true;
        int tiempoDelContinue = 9;
        public MapaDelJuego mapa;
      
        public static EjemploAlumno getInstance()
        {
            return game;
        }
        public BarraEstatica getBarraSed()
        {
            return barraSed;
        }
        public BarraEstatica getBarraComida()
        {
            return barraHambre;
        }

        public override string getCategory()
        {
            return "AlumnoEjemplos";
        }

        public override string getName()
        {
            return "RamboAxe";
        }

        public override string getDescription()
        {
            return "Survival craft 3d.";
        }
        TgcD3dInput d3dInput;
        TgcText2d textHud;
        TgcText2d textHudExplicacionJuego;
        TgcText2d text3;
        TgcText2d text4;
        TgcText2d textGameOver;
        TgcText2d textGameContinue;
        SkyBox skyBox;
        public GameCamera camera;
        
        public bool falling = false;
        List<Collider> objetosColisionables = new List<Collider>();


        int widthCuadrante = 1000;
        int heightCuadrante = 1000;
        private TgcBox cuerpoPj;

        
        public override void init()
        {
    
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            GuiController.Instance.CustomRenderEnabled = true;
           
            EjemploAlumno.game = this;
            
            //Iniciarlizar PickingRay
            pickingRay = new TgcPickingRay();
            TgcTexture texture = TgcTexture.createTexture(d3dDevice, GuiController.Instance.AlumnoEjemplosDir + "Ramboaxe\\Media\\" + "tile_black.png");
            piso = new TgcPlaneWall(new Vector3(0, 0, 0), new Vector3(heightCuadrante, 0, widthCuadrante), TgcPlaneWall.Orientations.XZplane, texture);

            pj.position = new Vector3(500, 125,500);

            cuerpoPj = TgcBox.fromSize(pj.position, new Vector3(10 , pj.playerHeight, 10), Color.Aquamarine);
            d3dInput = GuiController.Instance.D3dInput;

            this.initMapa();
            this.initInventario();
            this.initCollisions();

            this.initCamera();

            this.vistaConstruyendo = new VistaConstruyendo();
            this.hud();
            this.skyboxInit();
            this.initBarrasVida();
           
        }
        public void initMapa(){
            mapa = new MapaDelJuego((int)widthCuadrante,(int)heightCuadrante);
        }
    
        public void initbarraInteraccion(float time,int color)
        {
            barraInteraccion        = new Barra();
            barraInteraccion.init(color, true, 360, 160, time);
        }
        public void initBarrasVida()
        {
            float barrasWidth = 280;
            barraHambre = new BarraEstatica();
            barraSed = new BarraEstatica();
            barraVida = new BarraEstatica();
            barraHambre.init(BarraEstatica.RED, 80, 460, 0, pj.maximaHambre);
            barraSed.init(BarraEstatica.VIOLET, (barrasWidth) + 80, 460, 0, pj.maximaSed);
            barraVida.init(BarraEstatica.YELLOW, (barrasWidth * 2) + 80, 460, 0, pj.maximaVida);

            barraHambre.valorActual = pj.hambre;
            barraVida.valorActual = pj.vida;
            barraSed.valorActual = pj.sed;

            barraVida.barTitleText       = "Vida";
            barraSed.barTitleText = "Nivel de Sed";
            barraHambre.barTitleText          = "Hambre";
        }
        private bool rotateCameraWithMouse = false;
        private float rads = 0;
        private Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
        public void handleInput() {
            TgcD3dInput input = GuiController.Instance.D3dInput;
            direction = new Vector3(0.0f, 0.0f, 0.0f);
            //direction.Z += 5.0f;
            //Forward

            if (GuiController.Instance.D3dInput.keyPressed(Microsoft.DirectX.DirectInput.Key.P))
            {
                rotateCameraWithMouse = !rotateCameraWithMouse;
                if (rotateCameraWithMouse)
                {
                   // Cursor.Hide();
                }
                else
                {
                  //  Cursor.Show();
                }
            }
            if (rotateCameraWithMouse)
            {
                int ScreenWidth = GuiController.Instance.D3dDevice.Viewport.Width;
                int ScreenHeight = GuiController.Instance.D3dDevice.Viewport.Height;
                int ScreenX = GuiController.Instance.D3dDevice.Viewport.X;
                int ScreenY = GuiController.Instance.D3dDevice.Viewport.Y;
                
                Cursor.Position = new Point(MainForm.ActiveForm.Width/2, ScreenY + (ScreenHeight / 2));
            }

            float heading = 0.0f;
            float pitch = 0.0f;


            //Solo rotar si se capturo el mouse.
            // ignorar: aprentando el boton del mouse configurado d3dInput.buttonDown(rotateMouseButton) ||
            if (rotateCameraWithMouse)
            {
                pitch = d3dInput.YposRelative * rotationSpeed;
                heading = d3dInput.XposRelative * rotationSpeed;
                camera.rotate(heading, pitch, 0.0f);
            }


            if (d3dInput.keyDown(Key.W) || d3dInput.keyDown(Key.UpArrow))
            {
                direction.Z = 1;
            }

            //Backward
            if (d3dInput.keyDown(Key.S) || d3dInput.keyDown(Key.DownArrow))
            {
                direction.Z =-1;
            }
           

            //Strafe right
            if (d3dInput.keyDown(Key.D) || d3dInput.keyDown(Key.RightArrow))
            {
                direction.X = 1;
            }
      
            //Strafe left
            if (d3dInput.keyDown(Key.A) || d3dInput.keyDown(Key.LeftArrow))
            {
                direction.X = -1;
            }
         
            //Jump
            if (d3dInput.keyDown(Key.Space))
            {
                pj.jump();
            }
            //Crouch
            if (d3dInput.keyDown(Key.LeftControl) || d3dInput.keyDown(Key.RightControl))
            {
                pj.crouch();
            }
            else
            {
                if (d3dInput.keyUp(Key.LeftControl) || d3dInput.keyUp(Key.RightControl))
                {
                    pj.stand();
                }
            }

           // Vector3 realMovement = collisionManager.moveCharacter(characterElipsoid, direction, objetosColisionables);


            Vector3 dest = new Vector3(camera.getLookAt().X,0,camera.getLookAt().Z);
            Vector3 menosOrigen = new Vector3(-pj.position.X,0,-pj.position.Z);
            Vector3 vectorCentro0 = new Vector3();
            vectorCentro0.Add(dest); 
            vectorCentro0.Add(menosOrigen);

            //Comienza el movimiento  PJ.
            //calculo los radianes de direfencia entre el lookAt de la camara y la posicion del pj(que tambien es el eye de la camara).
            rads = FastMath.ToRad(90);
            if (vectorCentro0.X > 0 && vectorCentro0.Z > 0)
            {
                float tangente = vectorCentro0.Z / vectorCentro0.X;
                rads = FastMath.Atan(tangente);
            }
            else if (vectorCentro0.X < 0 && vectorCentro0.Z > 0)
            {
                float tangente = vectorCentro0.Z / (-vectorCentro0.X);
                rads = (FastMath.PI) - FastMath.Atan(tangente);
            }
            else if (vectorCentro0.X < 0 && vectorCentro0.Z < 0)
            {
                float tangente = vectorCentro0.Z / vectorCentro0.X;
                rads = (FastMath.PI * (3 / 2)) + FastMath.Atan(tangente);
            }
            else if (vectorCentro0.X > 0 && vectorCentro0.Z < 0)
            {
                float tangente = -vectorCentro0.Z / vectorCentro0.X;
                rads = (FastMath.PI * (2)) - FastMath.Atan(tangente);
            }
           
            piso.setExtremes(new Vector3(pj.position.X - (2000), -10, pj.position.Z - 2000), new Vector3(pj.position.X + 2000, -10, pj.position.Z + 2000));
            piso.updateValues();

            bool abierto = vistaInventario.abierto;
            bool selected = false;
            if(abierto && CharacterSheet.getInstance().estaConstruyendo){
                abierto = false;
                vistaInventario.cerrar();
            }
            if (!abierto) {
                //if (GuiController.Instance.D3dInput.buttonUp(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                if (GuiController.Instance.D3dInput.buttonDown(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    if (barraInteraccion == null)
                    {
                        //Actualizar Ray de colisión en base a posición del mouse
                        pickingRay.updateRay();

                        //Testear Ray contra el AABB de todos los meshes
                        foreach (GameObjectAbstract go in mapa.getCuadrante((int)currentCuadrantX,(int)currentCuadrantZ).getObjects())
                        {
                            TgcBoundingBox aabb = go.getMesh().BoundingBox;

                            //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                            selected = TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, aabb, out collisionPoint);
                            if (selected)
                            {
                                Vector3 p1 = camera.Position;
                                Vector3 p2 = collisionPoint;
                                distanciaObjeto = Vector3.LengthSq(p2 - p1); //Es mas eficiente porque evita la raiz cuadrada (pero te da el valor al cuadrado)
                                if (distanciaObjeto < 16264)
                                {
                                    initbarraInteraccion(go.delayUso,Barra.RED);
                                    selectedGameObject = go;
                                    break;
                                }
                                else
                                {
                                    if (barraInteraccion != null)
                                    {
                                        barraInteraccion.dispose();
                                        barraInteraccion = null;
                                        selectedGameObject = null;
                                    }
                                }
                            }
                        }
                    }else{
                        if (selectedGameObject!= null)
                        {
                            TgcBoundingBox aabb = selectedGameObject.getMesh().BoundingBox;
                            //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                            selected = TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, aabb, out collisionPoint);
                            Vector3 p1 = camera.Position;
                            Vector3 p2 = collisionPoint;
                            distanciaObjeto = Vector3.LengthSq(p2 - p1); //Es mas eficiente porque evita la raiz cuadrada (pero te da el valor al cuadrado)
                            if (distanciaObjeto > 16264)
                            {
                                if (barraInteraccion != null)
                                {
                                    barraInteraccion.dispose();
                                    barraInteraccion = null;
                                    selectedGameObject = null;
                                }
                            }
                        }
                    }
                }
                if (GuiController.Instance.D3dInput.buttonUp(TgcViewer.Utils.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    selectedGameObject = null;
                    if (barraInteraccion != null)
                    {
                        barraInteraccion.dispose();
                        barraInteraccion = null;
                    }
                }

                // Handle input de vista construyendo
                if (input.keyPressed(Key.Return))
                {
                    CharacterSheet.getInstance().construir();
                } else if(input.keyPressed(Key.C)){
                    CharacterSheet.getInstance().cancelarConstruccion();
                }
            }

            if (input.keyPressed(Key.I))
            {
                if (vistaInventario.abierto)
                {
                    vistaInventario.cerrar();
                }
                else
                {
                    vistaInventario.abrir();
                }
            }
            if (abierto)
            {
                if (input.keyPressed(Key.RightArrow))
                {
                    vistaInventario.cambiarTab();
                }
                else if (input.keyPressed(Key.DownArrow))
                {
                    vistaInventario.siguienteItem();
                }
                else if (input.keyPressed(Key.UpArrow))
                {
                    vistaInventario.anteriorItem();
                }
                else if (input.keyPressed(Key.Return))
                {
                    if (!vistaInventario.esReceta)
                    {
                        if (vistaInventario.esInventario)
                        {
                            string consumido = vistaInventario.consumirActual();
                            if (consumido == "Racion")
                            {
                                pj.addLevelHambre(-20);
                            }
                            else if (consumido == "Hacha")
                            {   /*
                                if (!hachaEquipada)
                                {
                                    hachaEquipada = true;
                                    GuiController.Instance.Drawer2D.beginDrawSprite();
                                    spriteHacha.render();
                                    GuiController.Instance.Drawer2D.endDrawSprite();
                                }*/
                            }
                            else if (consumido == "Pantalon")
                            {
                                CharacterSheet.getInstance().getInventario().agregar(InventarioManager.Pantalon);
                                ObjetoInventario objAEquipar = new ObjetoInventario();
                                objAEquipar.nombre = consumido;
                                if (objAEquipar != null)
                                {
                                    text4.Text = "OBJETO EQUIPADO: " + objAEquipar.nombre;
                                }
                                else
                                {
                                    text4.Text = "ERROR EQUIPANDO OBJETO";
                                }
                                ObjetoInventario obj;
                                pj.equipoEnUso.TryGetValue(CharacterSheet.PIERNAS, out obj);
                                if (obj != null)
                                {
                                    pj.desequiparObjetoDeParteDelCuerpo(CharacterSheet.PIERNAS);
                                }
                                else
                                {
                                    pj.equiparObjetoEnParteDelCuerpo(CharacterSheet.PIERNAS, objAEquipar);
                                }

                                vistaInventario.cambioObservable();
                            }
                        }

                        else if (vistaInventario.esEquipable)
                        {
                            //TAB EQUIPO

                        }

                        // TODO: hacer algo al consumir
                        // Console.WriteLine("Item consumido: {0}", consumido);
                        // GuiController.Instance.Logger.log
                    }
                    else
                    {
                        vistaInventario.fabricarActual();
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

        public void handleResetGame(float elapsedTime) {
            TgcD3dInput input = GuiController.Instance.D3dInput;
            textGameOver.render();
            tiempoAcumuladoParaContinue = tiempoAcumuladoParaContinue + elapsedTime;
            int COUNTDOWN = tiempoDelContinue - (int)tiempoAcumuladoParaContinue;
            if (COUNTDOWN < 0) COUNTDOWN = 0;
            textGameContinue.Text = "Espere y presione la tecla \" C \" si desea otra oportunidad  " + COUNTDOWN.ToString();

            textGameContinue.render();

            if (tiempoAcumuladoParaContinue > 9 && input.keyDown(Key.C))
            {
                pj.incrementContinueCounter();
                pj.reloadContinueStats();
                gameOver = false;
                tiempoAcumuladoParaContinue = 9;
            }

        }
        

        public void initInventario() {
            InventarioManager.init();
            vistaInventario = new VistaInventario();
            pj.getInventario().agregar(InventarioManager.Palos);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Piedra);
            pj.getInventario().agregar(InventarioManager.Piedra);
            pj.getInventario().agregar(InventarioManager.RecetaCasa);
            pj.getInventario().agregar(InventarioManager.RecetaArbol);
            pj.getInventario().agregar(InventarioManager.Hacha);
            pj.getInventario().agregar(InventarioManager.Pantalon);


        }

        public void initCamera()
        {
            camera = new GameCamera(this);
           
            //Configurar FPS Camara
            camera.Enable = true;
            Vector3 lookAt = new Vector3(0,0,0);
            lookAt.Add(pj.position);
            lookAt.Add(new Vector3(0,0,1));
            camera.setCamera(pj.position,lookAt );
        }
        public void initCollisions()
        {
            objetosColisionables.Clear();
        }

        
        private void gameLoop(float elapsedTime)
        {

            Vector2 currentCuadrante  = mapa.getCuadranteCoordsFor((int)pj.position.X,(int)pj.position.Z);
            currentCuadrantX = (int)currentCuadrante.X;
            currentCuadrantZ = (int)currentCuadrante.Y;
    
            if (selectedGameObject != null)
            {
                if (barraInteraccion != null && !barraInteraccion.isActive())
                {
                    barraInteraccion.dispose();
                    barraInteraccion = null;
                    selectedGameObject.use();
                }
            }


            //  Estos son los grados a los que apunta la camara con respecto al eje x,z del mundo.

            //  FastMath.ToDeg(rads).ToString();
    
            //Buscando la altura del HeightMap del floor en la coordenada actual.
           
            mapa.getCuadrante(currentCuadrantX, currentCuadrantZ).getTerrain().interpoledHeight(pj.position.X, pj.position.Z, out pj.position.Y);
            pj.position.Y = pj.position.Y + pj.playerHeight;
            if (pj.jumpHeight > 0)
            {
                pj.fall();
                pj.position.Y += pj.jumpHeight;
            }


            cuerpoPj.Size = new Vector3(10, pj.playerHeight, 10);
            
            cuerpoPj.updateValues();
            Vector3 pjPrevPos = new Vector3(pj.position.X, pj.position.Y, pj.position.Z);
            
            //Calcula y mueve la posicion del pj de acuerdo a los radianes calculados en el paso anterior
            if (direction.Z == 1)
            {
                Matrix rotMtx = Matrix.RotationY(-rads + (FastMath.PI / 2));
                Vector4 result = Vector3.Transform(new Vector3(0, 0, pj.velocity), rotMtx);
                Vector3 zAxis = new Vector3(result.X, result.Y, result.Z);
                pj.position.Add(zAxis);
            }
            else if (direction.Z == -1)
            {
                Matrix rotMtx = Matrix.RotationY(-rads + (FastMath.PI / 2));
                Vector4 result = Vector3.Transform(new Vector3(0, 0, -pj.velocity), rotMtx);
                Vector3 zAxis = new Vector3(result.X, result.Y, result.Z);
                pj.position.Add(zAxis);
            }
            if (direction.X == 1)
            {
                Matrix rotMtx = Matrix.RotationY(-rads + (FastMath.PI / 2));
                Vector4 result = Vector3.Transform(new Vector3(pj.velocity, 0, 0), rotMtx);
                Vector3 zAxis = new Vector3(result.X, result.Y, result.Z);
                pj.position.Add(zAxis);
            }
            else if (direction.X == -1)
            {
                Matrix rotMtx = Matrix.RotationY(-rads + (FastMath.PI / 2));
                Vector4 result = Vector3.Transform(new Vector3(-pj.velocity, 0, 0), rotMtx);
                Vector3 zAxis = new Vector3(result.X, result.Y, result.Z);
                pj.position.Add(zAxis);
            }
           
           
            //Fin movimiento PJ.
            cuerpoPj.Position = pj.position;
            cuerpoPj.updateValues();
            //Colision del jugador con objetos del cuadrante.
            Cuadrante unCuadrante = mapa.getCuadrante(currentCuadrantX,currentCuadrantZ);
            foreach (GameObjectAbstract go in unCuadrante.getObjects())
            {
                TgcBoundingBox cuerpoBounds=  cuerpoPj.BoundingBox;
                float y = 0;
                unCuadrante.getTerrain().interpoledHeight(go.getX(), go.getZ(), out y);
                go.getMesh().Position = new Vector3(go.getX() + (currentCuadrantX * widthCuadrante), y, go.getZ() + (heightCuadrante * currentCuadrantZ));
                TgcBoundingBox goBounds =  go.getMesh().BoundingBox;
                TgcCollisionUtils.BoxBoxResult collisionPjObjetosCuadrantes = TgcCollisionUtils.classifyBoxBox(cuerpoBounds,goBounds);

                if ((collisionPjObjetosCuadrantes != TgcCollisionUtils.BoxBoxResult.Afuera))
                {

                    //colision norte
                    int fixDistance = 6;
                    int fixDistanceCorner = 5;
                    if (cuerpoBounds.PMin.X > goBounds.PMin.X && cuerpoBounds.PMax.X < goBounds.PMax.X && cuerpoBounds.PMin.Z < goBounds.PMax.Z && cuerpoBounds.PMax.Z > goBounds.PMax.Z)
                    {
                        pj.position.Z = goBounds.PMax.Z + fixDistance;
                    }//colision sur
                    else if (cuerpoBounds.PMax.X > goBounds.PMin.Z && cuerpoBounds.PMin.Z < goBounds.PMin.Z && cuerpoBounds.PMin.X > goBounds.PMin.X && cuerpoBounds.PMax.X < goBounds.PMax.X)
                    {
                        pj.position.Z = goBounds.PMin.Z - fixDistance;
                    }//colision oeste
                    else if (cuerpoBounds.PMin.X < goBounds.PMin.X && cuerpoBounds.PMax.X > goBounds.PMin.X && cuerpoBounds.PMax.Z < goBounds.PMax.Z && cuerpoBounds.PMin.Z > goBounds.PMin.Z)
                    {
                        pj.position.X = goBounds.PMin.X - fixDistance;
                    }//colision este
                    else if (cuerpoBounds.PMin.X < goBounds.PMax.X && cuerpoBounds.PMax.X > goBounds.PMax.X && cuerpoBounds.PMax.Z < goBounds.PMax.Z && cuerpoBounds.PMin.Z > goBounds.PMin.Z)
                    {
                        pj.position.X = goBounds.PMax.X + fixDistance;
                    }//colision noroeste
                    else if (cuerpoBounds.PMin.X > goBounds.PMin.X && cuerpoBounds.PMax.X < goBounds.PMax.X && cuerpoBounds.PMin.Z < goBounds.PMax.Z && cuerpoBounds.PMax.Z < goBounds.PMax.Z)
                    {
                        pj.position.Z = goBounds.PMax.Z + fixDistanceCorner;
                        pj.position.X = goBounds.PMin.X - fixDistanceCorner;
                    }//colision noreste
                    else if (cuerpoBounds.PMin.X < goBounds.PMax.X && cuerpoBounds.PMax.X > goBounds.PMax.X && cuerpoBounds.PMin.Z > goBounds.PMax.Z && cuerpoBounds.PMax.Z > goBounds.PMax.Z)
                    {
                        pj.position.Z = goBounds.PMax.Z + fixDistanceCorner;
                        pj.position.X = goBounds.PMax.X + fixDistanceCorner;
                    }//colision suroeste
                    else if (cuerpoBounds.PMin.X < goBounds.PMin.X && cuerpoBounds.PMax.X > goBounds.PMin.X && cuerpoBounds.PMax.Z > goBounds.PMin.Z && cuerpoBounds.PMax.Z < goBounds.PMax.Z)
                    {
                        pj.position.Z = goBounds.PMin.Z - fixDistanceCorner;
                        pj.position.X = goBounds.PMin.X - fixDistanceCorner;
                        
                    }//colision sureste
                    else if (cuerpoBounds.PMin.X < goBounds.PMax.X && cuerpoBounds.PMax.X > goBounds.PMax.X && cuerpoBounds.PMax.Z > goBounds.PMin.Z && cuerpoBounds.PMax.Z < goBounds.PMax.Z)
                    {
                        pj.position.Z = goBounds.PMin.Z - fixDistanceCorner ;
                        pj.position.X = goBounds.PMax.X + fixDistanceCorner;
                    }
                    break;
                }
            }
            cuerpoPj.Position = pj.position;
            cuerpoPj.updateValues();
            camera.setPosition(pj.position);
            //fin colision jugador con objetos
            
            if (pj.vida <= 0)
            {
                gameOver = true;
            }
            temperaturaCuadranteActual = mapa.getCuadrante((int)currentCuadrantX, (int)currentCuadrantZ).getTemperatura() + HoraDelDia.getInstance().getMomentoDelDia() - 2;

            if (temperaturaCuadranteActual > 0) {
                pj.cantDanioPorCalor = temperaturaCuadranteActual * 2;
            }else if (temperaturaCuadranteActual < 0){
                pj.cantDanioPorFrio = temperaturaCuadranteActual * -2;
            }

            string text = "";
            switch (temperaturaCuadranteActual)
            {
                case -3:
                    text = "Congelante";
                    break;
                case -2:
                    text = "Muy Frio";
                    break;
                case -1:
                    text = "Frio";
                    break;
                case 0:
                    text = "Templado";
                    break;
                case 1:
                    text = "Soleado";
                    break;
                case 2:
                    text = "Caluroso";
                    break;
                case 3:
                    text = "Ardiente";
                    break;
            }
            text3.Text = "Temperatura: " + text + " es " + HoraDelDia.getInstance().getHoraEnString();
            String hudInfo;
            hudInfo = " FPS " + HighResolutionTimer.Instance.FramesPerSecond.ToString() + " " + currentCuadrantX + " " + currentCuadrantZ;
            text3.Text += hudInfo;            
        }


        public override void render(float elapsedTime)
        {
            if (gameOver){
                if (barraInteraccion != null)
                {
                    barraInteraccion.dispose();
                    barraInteraccion = null;
                    selectedGameObject.use();
                }
                direction = new Vector3(0, 0, 0);
                this.handleResetGame(elapsedTime);
            }else{
                this.handleInput();
            }
            
            cuerpoPj.BoundingBox.render();
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            
            d3dDevice.BeginScene();

            if (_lastTime > 0.03)
            {
                gameLoop(elapsedTime);
                _lastTime = 0;
            }
            _lastTime += elapsedTime;
                

            barraSed.render(elapsedTime);
            barraVida.render(elapsedTime);
            barraHambre.render(elapsedTime);


            if (barraInteraccion != null)
            {
                barraInteraccion.render(elapsedTime);
            }

            vistaInventario.render();
           
 
           //text3.Text = "TEMPERATURA: " + vectorTemperaturas[temperaturaCuadranteActual] + "  indiceVector:" + temperaturaCuadranteActual + "tiempoDiaActual:" + tiempoDiaActual + "  x:" + currentCuadrantX + "  z:" + currentCuadrantZ;       
            //text2.render();
            
            if (!vistaInventario.abierto  && !gameOver )
            {
                barraHambre.valorActual = pj.hambre;
                barraVida.valorActual = pj.vida;
                barraSed.valorActual = pj.sed;

                barraSed.render(elapsedTime);
                barraVida.render(elapsedTime);
                barraHambre.render(elapsedTime);
            }
            
            if (barraInteraccion != null && !gameOver) {
                barraInteraccion.render(elapsedTime);
            }else{
                vistaInventario.render();
            }

            // TODO: descomentar para ver el construyendo actual
            //vistaConstruyendo.render();
           // box.render();





            changeSkyBox();
            skyBox.Center = camera.Position;
            skyBox.updateValues();
            skyBox.render();
            Vector2 result = new Vector2(0, 0);
            for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    Cuadrante unCuadrante = mapa.getCuadrante((int)(currentCuadrantX+(x-1)), ((int)currentCuadrantZ+z-1));
                    unCuadrante.getTerrain().render();
                    foreach (GameObjectAbstract go in unCuadrante.getObjects())
                    {
                        int foreachCuadranteX = currentCuadrantX + x -1;
                        int foreachCuadranteZ = currentCuadrantZ + z -1;
                        TgcMesh mesh = go.getMesh();
                        float y = 0;
                        unCuadrante.getTerrain().interpoledHeight(go.getX(), go.getZ(),out y);
                        mesh.Position = new Vector3(go.getX() + (foreachCuadranteX * widthCuadrante), y, go.getZ() + (heightCuadrante * foreachCuadranteZ));
                        mesh.updateBoundingBox();
                        
                        mesh.BoundingBox.render();
                        mesh.render();
                    }
                }
            }
             
            piso.render();
            
            
           if (selectedGameObject != null)
           {
               if (barraInteraccion != null && !barraInteraccion.isActive())
               {
                   barraInteraccion.dispose();
                   barraInteraccion = null;
                   selectedGameObject.use();
               }
           }


           text3.render();
       //    text4.render();
           GuiController.Instance.D3dDevice.EndScene();
           
        }

        public void userVars()
        {
        
       /*     GuiController.Instance.UserVars.addVar("Cam Pos X", camera.Position.X);
            GuiController.Instance.UserVars.addVar("Cam Pos Y", camera.Position.Y);
            GuiController.Instance.UserVars.addVar("Cam Pos Z", camera.Position.Z);
            GuiController.Instance.UserVars.addVar("Cam Look X", camera.getLookAt().X);
            GuiController.Instance.UserVars.addVar("Cam Look Y", camera.getLookAt().Y);
            GuiController.Instance.UserVars.addVar("Cam Look Z", camera.getLookAt().Z);
        }
        public void userVarUpdates()
        {
            GuiController.Instance.UserVars.setValue("Cam Pos X", camera.Position.X);
            GuiController.Instance.UserVars.setValue("Cam Pos Y", camera.Position.Y);
            GuiController.Instance.UserVars.setValue("Cam Pos Z", camera.Position.Z);
            GuiController.Instance.UserVars.setValue("Cam Look X", camera.getLookAt().X);
            GuiController.Instance.UserVars.setValue("Cam Look Y", camera.getLookAt().Y);
            GuiController.Instance.UserVars.setValue("Cam Look Z", camera.getLookAt().Z);*/
        }
        public void skyboxInit(){
            skyBox = new SkyBox();
           // skyBox.Center = new Vector3(0, 500, 0);
            skyBox.Size = new Vector3(10000, 10000, 10000);
            
        }


        string momentoDiaAnterior = "";
        private void changeSkyBox()
        {

            float horaDelDia = HoraDelDia.getInstance().getHoraDelDia() * 24;
            string momentoDiaString = "";
            
            if (horaDelDia < 4)  momentoDiaString = "Night";  // "Night es el nombre de la Carpeta dentro del skybox"
            else if (horaDelDia < 7) momentoDiaString = "Amanecer"; 
            else if  (horaDelDia < 12)  momentoDiaString = "Day";
            else if  (horaDelDia < 18)  momentoDiaString = "MidDay";     
            else  momentoDiaString = "Night";

            if (momentoDiaAnterior != momentoDiaString)
            {
                skyBox.updateSkyBoxTextures(momentoDiaString);
                momentoDiaAnterior = momentoDiaString;
            }           
        }


           public void hud()
        {
            int ScreenWidth = GuiController.Instance.D3dDevice.Viewport.Width;
            int ScreenHeight = GuiController.Instance.D3dDevice.Viewport.Width;
            textHud = new TgcText2d();
            //textHud.Text = "Texto del hud.";
            textHud.Align = TgcText2d.TextAlign.LEFT;
            textHud.Position = new Point(5, 20);
            textHud.Size = new Size(310, 100);
            textHud.Color = Color.Gold;
            textHud.Position = new Point(5, 20);


            textHudExplicacionJuego = new TgcText2d();
            // textHudExplicacionJuego.Text = "-\"wasd\" para moverse    -\"P\" para capturar el mouse    -\"I\" para el inventario    -\"Click Izq\" para interactuar" ;
            textHudExplicacionJuego.Align = TgcText2d.TextAlign.LEFT;
            textHudExplicacionJuego.Size = new Size(800, 100);
            textHudExplicacionJuego.Color = Color.Gold;
            textHudExplicacionJuego.Position = new Point(75, 10);

            text3 = new TgcText2d();
            
            text3.Align = TgcText2d.TextAlign.CENTER;
            text3.Size = new Size(800, 100);
            text3.Color = Color.Black;
            text3.Position = new Point(115, 30);

            text4 = new TgcText2d();

            text4.Align = TgcText2d.TextAlign.CENTER;
            text4.Size = new Size(800, 100);
            text4.Color = Color.Gold;
            text4.Position = new Point(155, 30);

            textGameOver = new TgcText2d();
            textGameOver.Text = "GAME OVER";
            textGameOver.Align = TgcText2d.TextAlign.CENTER;
            textGameOver.Size = new Size(400, 100);
            textGameOver.Color = Color.Red;
            //textGameOver.Position = new Point(115, 30);
            System.Drawing.Font font1 = new System.Drawing.Font("Arial", 44);
            textGameOver.changeFont(font1);
            textGameOver.Position = new Point( ScreenWidth /2 -200 , ScreenHeight / 2 - 100);

            textGameContinue = new TgcText2d();
            textGameContinue.Text = "Espere y presione la tecla \" C \" si desea otra oportunidad  ";
            textGameContinue.Align = TgcText2d.TextAlign.CENTER;
            textGameContinue.Size = new Size(400, 100);
            textGameContinue.Color = Color.Red;
            System.Drawing.Font font2 = new System.Drawing.Font("Arial", 44);
            textGameContinue.Position = new Point(ScreenWidth / 2 - 200, ScreenHeight / 2 );               


        }

        public override void close()
        {
            textHud.dispose();
            textHudExplicacionJuego.dispose();
            text3.dispose();
            textGameContinue.dispose();
            textGameOver.dispose();
            vistaConstruyendo.dispose();
            vistaInventario.dispose();
            InventarioManager.dispose();
            MeshManager.dispose();
        }
    }
}
