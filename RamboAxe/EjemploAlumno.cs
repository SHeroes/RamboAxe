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
namespace AlumnoEjemplos.RamboAxe
{
    public class EjemploAlumno : TgcExample
    {
        Barra barraInteraccion;
        BarraEstatica barraHambre; BarraEstatica barraVida;
        BarraEstatica barraSed;
        string[] vectorTemperaturas = new string[5] {"CONGELADOR","FRIO","TEMPLADO","CALUROSO","ARDIENTE"};

        int temperaturaCuadranteActual;
        float distanciaObjeto = 0;
        TgcPickingRay pickingRay;
        static EjemploAlumno game;
        Vector3 collisionPoint;
        GameObjectAbstract selectedGameObject;
        double currentCuadrantX, currentCuadrantZ = 1;
        CharacterSheet pj = CharacterSheet.getInstance();
        VistaInventario vistaInventario;
        VistaConstruyendo vistaConstruyendo;
        bool firstRun = true;
        bool hachaEquipada = true;

        MapaDelJuego mapa;
        TgcSprite spriteHacha;
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
            return "Entendiendo como crear el mundo 3d.";
        }
        TgcD3dInput d3dInput;
        TgcText2d text;
        TgcText2d text2;
        TgcText2d text3;
        SkyBox skyBox;
        public GameCamera camera;
        TgcPlaneWall ground;
        public bool falling = false;
        List<Collider> objetosColisionables = new List<Collider>();
        ElipsoidCollisionManager collisionManager;

        TgcElipsoid characterElipsoid;
        double prevCuadrantX = 1;
        double prevCuadrantZ = 1;
        float width = 2000;
        float height = 2000;

        
        TgcPlaneWall[][] floors = new TgcPlaneWall [3][];
        
        public override void init()
        {

            EjemploAlumno.game = this;
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            //Iniciarlizar PickingRay
            pickingRay = new TgcPickingRay();
            TgcTexture texture = TgcTexture.createTexture(d3dDevice, GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\" + "tile_1.png");
           // string terrainHm = GuiController.Instance.AlumnoEjemplosMediaDir + "RamboAxe\\" + "fps2\\" + "hm.jpg";

            
            ground = new TgcPlaneWall(new Vector3(0,0,0),new Vector3(width,0,height),TgcPlaneWall.Orientations.XZplane,texture);

            for(int i = 0;i<3;i++){
                floors[i] = new TgcPlaneWall[3];
                for(int x = 0;x<3;x++){
                    floors[i][x]= ground.clone();
                }
            }
            

            for(int i = 0;i<3;i++){
                for(int x = 0;x<3;x++){
                    floors[i][x].setExtremes(new Vector3((1 + i) * width, 0, (1 + x) * height), new Vector3((1 + i) * width + width, 0, (1 + x) * height+height));
                    floors[i][x].updateValues();
                }
            }

            d3dInput = GuiController.Instance.D3dInput;

            spriteHacha = new TgcSprite();
            spriteHacha.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosDir + "RamboAxe\\Media\\hacha.png");
            spriteHacha.Scaling = new Vector2(1.5f, 1.5f);

            //Ubicado centrado en la pantalla
            Size screenSize = GuiController.Instance.Panel3d.Size;
            Size textureSize = spriteHacha.Texture.Size;
            spriteHacha.Position = new Vector2(220,380);
            GuiController.Instance.Logger.log(String.Format("Posicion sprite: X:{0} , Y:{1}",spriteHacha.Position.X,spriteHacha.Position.Y));

            characterElipsoid = new TgcElipsoid(new Vector3(0, 250, 0), new Vector3(12, 48, 12));
            this.initMapa();
            this.initInventario();
            this.initCollisions();
            this.initCamera();
            vistaConstruyendo = new VistaConstruyendo(this);
            this.hud();
            this.skyboxInit();
            this.initBarrasVida();
           
        }
        public void initMapa(){
            mapa = new MapaDelJuego((int)width,(int)height);
            
            
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
            barraHambre.init(BarraEstatica.RED, 80, 460, 0, 100);
            barraSed.init(BarraEstatica.VIOLET, (barrasWidth) + 80, 460, 0, 100);
            barraVida.init(BarraEstatica.YELLOW, (barrasWidth * 2) + 80, 460, 0, 100);


            barraVida.valorActual = 0.5f;
            barraVida.barTitleText       = "Vida";
            barraSed.barTitleText = "Nivel de Sed";
            barraHambre.barTitleText          = "Hambre";
        }

