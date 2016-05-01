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

namespace AlumnoEjemplos.SurvivalBars
{
    public class SurvivalBarsExample : TgcExample
    {
        TgcSprite sprite;
        


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

        public override void init()
        {
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            //Crear Sprite
            sprite = new TgcSprite();
            sprite.Texture = TgcTexture.createTexture(GuiController.Instance.AlumnoEjemplosMediaDir + "\\survivalBars\\lifeBar.png");

            //Ubicarlo centrado en la pantalla
            Size screenSize = GuiController.Instance.Panel3d.Size;
            Size textureSize = sprite.Texture.Size;
            sprite.Position = new Vector2((float)0.0, FastMath.Max(screenSize.Height / 2 - textureSize.Height / 2, 0));
            sprite.Scaling = new Vector2((float)0.61, (float)0.28);

            /*
            //Modifiers para variar parametros del sprite
            GuiController.Instance.Modifiers.addVertex2f("position", new Vector2(0, 0), new Vector2(screenSize.Width, screenSize.Height), sprite.Position);
            GuiController.Instance.Modifiers.addVertex2f("scaling", new Vector2(0, 0), new Vector2(4, 4), sprite.Scaling);
            GuiController.Instance.Modifiers.addFloat("rotation", 0, 360, 0);
            */
        }

        public override void close()
        {
            sprite.dispose();
        }

        public override void render(float elapsedTime)
        {
            Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

            /*
            //Actualizar valores cargados en modifiers
            sprite.Position = (Vector2)GuiController.Instance.Modifiers["position"];
            sprite.Scaling = (Vector2)GuiController.Instance.Modifiers["scaling"];
            sprite.Rotation = FastMath.ToRad((float)GuiController.Instance.Modifiers["rotation"]);
            */

            //Iniciar dibujado de todos los Sprites de la escena (en este caso es solo uno)
            GuiController.Instance.Drawer2D.beginDrawSprite();

            //Dibujar sprite (si hubiese mas, deberian ir todos aquí)
            sprite.render();

            //Finalizar el dibujado de Sprites
            GuiController.Instance.Drawer2D.endDrawSprite();



            TgcD3dInput input = GuiController.Instance.D3dInput;

            if (input.keyPressed(Key.Tab))
            {



            }

        }
    }
}
