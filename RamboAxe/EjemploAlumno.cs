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
using TgcViewer.Utils.Shaders;
using TgcViewer.Utils.Interpolation;

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
        // poner en la parte de variables globales
        Random RanWind = new Random();
        int intmomentoDiaAnterior = -10;
        float vientoX;
        float vientoZ;
        float intensidadViento;
        Vector2 vientoActual = new Vector2();
        string vientoInfo = "Viento Dirección: ";
        bool llueve = false;
        float intensidadLluvia;
        string lluviaInfo = "";
        float chanceLluvia = 0.5f;
        string pjStatusInfo = "";
        
        float distanciaObjeto = 0;
        TgcPickingRay pickingRay;
        static EjemploAlumno game;
        Vector3 collisionPoint;
        GameObjectAbstract selectedGameObject;
        int currentCuadrantX, currentCuadrantZ = 1;
        CharacterSheet pj = CharacterSheet.getInstance();
        public VistaInventario vistaInventario;
        VistaConstruyendo vistaConstruyendo;
        float tiempoAcumuladoParaContinue = 0;
        TgcText2d textoAyuda;
        //TgcPlaneWall piso;

        TgcMesh piso;
        

        TgcSprite lluviaSprite;
        Vector2 posicionLluvia;

        int ScreenWidth = GuiController.Instance.D3dDevice.Viewport.Width;
        int ScreenHeight = GuiController.Instance.D3dDevice.Viewport.Height;


        bool gameOver = false;
        public bool forceUpdate = true;
        int tiempoDelContinue = 3;
        public MapaDelJuego mapa;
        Sonido sonidoDeFondo;
        TgcSprite menuAyuda;
        private bool meshShadersEnable = true;
        private bool quadShadersEnable = false;


        VertexBuffer screenQuadVB;
        Texture renderTarget2D;
        Surface pOldRT;
        Microsoft.DirectX.Direct3D.Effect effect;
        //InterpoladorVaiven intVaivenOscurecer;
 

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
            return "Survival craft 3D";
        }
        TgcD3dInput d3dInput;
        TgcText2d textHud;
        TgcText2d textHudExplicacionJuego;
        TgcText2d infoHudBasicaText;
        TgcText2d infoBoxText;
        TgcText2d textGameOver;
        TgcText2d textGameContinue;
        SkyBox skyBox;
        public GameCamera camera;
        
        public bool falling = false;
        List<Collider> objetosColisionables = new List<Collider>();

        GameObjectAbstract aguaSucia;
        int widthCuadrante = 1000;
        int heightCuadrante = 1000;
        private TgcBox cuerpoPj;
        TgcSprite hudBack;
        
        Random r;
        public override void init()
        {

            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            //PostProcess sobre quad
            CustomVertex.PositionTextured[] screenQuadVertices = new CustomVertex.PositionTextured[]
		    {
    			new CustomVertex.PositionTextured( -1, 1, 1, 0,0), 
			    new CustomVertex.PositionTextured(1,  1, 1, 1,0),
			    new CustomVertex.PositionTextured(-1, -1, 1, 0,1),
			    new CustomVertex.PositionTextured(1,-1, 1, 1,1)
    		};
            //vertex buffer de los triangulos
            screenQuadVB = new VertexBuffer(typeof(CustomVertex.PositionTextured),
                    4, d3dDevice, Usage.Dynamic | Usage.WriteOnly,
                        CustomVertex.PositionTextured.Format, Pool.Default);
            screenQuadVB.SetData(screenQuadVertices, 0, LockFlags.None);

            //Creamos un Render Targer sobre el cual se va a dibujar la pantalla

            renderTarget2D = new Texture(d3dDevice, d3dDevice.PresentationParameters.BackBufferWidth
                    , d3dDevice.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                        Format.X8R8G8B8, Pool.Default);
            
             //codigo extra para que funcione con placas de video no integradas
           /*
            stencil = d3dDevice.CreateDepthStencilSurface(d3dDevice.PresentationParameters.BackBufferWidth,d3dDevice.PresentationParameters.BackBufferHeight, DepthFormat.D24S8, MultiSampleType.None, 0, true);
            d3dDevice.DepthStencilSurface = stencil;
            */

            //Cargar shader con efectos de Post-Procesado
            effect = TgcShaders.loadEffect(GuiController.Instance.ExamplesMediaDir + "Shaders\\PostProcess.fx");

            //Configurar Technique dentro del shader
            effect.Technique = "OscurecerTechnique";

            //Interpolador para efecto de variar la intensidad de la textura de alarma
            /*intVaivenOscurecer = new InterpoladorVaiven();
            intVaivenOscurecer.Min = 0;
            intVaivenOscurecer.Max = 1;
            intVaivenOscurecer.Speed = 0.4f;
            intVaivenOscurecer.reset();*/

            
            //Fin inicio

            aguaSucia = new AguaSuciaGo();
            r = new Random();
            hudBack = new TgcSprite();
            
            hudBack.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\fondo_hud_violeta_16a.png");

            initLluviaSprites();

            GuiController.Instance.CustomRenderEnabled = true;
           
            EjemploAlumno.game = this;
            sonidoDeFondo = new Sonido(GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\sonidofondo.mp3");
            sonidoDeFondo.setLoop(true);

            menuAyuda = new TgcSprite();
            menuAyuda.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\menuayuda.png");
            Size tamaño = menuAyuda.Texture.Size;
            menuAyuda.Scaling = new Vector2(1.2f, 0.8f);
            menuAyuda.Position = new Vector2((ScreenWidth - tamaño.Width) / 2.2f,
                   (ScreenHeight - tamaño.Height / 2) / 2.8f);

            //Iniciarlizar PickingRay
            pickingRay = new TgcPickingRay();
            TgcTexture texture = TgcTexture.createTexture(d3dDevice, GuiController.Instance.AlumnoEjemplosDir + "Ramboaxe\\Media\\" + "agua.jpg");
            TgcPlaneWall pisoPlane = new TgcPlaneWall(new Vector3(0, 0, 0), new Vector3(9000, 0, 9000), TgcPlaneWall.Orientations.XZplane, texture);
            
            pj.position = new Vector3(500, 125,500);
            pisoPlane.setExtremes(new Vector3(pj.position.X - (4500), 4, pj.position.Z - 4500), new Vector3(pj.position.X + 4500, 8, pj.position.Z + 4500));
            piso = pisoPlane.toMesh("pisoMesh");
            cuerpoPj = TgcBox.fromSize(pj.position, new Vector3(10 , pj.playerHeight, 10), Color.Aquamarine);
            d3dInput = GuiController.Instance.D3dInput;

            this.initMapa();
            this.initInventario();
            this.initCollisions();

            this.initCamera();

            this.vistaConstruyendo = new VistaConstruyendo();
            this.initHud();
            this.skyboxInit();
            this.initBarrasVida(ScreenWidth,ScreenHeight);


            hudBack.Position = new Vector2(10, 10);
            float _result = ((GuiController.Instance.D3dDevice.Viewport.Width - 20) / 32);
            hudBack.Scaling = new Vector2(_result, 1.9f);
            
        }
        public void initMapa(){
            mapa = new MapaDelJuego((int)widthCuadrante,(int)heightCuadrante);
        }
    
        public void initbarraInteraccion(float time,int color)
        {
            barraInteraccion = new Barra();
            barraInteraccion.init(color, true, 360, 160, time);

        }
        public void initBarrasVida(float screenWidth, float screenHeight)
        {
            float barrasWidth = 280;
            barraHambre = new BarraEstatica();
            barraSed = new BarraEstatica();
            barraVida = new BarraEstatica();

            barraHambre.init(BarraEstatica.RED, GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\comidaicon.png",80, screenHeight - 30, 0, pj.maximaHambre);
            barraSed.init(BarraEstatica.VIOLET, GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\sedicon.png", (barrasWidth) + 80, screenHeight - 30, 0, pj.maximaSed);
            barraVida.init(BarraEstatica.YELLOW, GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\vidaicon.png", (barrasWidth * 2) + 80, screenHeight - 30, 0, pj.maximaVida);
            
            //barraHambre.init(BarraEstatica.RED, 80, 460, 0, pj.maximaHambre);
            //barraSed.init(BarraEstatica.VIOLET, (barrasWidth) + 80, 460, 0, pj.maximaSed);
            //barraVida.init(BarraEstatica.YELLOW, (barrasWidth * 2) + 80, 460, 0, pj.maximaVida);

            barraHambre.valorActual = pj.hambre;
            barraVida.valorActual = pj.vida;
            barraSed.valorActual = pj.sed;

            barraVida.barTitleText       = "Vida";
            barraSed.barTitleText = "Nivel de Sed";
            barraHambre.barTitleText          = "Hambre";
        }

        private bool rotateCameraWithMouse = false;
        private float rads = FastMath.ToRad(90);
        private Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);
        public void handleInput() {
            TgcD3dInput input = GuiController.Instance.D3dInput;
            direction = new Vector3(0.0f, 0.0f, 0.0f);
            //direction.Z += 5.0f;
            //Forward
            float heading = 0.0f;
            float pitch = 0.0f;
            if (GuiController.Instance.D3dInput.keyPressed(Microsoft.DirectX.DirectInput.Key.L))
            {
                meshShadersEnable = !meshShadersEnable;
            }
            if (GuiController.Instance.D3dInput.keyPressed(Microsoft.DirectX.DirectInput.Key.K))
            {
                quadShadersEnable = !quadShadersEnable;
            }
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
            if (rotateCameraWithMouse )
            {
                int ScreenX = GuiController.Instance.D3dDevice.Viewport.X;
                int ScreenY = GuiController.Instance.D3dDevice.Viewport.Y;

                Cursor.Position = new Point(MainForm.ActiveForm.Width/2, ScreenY + (ScreenHeight / 2));               
                pitch = d3dInput.YposRelative * rotationSpeed;
                heading = d3dInput.XposRelative * rotationSpeed;
                camera.rotate(heading, pitch, 0.0f);
            }

           

            //Solo rotar si se capturo el mouse.
            // ignorar: aprentando el boton del mouse configurado d3dInput.buttonDown(rotateMouseButton) ||
            

            if (!vistaInventario.abierto)
            {

                if (d3dInput.keyDown(Key.W) || d3dInput.keyDown(Key.UpArrow))
                {
                    direction.Z = 1;
                }

                //Backward
                if (d3dInput.keyDown(Key.S) || d3dInput.keyDown(Key.DownArrow))
                {
                    direction.Z = -1;
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
                
                //direction = new Vector3(0, 0, 0);
            }
           // Vector3 realMovement = collisionManager.moveCharacter(characterElipsoid, direction, objetosColisionables);


            Vector3 dest = new Vector3(camera.getLookAt().X,0,camera.getLookAt().Z);
            Vector3 menosOrigen = new Vector3(-pj.position.X,0,-pj.position.Z);
            Vector3 vectorCentro0 = new Vector3();
            vectorCentro0.Add(dest); 
            vectorCentro0.Add(menosOrigen);

            //Comienza el movimiento  PJ.
            //calculo los radianes de direfencia entre el lookAt de la camara y la posicion del pj(que tambien es el eye de la camara).
            if (rotateCameraWithMouse)
            {
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
            }
            

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
                                Vector3 p1 = cuerpoPj.BoundingBox.calculateBoxCenter();
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
                        if (barraInteraccion==null&&pj.position.Y <=pj.playerHeight+3)
                        {
                            piso.BoundingBox.setExtremes(new Vector3(cuerpoPj.BoundingBox.PMin.X-10, cuerpoPj.BoundingBox.PMin.Y, cuerpoPj.BoundingBox.PMin.Z-10), new Vector3(cuerpoPj.BoundingBox.PMax.X+10, cuerpoPj.BoundingBox.PMin.Y, cuerpoPj.BoundingBox.PMax.Z+10));
                            if (TgcCollisionUtils.intersectRayAABB(pickingRay.Ray,piso.BoundingBox, out collisionPoint))
                            {
                                initbarraInteraccion(aguaSucia.delayUso, Barra.RED);
                                selectedGameObject = aguaSucia;
                            }
                        }
                    }else{
                        if (selectedGameObject!= null)
                        {
                            if (selectedGameObject.getMesh() != null)
                            {
                                TgcBoundingBox aabb = selectedGameObject.getMesh().BoundingBox;
                                //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                                selected = TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, aabb, out collisionPoint);
                                Vector3 p1 = cuerpoPj.BoundingBox.calculateBoxCenter();
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
                            else
                            {
                                pickingRay.updateRay();
                                piso.BoundingBox.setExtremes(new Vector3(cuerpoPj.BoundingBox.PMin.X - 10, cuerpoPj.BoundingBox.PMin.Y, cuerpoPj.BoundingBox.PMin.Z - 10), new Vector3(cuerpoPj.BoundingBox.PMax.X + 10, cuerpoPj.BoundingBox.PMin.Y, cuerpoPj.BoundingBox.PMax.Z + 10));
                                if (!TgcCollisionUtils.intersectRayAABB(pickingRay.Ray, piso.BoundingBox, out collisionPoint))
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
                if (input.keyPressed(Key.LeftArrow) || input.keyPressed(Key.A))
                {
                    vistaInventario.cambiarTab();
                    vistaInventario.cambiarTab();
                } else
                if (input.keyPressed(Key.RightArrow) || input.keyPressed(Key.D))
                {
                    vistaInventario.cambiarTab();
                }
                else if (input.keyPressed(Key.DownArrow) || input.keyPressed(Key.S))
                {
                    vistaInventario.siguienteItem();
                }
                else if (input.keyPressed(Key.UpArrow) || input.keyPressed(Key.W))
                {
                    vistaInventario.anteriorItem();
                }
                else if (input.keyPressed(Key.Return))
                {
                    vistaInventario.accionarItem();
                }
                else if(input.keyPressed(Key.T)){
                    vistaInventario.tirarActual();
                }
            }
            else
            {
                if(input.keyPressed(Key.Q)){
                    vistaInventario.anteriorItem();
                }
                else if (input.keyPressed(Key.E))
                {
                    vistaInventario.siguienteItem();
                }
                else if (input.keyPressed(Key.R))
                {
                    vistaInventario.usarAccesoRapido();
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
            
            tiempoAcumuladoParaContinue = tiempoAcumuladoParaContinue + elapsedTime;
            int COUNTDOWN = tiempoDelContinue - (int)tiempoAcumuladoParaContinue;
            if (COUNTDOWN < 0) COUNTDOWN = 0;
            textGameContinue.Text = "Espere y presione la tecla \" C \" si desea otra oportunidad  " + COUNTDOWN.ToString();

            

            if (tiempoAcumuladoParaContinue > 3 && input.keyDown(Key.C))
            {
                
                pj.incrementContinueCounter();
                pj.reloadContinueStats();
                gameOver = false;
                quadShadersEnable = false;
                sonidoDeFondo.setLoop(true);
                sonidoDeFondo.loadMp3(GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\sonidofondo.mp3");
                tiempoAcumuladoParaContinue = 3;
            }

        }

        public void initLluviaSprites()
        {
            lluviaSprite = new TgcSprite();
            //lluviaSprite.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\Rain_Effect.png");
            lluviaSprite.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\rain-sprite.png");
            posicionLluvia = new Vector2(0f, 0f);
            lluviaSprite.Position = posicionLluvia;
            lluviaSprite.Scaling = new Vector2(1f, 1f);
        }

        public void initInventario() {
            InventarioManager.init();
            vistaInventario = new VistaInventario();
            pj.getInventario().agregar(InventarioManager.Ramita, 10);
            pj.getInventario().agregar(InventarioManager.Piedra, 2);
            pj.getInventario().agregar(InventarioManager.Hacha);
            pj.getInventario().agregar(InventarioManager.Pantalon);
            pj.getInventario().agregar(InventarioManager.Racion, 10);
            pj.getInventario().agregar(InventarioManager.RecetaArbol);
            pj.getInventario().agregar(InventarioManager.Remera);
            pj.getInventario().agregar(InventarioManager.Zapatillas);

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
        int prevCuadrantX, prevCuadrantZ = -100;
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
            //Buscando la altura del HeightMap del floor en la coordenada actual.
           

            //Fin movimiento PJ.
            cuerpoPj.Position = pj.position;
            cuerpoPj.Size = new Vector3(6, pj.playerHeight, 6);
            cuerpoPj.updateValues();
           

            //Colision del jugador con objetos del cuadrante.
            Cuadrante unCuadrante = mapa.getCuadrante(currentCuadrantX,currentCuadrantZ);
            foreach (GameObjectAbstract go in unCuadrante.getObjects())
            {
                TgcBoundingBox cuerpoBounds=  cuerpoPj.BoundingBox;
                bool collisionFound = false;
                foreach (TgcMesh bounds in go.getBounds())
                {
                  
                    TgcBoundingBox goBounds = bounds.BoundingBox;
                    TgcCollisionUtils.BoxBoxResult collisionPjObjetosCuadrantes = TgcCollisionUtils.classifyBoxBox(cuerpoBounds, goBounds);

                    if ((collisionPjObjetosCuadrantes != TgcCollisionUtils.BoxBoxResult.Afuera))
                    {
                        collisionFound = true;
                    }
                }
                if (collisionFound)
                {
                    pj.position = new Vector3(pjPrevPos.X, pjPrevPos.Y, pjPrevPos.Z);
                    break;
                }
            }
           
            cuerpoPj.Position = pj.position;
            cuerpoPj.updateValues();
            pjPrevPos = new Vector3(pj.position.X,pj.position.Y, pj.position.Z);
            float nuevaY;
            mapa.getCuadrante(currentCuadrantX, currentCuadrantZ).getTerrain().interpoledHeight(pj.position.X, pj.position.Z, out nuevaY);
            //pj.position.Y = pj.position.Y + pj.playerHeight;
            if (pj.fallStrength != 0)
            {
                pj.fall();
                pj.position.Add(new Vector3(0, -1 * pj.fallStrength, 0));
                if (pj.position.Y <= nuevaY + pj.playerHeight)
                {
                    pj.position.Y = nuevaY + pj.playerHeight;
                    pj.golpearPiso();
                }
            }
            else
            {
                if (pj.position.Y < nuevaY + pj.playerHeight)
                {
                    pj.position.Y = nuevaY + pj.playerHeight;
                }
                if (pj.position.Y > nuevaY + pj.playerHeight)
                {
                    pj.fall();
                    pj.position.Add(new Vector3(0, -1 * pj.fallStrength, 0));
                    if (pj.position.Y <= nuevaY + pj.playerHeight)
                    {
                        pj.position.Y = nuevaY + pj.playerHeight;
                        pj.golpearPiso();
                    }
                }
            }
            cuerpoPj.Position = pj.position;
            cuerpoPj.updateValues();
            //Me fijo una seguna vez para ajustar el salto /crouch del pj
            foreach (GameObjectAbstract go in unCuadrante.getObjects())
            {
                TgcBoundingBox cuerpoBounds = cuerpoPj.BoundingBox;
                bool collisionFound = false;
                foreach (TgcMesh bounds in go.getBounds())
                {
                    TgcBoundingBox goBounds = bounds.BoundingBox;
                    TgcCollisionUtils.BoxBoxResult collisionPjObjetosCuadrantes = TgcCollisionUtils.classifyBoxBox(cuerpoBounds, goBounds);

                    if ((collisionPjObjetosCuadrantes != TgcCollisionUtils.BoxBoxResult.Afuera))
                    {
                        collisionFound = true;
                    }
                }
                if (collisionFound)
                {
                    pj.position = new Vector3(pjPrevPos.X, pjPrevPos.Y, pjPrevPos.Z);
                    pj.golpearPiso();
                    break;
                }
            }

            foreach (GameObjectAbstract go in unCuadrante.getObjects())
            {
                if (go.esBailador) {
                    
                    go.bailar(vientoActual, intensidadViento);

                   
                }
            }


            cuerpoPj.Position = pj.position;
            cuerpoPj.updateValues();
            camera.setPosition(pj.position);
            camera.updateViewMatrix(GuiController.Instance.D3dDevice);
            camera.updateCamera();

           
            
            //fin colision jugador con objetos
            
            if (pj.vida <= 0)
            {
                gameOver = true;
            }
            temperaturaCuadranteActual = mapa.getCuadrante((int)currentCuadrantX, (int)currentCuadrantZ).getTemperatura() + HoraDelDia.getInstance().getMomentoDelDia() - 2;
            
            //Cuando cambia momento del dia
            if (HoraDelDia.getInstance().getMomentoDelDia() != intmomentoDiaAnterior)
            {
                if (chanceLluvia > (float)RanWind.NextDouble()) {
                    llueve = true;
                    lluviaInfo = " Llueve " + getLluviaIntensidadString();
                    if (!pj.deBuffes.Contains("Mojado"))
                    {
                        pj.deBuffes.Add("Mojado");
                    }

                }
                else
                {
                    llueve = false;
                    lluviaInfo = "";
                }
                //Si esta calentito te secas
                if (temperaturaCuadranteActual > 0)
                {
                    if (pj.deBuffes.Contains("Mojado"))
                    {
                        pj.deBuffes.Remove("Mojado");
                    }
                }

                intmomentoDiaAnterior = HoraDelDia.getInstance().getMomentoDelDia();
                vientoX = (float)RanWind.NextDouble() - 0.5f;
                vientoZ = (float)RanWind.NextDouble() - 0.5f;
                intensidadViento = (float)RanWind.NextDouble();
                vientoActual = new Vector2(vientoX, vientoZ);
                vientoActual.Normalize();
                intensidadViento = (float)RanWind.NextDouble() * (float)RanWind.NextDouble() * (float)RanWind.NextDouble() * (float)RanWind.NextDouble();
                intensidadLluvia = (float)RanWind.NextDouble() * (float)RanWind.NextDouble();
                vientoInfo = "Viento Direccion: " + vientoActualString() + " intensidad: " + (int)(intensidadViento * 120) + "KM/h";
                //vientoInfo += "\t VectorVientoNormalizado" + vientoActual.X.ToString() + vientoActual.Y.ToString();
            };
            pjStatusInfo = "";
            if (pj.deBuffes.Count != 0)
            {
                foreach (var debuff in pj.deBuffes)
                {
                    pjStatusInfo += " " + debuff + "\n ";
                }
            }
            else { }




            if (temperaturaCuadranteActual > 0) {
               // pj.cantDanioPorCalor = temperaturaCuadranteActual;
            }else if (temperaturaCuadranteActual < 0){
                //pj.cantDanioPorFrio = temperaturaCuadranteActual *;
            }

            string text = "";
            switch (temperaturaCuadranteActual)
            {
                case -3:
                    text = "hace muchisimo frio.";
                    break;
                case -2:
                    text = "hace mucho frio.";
                    break;
                case -1:
                    text = "hace frio.";
                    break;
                case 0:
                    text = "esta templado.";
                    break;
                case 1:
                    text = "hace calor.";
                    break;
                case 2:
                    text = "hace mucho calor.";
                    break;
                case 3:
                    text = "hace muchisimo calor.";
                    break;
            }
           infoHudBasicaText.Text = " Dia "+ HoraDelDia.getInstance().dia.ToString()+" es " + HoraDelDia.getInstance().getHoraEnString()+ ", "+text;
            String hudInfo;
            hudInfo = " FPS " + HighResolutionTimer.Instance.FramesPerSecond.ToString() + " " + currentCuadrantX + " " + currentCuadrantZ;
            infoHudBasicaText.Text += hudInfo;
            infoHudBasicaText.Text += vientoInfo;
            infoHudBasicaText.Text += lluviaInfo;
            infoHudBasicaText.Text += pjStatusInfo;
            
            barraHambre.valorActual = pj.hambre;
            barraVida.valorActual = pj.vida;
            barraSed.valorActual = pj.sed;
            if (gameOver)
            {
                if (barraInteraccion != null)
                {
                    barraInteraccion.dispose();
                    barraInteraccion = null;
                    selectedGameObject.use();
                }
                quadShadersEnable = true;
                direction = new Vector3(0, 0, 0);
                this.handleResetGame(elapsedTime);
            }


           
         
            
        }


        public override void render(float elapsedTime)
        {
            piso.Position =new Vector3(pj.position.X -4500,8,pj.position.Z - 4500);
            //piso.BoundingBox.render();
            if (_lastTime > 0.03)
            {
                gameLoop(elapsedTime);
                _lastTime = 0;
            }
            
            _lastTime += elapsedTime;
            if (!gameOver)
            {
                this.handleInput();
            }
            //RENDER BEGINS

            

            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            //Cargamos el Render Targer al cual se va a dibujar la escena 3D. Antes nos guardamos el surface original
            //En vez de dibujar a la pantalla, dibujamos a un buffer auxiliar, nuestro Render Target.
            pOldRT = d3dDevice.GetRenderTarget(0);
            Surface pSurf = renderTarget2D.GetSurfaceLevel(0);
            d3dDevice.SetRenderTarget(0, pSurf);
            d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);

            //Dibujamos la escena comun, pero en vez de a la pantalla al Render Target
            drawSceneToRenderTarget(d3dDevice,elapsedTime);

            //Liberar memoria de surface de Render Target
            pSurf.Dispose();

            //Si quisieramos ver que se dibujo, podemos guardar el resultado a una textura en un archivo para debugear su resultado (ojo, es lento)
            //TextureLoader.Save(GuiController.Instance.ExamplesMediaDir + "Shaders\\render_target.bmp", ImageFileFormat.Bmp, renderTarget2D);


            //Ahora volvemos a restaurar el Render Target original (osea dibujar a la pantalla)
            d3dDevice.SetRenderTarget(0, pOldRT);


            //Luego tomamos lo dibujado antes y lo combinamos con una textura con efecto de alarma
            drawPostProcess(d3dDevice);
            d3dDevice.BeginScene();
            if (!gameOver)
            {
                if (barraInteraccion != null)
                {

                    barraInteraccion.render(elapsedTime);

                }
                if (!vistaInventario.abierto)
                {
                    GuiController.Instance.Drawer2D.beginDrawSprite();
                    hudBack.render();
                    GuiController.Instance.Drawer2D.endDrawSprite();
                    infoHudBasicaText.render();
                    textoAyuda.render();

                }
                if (llueve && intensidadLluvia > 0.5)
                {
                    lluviaFuerte();
                }
                sonidoDeFondo.playMusic();

                vistaInventario.render();
                //vistaConstruyendo.render();
            }
            else
            {
                vistaInventario.cerrar();
                textGameOver.render();
                textGameContinue.render();
                
                if (sonidoDeFondo.getActualSound() == GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\sonidofondo.mp3")
                {
                    //GuiController.Instance.Logger.log("sonidoDeFondo: " + sonidoDeFondo.getActualSound());
                    sonidoDeFondo.loadMp3(GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\siamofuori.mp3");
                    sonidoDeFondo.playMusic(false);
                }
                
            }
            //Mostrar Ayuda
            if (d3dInput.keyDown(Key.F5))
            {
                GuiController.Instance.Drawer2D.beginDrawSprite();
                menuAyuda.render();
                GuiController.Instance.Drawer2D.endDrawSprite();
            }
            else
            {
                barraSed.render(elapsedTime);
                barraVida.render(elapsedTime);
                barraHambre.render(elapsedTime);
            }



/*
            if (selectedGameObject != null)
            {
                if (barraInteraccion != null && !barraInteraccion.isActive())
                {
                    //barraInteraccion.render(elapsedTime);

                    barraInteraccion.dispose();
                    barraInteraccion = null;
                    selectedGameObject.use();

                }
            }
 * */

            d3dDevice.EndScene();
        }
        /// <summary>
        /// Dibujamos toda la escena pero en vez de a la pantalla, la dibujamos al Render Target que se cargo antes.
        /// Es como si dibujaramos a una textura auxiliar, que luego podemos utilizar.
        /// </summary>
        private void drawSceneToRenderTarget(Microsoft.DirectX.Direct3D.Device d3dDevice,float elapsedTime)
        {
            d3dDevice.BeginScene();

            changeSkyBox();

            skyBox.Center = camera.Position;
            //skyBox.updateValues();

            /*
             ...
               skyBox.Center = camera.Position;
                skyBox.updateValues(); //aquí se instancia texturas y vertices, esto no es recomendado hacerlo en render time, se aviso por la cadena de mail no utilizara este metodo. utilicen una matriz traslación.
                ...
             */
            
            


            int boxesToCheck = 9;

            if (currentCuadrantX != prevCuadrantX || currentCuadrantZ != prevCuadrantZ)
            {
                //   infoBoxText.Text = " Loading... ";
                prevCuadrantZ = currentCuadrantZ;
                prevCuadrantX = currentCuadrantX;
            }
            else
            {
                //  infoBoxText.Text = "";
            }
            infoBoxText.render();

            for (int x = 0; x < boxesToCheck; x++)
            {
                for (int z = 0; z < boxesToCheck; z++)
                {

                    Cuadrante unCuadrante = mapa.getCuadrante((int)(currentCuadrantX + (x - ((int)boxesToCheck / 2))), ((int)currentCuadrantZ + z - (int)boxesToCheck / 2));
                    TgcCollisionUtils.FrustumResult r = TgcCollisionUtils.classifyFrustumAABB(GuiController.Instance.Frustum, unCuadrante.getBoundingBox());

                    if (r == TgcCollisionUtils.FrustumResult.INTERSECT || r == TgcCollisionUtils.FrustumResult.INSIDE)
                    {

                       
                        unCuadrante.getTerrain().Effect = piso.Effect;
                        unCuadrante.getTerrain().Technique = piso.Technique;
                        
                        unCuadrante.getTerrain().render();
                        // text4.Text += ">> " +unCuadrante.getLatitud().ToString() + " " + unCuadrante.getLongitud().ToString()+"\n";
                        foreach (GameObjectAbstract go in unCuadrante.getObjects())
                        {

                            int foreachCuadranteX = currentCuadrantX + x - 1;
                            int foreachCuadranteZ = currentCuadrantZ + z - 1;
                            TgcMesh mesh = go.getMesh();
                            r = TgcCollisionUtils.classifyFrustumAABB(GuiController.Instance.Frustum, mesh.BoundingBox);

                            if (r == TgcCollisionUtils.FrustumResult.INSIDE || r == TgcCollisionUtils.FrustumResult.INTERSECT)
                            {
                                mesh.Effect = piso.Effect;
                                mesh.Technique = piso.Technique;
                                mesh.render();
                            }


                        }
                    }
                }
            }


             if (meshShadersEnable)
             {
                 piso.Effect = TgcShaders.loadEffect(GuiController.Instance.ExamplesMediaDir + "Shaders\\Ejemplo1.fx");
                 piso.Technique = "Darkening";
                 foreach (TgcMesh face in skyBox.Faces)
                 {
                     face.Effect = piso.Effect;
                     face.Technique = piso.Technique;
                 }
             }
             else
             {
                 piso.Effect = GuiController.Instance.Shaders.TgcMeshShader;
                 piso.Technique = GuiController.Instance.Shaders.getTgcMeshTechnique(piso.RenderType);
                 foreach (TgcMesh face in skyBox.Faces)
                 {
                     face.Effect = piso.Effect;
                     face.Technique = piso.Technique;
                 }
             }

            if (meshShadersEnable)
            {
                //Cargar variables shader de la luz
                factorOscuridad = 0;
                if (HoraDelDia.getInstance().isAM())
                    factorOscuridad = (float)HoraDelDia.getInstance().getHoraDelDia();
                else  factorOscuridad = 1f - (float)HoraDelDia.getInstance().getHoraDelDia();
                //GuiController.Instance.Logger.log("factorOscuridad: "+factorOscuridad.ToString() );
                piso.Effect.SetValue("darkFactor", factorOscuridad);
               

                //infoBoxText.Text = skyBox.Faces.Length.ToString();
                foreach (TgcMesh face in skyBox.Faces)
                {
                    face.Effect.SetValue("darkFactor", factorOscuridad);
                    face.render();
                }
               // piso.Effect.SetValue("random", (float)r.NextDouble());
                //piso.Effect.SetValue("textureOffset", (float)GuiController.Instance.Modifiers["textureOffset"]);

            }
            else
            {
                skyBox.render();
            }// descomentar skybox.render() si se comenta meshShadersEnable y viseversa
            //skyBox.render();

            piso.render();
            vistaConstruyendo.render();
            

            //cuerpoPj.BoundingBox.render();
            d3dDevice.EndScene();
        }


        /// <summary>
        /// Se toma todo lo dibujado antes, que se guardo en una textura, y se le aplica un shader para oscurecer la imagen
        /// </summary>
        private void drawPostProcess(Microsoft.DirectX.Direct3D.Device d3dDevice)
        {
            //Arrancamos la escena
            d3dDevice.BeginScene();

            //Cargamos para renderizar el unico modelo que tenemos, un Quad que ocupa toda la pantalla, con la textura de todo lo dibujado antes
            d3dDevice.VertexFormat = CustomVertex.PositionTextured.Format;
            d3dDevice.SetStreamSource(0, screenQuadVB, 0);

            //Ver si el efecto de oscurecer esta activado, configurar Technique del shader segun corresponda
            
            if (quadShadersEnable)
            {
                effect.Technique = "OscurecerTechnique";
            }
            else
            {
                effect.Technique = "DefaultTechnique";
            }

            //Cargamos parametros en el shader de Post-Procesado
            effect.SetValue("render_target2D", renderTarget2D);
            effect.SetValue("scaleFactor",HoraDelDia.getInstance().getLuz());

            //Limiamos la pantalla y ejecutamos el render del shader
            d3dDevice.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            effect.Begin(FX.None);
            effect.BeginPass(0);
            d3dDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();

            //Terminamos el renderizado de la escena
            d3dDevice.EndScene();
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
        private float factorOscuridad;


        private Surface stencil;
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


           public void initHud()
        {
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

            infoHudBasicaText = new TgcText2d();
            
            infoHudBasicaText.Align = TgcText2d.TextAlign.CENTER;
            infoHudBasicaText.Size = new Size(GuiController.Instance.D3dDevice.Viewport.Width - 20, 72);
            infoHudBasicaText.Color = Color.Gold;
            infoHudBasicaText.Position = new Point(14, 14);

            textoAyuda = new TgcText2d();
            textoAyuda.Text = "¿Como jugar ? Presioná F5";
            textoAyuda.Color = Color.Yellow;
            textoAyuda.Align = TgcText2d.TextAlign.LEFT;
            textoAyuda.Position = new Point(1050, 650);
            textoAyuda.Size = new Size(240, 50);
            textoAyuda.changeFont(new System.Drawing.Font("TimesNewRoman", 15, FontStyle.Bold | FontStyle.Italic));

            infoBoxText = new TgcText2d();

            infoBoxText.Align = TgcText2d.TextAlign.CENTER;
            infoBoxText.Size = new Size(GuiController.Instance.D3dDevice.Viewport.Width - 20, 100);
            infoBoxText.Color = Color.Gold;
            infoBoxText.Position = new Point(10, GuiController.Instance.D3dDevice.Viewport.Height/2-15);

            textGameOver = new TgcText2d();
            textGameOver.Text = "GAME OVER";
            textGameOver.Align = TgcText2d.TextAlign.CENTER;
            textGameOver.Size = new Size(400, 100);
            textGameOver.Color = Color.Red;
            //textGameOver.Position = new Point(115, 30);
            System.Drawing.Font font1 = new System.Drawing.Font("Arial", 44);
            textGameOver.changeFont(font1);
            textGameOver.Position = new Point(ScreenWidth / 2 - 200, ScreenHeight / 2 - 100);

            textGameContinue = new TgcText2d();
            textGameContinue.Text = "Espere y presione la tecla \" C \" si desea otra oportunidad  ";
            textGameContinue.Align = TgcText2d.TextAlign.CENTER;
            textGameContinue.Size = new Size(400, 100);
            textGameContinue.Color = Color.Red;
            System.Drawing.Font font2 = new System.Drawing.Font("Arial", 44);
            textGameContinue.Position = new Point((int)ScreenWidth / 2 - 200, (int)ScreenHeight / 2 );



            /*//Modifiers para variables de luz
            GuiController.Instance.Modifiers.addBoolean("lightEnable", "lightEnable", true);
            GuiController.Instance.Modifiers.addFloat("lightIntensity", 0, 150, 20);
            GuiController.Instance.Modifiers.addFloat("lightAttenuation", 0.1f, 2, 0.3f);
            GuiController.Instance.Modifiers.addFloat("specularEx", 0, 20, 9f);

            //Modifiers para material
            GuiController.Instance.Modifiers.addColor("mEmissive", Color.Black);
            GuiController.Instance.Modifiers.addColor("mAmbient", Color.White);
            GuiController.Instance.Modifiers.addColor("mDiffuse", Color.White);
            GuiController.Instance.Modifiers.addColor("mSpecular", Color.White);*/
         /*   GuiController.Instance.Modifiers.addInterval("Technique", new string[] { 
                "OnlyTexture", 
                "OnlyColor", 
                "Darkening",
                "Complementing",
                "MaskRedOut",
                "RedOnly",
                "RandomTexCoord",
                "RandomColorVS",
                "TextureOffset"
            }, 0);

            //Modifier para variables de shader
            GuiController.Instance.Modifiers.addFloat("darkFactor", 0f, 1f, 0.5f);
            GuiController.Instance.Modifiers.addFloat("textureOffset", 0f, 1f, 0.5f);*/
            
        }

        private string getLluviaIntensidadString() {
            string intensidadLluviaString = "";
            if (intensidadLluvia < 0.1) { 
                intensidadLluviaString = "Finito";
            } else
            if (intensidadLluvia < 0.3)
            {
                intensidadLluviaString = "Poco";
            } else
            if (intensidadLluvia < 0.5)
            {
                intensidadLluviaString = "Mucho";
             } else
            if (intensidadLluvia < 0.8)
            {
                intensidadLluviaString = "Demasiado";
            }
            return intensidadLluviaString;
        }
        private void lluviaFuerte()
        {
            Random rx = new Random();
            float rx1 = (float)rx.NextDouble();
            posicionLluvia.X = (rx1 * ScreenWidth - ScreenWidth / 2);
            lluviaSprite.Position = posicionLluvia;
            GuiController.Instance.Drawer2D.beginDrawSprite();

            lluviaSprite.render();
            posicionLluvia.X = (rx1 * ScreenWidth * 2);
            lluviaSprite.Position = posicionLluvia;
            lluviaSprite.render();
            posicionLluvia.X = (rx1 * ScreenWidth);
            lluviaSprite.Position = posicionLluvia;
            lluviaSprite.render();
            GuiController.Instance.Drawer2D.endDrawSprite();
        }
        private void lluviaSuave()
        {
            Random rx = new Random();
            float rx1 = (float)rx.NextDouble();
            float r2 = rx1 * ScreenWidth / 8;
            posicionLluvia.X = (r2);
            lluviaSprite.Position = posicionLluvia;

            GuiController.Instance.Drawer2D.beginDrawSprite();
            lluviaSprite.render();
            posicionLluvia.X = (r2 + ScreenWidth / 2);
            lluviaSprite.Position = posicionLluvia;
            lluviaSprite.render();
            GuiController.Instance.Drawer2D.endDrawSprite();
        }
        private string vientoActualString()
        {
            string viento = "";
            double angulo = FastMath.ToDeg(FastMath.Atan2(vientoActual.Y, vientoActual.X));
            if (angulo < 22.5f || angulo >337.5f ) {
                viento += "ESTE";
            }
            else if (angulo < 22.5f+45f )
            {
                viento += "NORESTE";
            }
            else if (angulo < 22.5f + 90f)
            {
                viento += "NORTE";
            }
            else if (angulo < 22.5f + 135f)
            {
                viento += "NOROESTE";
            }
            else if (angulo < 22.5f + 180f)
            {
                viento += "OESTE";
            }
            else if (angulo < 22.5f + 225f)
            {
                viento += "SUROESTE";
            }
            else if (angulo < 22.5f + 270f)
            {
                viento += "SUR";
            }
            else if (angulo < 22.5f + 315f)
            {
                viento += "SURESTE";
            }
            //return ((int)angulo).ToString() + "º " + "DIR VIENTO: " + viento;
            return viento;
        }

        

        public override void close()
        {
            textHud.dispose();
            textHudExplicacionJuego.dispose();
            infoHudBasicaText.dispose();
            textoAyuda.dispose();
            textGameContinue.dispose();
            textGameOver.dispose();
            vistaConstruyendo.dispose();
            vistaInventario.dispose();
            InventarioManager.dispose();
            MeshManager.dispose();

            menuAyuda.dispose();

            infoBoxText.Text = " Disposing...";
            mapa.dispose();
            infoBoxText.dispose();
            effect.Dispose();
            screenQuadVB.Dispose();
            renderTarget2D.Dispose();

        }

    }
}