        public void handleInput() {
            TgcD3dInput input = GuiController.Instance.D3dInput;

            bool abierto = vistaInventario.abierto;
            bool selected = false;
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
            if(abierto){
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
                        string consumido = vistaInventario.consumirActual();
                        if(consumido == "Racion"){
                           //barraHambre.agregarPorcentajeABarra(0.1f);
                        }
                        else
                        {
                            if (consumido == "Hacha")
                            {
                                if (!hachaEquipada)
                                {
                                    hachaEquipada = true;
                                    GuiController.Instance.Drawer2D.beginDrawSprite();
                                    spriteHacha.render();
                                    GuiController.Instance.Drawer2D.endDrawSprite();
                                }
                                
                            }
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

        public void agregarPiedraTallada()
        {
           //TODO
        }




        public void initInventario() {
            InventarioManager.init();
            vistaInventario = new VistaInventario();
            pj.getInventario().agregar(InventarioManager.Palos);
            pj.getInventario().agregar(InventarioManager.Leña);
            pj.getInventario().agregar(InventarioManager.Piedra);
            pj.getInventario().agregar(InventarioManager.Piedra);
            pj.getInventario().agregar(InventarioManager.RecetaCasa);
        }

        public void initCamera()
        {
            camera = new GameCamera(this);
           
            //Configurar FPS Camara
            camera.Enable = true;
            //            camera.setCamera(new Vector3(-722.6171f, 495.0046f, -31.2611f), new Vector3(164.9481f, 35.3185f, -61.5394f));
            camera.setCamera(new Vector3(floors[1][1].BoundingBox.calculateBoxCenter().X, floors[1][1].BoundingBox.calculateBoxCenter().Y + 125, floors[1][1].BoundingBox.calculateBoxCenter().Z), new Vector3(floors[1][1].BoundingBox.calculateBoxCenter().X + 50, floors[1][1].BoundingBox.calculateBoxCenter().Y, floors[1][1].BoundingBox.calculateBoxCenter().Z));
        }
        public void initCollisions()
        {
            objetosColisionables.Clear();
            for (int i = 0; i < 3; i++)
            {
                for (int x = 0; x < 3; x++)
                {
                    objetosColisionables.Add(BoundingBoxCollider.fromBoundingBox(floors[i][x].BoundingBox));
                }
            }

            collisionManager = new ElipsoidCollisionManager();
            collisionManager.GravityEnabled = true;
            collisionManager.GravityForce = new Vector3(0, -1.2f, 0);
        }


        public override void render(float elapsedTime)
        {
            handleInput();

            if (hachaEquipada)
            {
                GuiController.Instance.Drawer2D.beginDrawSprite();
                spriteHacha.render();
                GuiController.Instance.Drawer2D.endDrawSprite();
            }
            
            if (!vistaInventario.abierto)
            {
                barraSed.render(elapsedTime);
                barraVida.render(elapsedTime);
                barraHambre.render(elapsedTime);
            }
            
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;
            if (barraInteraccion != null) barraInteraccion.render(elapsedTime);
            vistaInventario.render();
            // TODO: descomentar para ver el construyendo actual
            //vistaConstruyendo.render();
            //box.render();
           // text.render();

            temperaturaCuadranteActual = mapa.getCuadrante((int)currentCuadrantX, (int)currentCuadrantZ).getTempratura() + HoraDelDia.getInstance().getMomentoDelDia();


           text3.Text = "TEMPERATURA: " + vectorTemperaturas[temperaturaCuadranteActual] + "  indiceVector:" + temperaturaCuadranteActual + "  x:" + currentCuadrantX + "  z:" + currentCuadrantZ;
            
            text3.render();
            text2.render();
            skyBox.Center = camera.Position;
            skyBox.updateValues();
            skyBox.render();
           
          //  currentScene.renderAll();
            String floorCords ="";
            for (int i = 0; i < 3; i++)
            {
                for (int x = 0; x < 3; x++)
                {
                    floors[i][x].render();
                    floorCords+= "\n["+i.ToString()+"/"+x.ToString()+"]"+floors[i][x].Position.X+floors[i][x].Position.Z;
                }
            }
            currentCuadrantX = (Math.Floor(characterElipsoid.Position.X / width)-1);
            currentCuadrantZ = (Math.Floor(characterElipsoid.Position.Z / width)-1);
            if (currentCuadrantX != prevCuadrantX || currentCuadrantZ != prevCuadrantZ || firstRun)
            {
                firstRun = false;

                collisionManager.GravityEnabled = false;
                objetosColisionables.Clear();
                for (int x = 0; x < 3; x++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        TgcPlaneWall f = floors[x][z];
                        float nx = ((float)(width * (currentCuadrantX - prevCuadrantX))) + f.Position.X;
                        float nz = ((float)(width * (currentCuadrantZ - prevCuadrantZ))) + f.Position.Z;
                        f.setExtremes(new Vector3(nx, 0, nz), new Vector3(nx + width, 0, nz + width));
                        f.updateValues();
                        objetosColisionables.Add(BoundingBoxCollider.fromBoundingBox(floors[x][z].BoundingBox));

                        foreach (GameObjectAbstract go in mapa.getCuadrante((int)(currentCuadrantX + (x - 1)),((int) currentCuadrantZ + (z - 1))).getObjects())
                        {
                            TgcMesh mesh = go.getMesh();
                            go.move(new Vector3(go.getX() + nx, go.getY() + f.Position.Y, go.getZ() + nz));
                            if (x == 1 && z == 1)
                            {
                                objetosColisionables.Add(BoundingBoxCollider.fromBoundingBox(mesh.BoundingBox));
                            }
                        }
                    }
                }

                collisionManager.GravityEnabled = true;
                prevCuadrantX = currentCuadrantX;
                prevCuadrantZ = currentCuadrantZ;

            }
            for (int x = 0; x < 3; x++)
            {
                for (int z = 0; z < 3; z++)
                {
                    foreach (GameObjectAbstract go in mapa.getCuadrante((int)(currentCuadrantX+(x-1)), ((int)currentCuadrantZ+z-1)).getObjects())
                    {
                        TgcMesh mesh = go.getMesh();
                        mesh.render();
                    }
                }
            }
        //   text.Text = floorCords + "\nCharacter: "+ characterElipsoid.Position.Z.ToString()+  " "  + characterElipsoid.Position.X.ToString() + "\n" + currentCuadrantZ.ToString() + " " + currentCuadrantX.ToString()+"\n"+characterElipsoid.Center.Y+" \n"+distanciaObjeto;
           if (selectedGameObject != null)
           {
               if (barraInteraccion != null && !barraInteraccion.isActive())
               {
                   barraInteraccion.dispose();
                   barraInteraccion = null;
                   selectedGameObject.use();
               }
           }
           
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
        public void skyboxInit()
        {

            skyBox = new SkyBox();
            skyBox.Center = new Vector3(0, 500, 0);
            skyBox.Size = new Vector3(10000, 10000, 10000);
            string texturesPath = GuiController.Instance.ExamplesMediaDir + "Texturas\\Quake\\SkyBox LostAtSeaDay\\";
            skyBox.setFaceTexture(SkyBox.SkyFaces.Up, texturesPath + "lostatseaday_up.jpg");
            skyBox.setFaceTexture(SkyBox.SkyFaces.Down, texturesPath + "lostatseaday_dn.jpg");
            skyBox.setFaceTexture(SkyBox.SkyFaces.Left, texturesPath + "lostatseaday_lf.jpg");
            skyBox.setFaceTexture(SkyBox.SkyFaces.Right, texturesPath + "lostatseaday_rt.jpg");
            skyBox.setFaceTexture(SkyBox.SkyFaces.Front, texturesPath + "lostatseaday_bk.jpg");
            skyBox.setFaceTexture(SkyBox.SkyFaces.Back, texturesPath + "lostatseaday_ft.jpg");
            skyBox.updateValues();
        }
     
           public void hud()
        {
            text = new TgcText2d();
            text.Text = "Texto del hud.";
            text.Align = TgcText2d.TextAlign.LEFT;
            text.Position = new Point(5, 20);
            text.Size = new Size(310, 100);
            text.Color = Color.Gold;
            text.Position = new Point(5, 20);


            text2 = new TgcText2d();
            text2.Text = "-\"wasd\" para moverse    -\"P\" para capturar el mouse    -\"I\" para el inventario    -\"Click Izq\" para interactuar" ;
            text2.Align = TgcText2d.TextAlign.LEFT;
            text2.Size = new Size(800, 100);
            text2.Color = Color.Gold;
            text2.Position = new Point(75, 10);

            text3 = new TgcText2d();
            text3.Text = "-TEMPERATURA: DESCONOCIDA VITEH";
            text3.Align = TgcText2d.TextAlign.CENTER;
            text3.Size = new Size(800, 100);
            text3.Color = Color.Red;
            text3.Position = new Point(115, 30);

        }


           public Vector3[] tryToMovePlayer(Vector3 from, Vector3 direction)
           {
               Vector3 elipsoidCorrection = new Vector3(from.X - characterElipsoid.Position.X, 0, from.Z - characterElipsoid.Position.Z);

               Vector3 realMovement = collisionManager.moveCharacter(characterElipsoid, elipsoidCorrection, objetosColisionables);

               Vector3 prevPos = new Vector3(characterElipsoid.Position.X, characterElipsoid.Position.Y, characterElipsoid.Position.Z);


               realMovement = collisionManager.moveCharacter(characterElipsoid, direction, objetosColisionables);

               Vector3[] returnValues = new Vector3[2];


               returnValues[0] = prevPos;
               returnValues[1] = realMovement;
               return returnValues;
           }

           public void playerCrouchs()
           {
               characterElipsoid.setValues(characterElipsoid.Center + new Vector3(0, 0, 0), new Vector3(12, 28, 12));
             //  camera.setPosition(characterElipsoid.Position);
           }
           public void playerStands()
           {
               characterElipsoid.setValues(characterElipsoid.Center + new Vector3(0, +20, 0), new Vector3(12, 48, 12));
              // camera.setPosition(characterElipsoid.Position);
           }
           public void playerPrevJump()
           {
               characterElipsoid.setValues(characterElipsoid.Center + new Vector3(0, 0, 0), new Vector3(12, 38, 12));
               
           }
           public void playerJumps()
           {
               characterElipsoid.setValues(characterElipsoid.Center + new Vector3(0, +120, 0), new Vector3(12, 48, 12));
               camera.setPosition(characterElipsoid.Position);
           }

        public override void close()
        {
            vistaConstruyendo.dispose();
            vistaInventario.dispose();
            InventarioManager.dispose();
        }
    }
}
