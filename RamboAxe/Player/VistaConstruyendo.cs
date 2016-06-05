using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using AlumnoEjemplos.RamboAxe.GameObjects;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcGeometry;
using System.Drawing;
using TgcViewer;

namespace AlumnoEjemplos.RamboAxe.Player
{
    public class VistaConstruyendo: Observador
    {
        private const float DISTANCIA_CONSTRUIBLE = 50.0f;
        private TgcMesh construyendo;
        private TgcMesh meshVerde;
        private TgcMesh meshRojo;
        private Vector3 ultimoLookAt;
        private Vector3 ultimoEye;
        private bool estaConstruyendo;

        public VistaConstruyendo()
        {
            estaConstruyendo = false;
            CharacterSheet.getInstance().agregarObservador(this);
        }

        private void init()
        {
            meshVerde = CharacterSheet.getInstance().construyendo.getMesh().clone("ConstruyendoCorrecto");
            meshRojo = meshVerde.clone("ConstruyendoIncorrecto");
            meshVerde.setColor(Color.LightGreen);
            meshRojo.setColor(Color.Salmon);
            construyendo = meshVerde;
            estaConstruyendo = true;
        }

        public void render()
        {
            if(!estaConstruyendo){
                return;
            }
            if (ultimoLookAt != EjemploAlumno.getInstance().camera.lookAt || ultimoEye != EjemploAlumno.getInstance().camera.eye)
            {
                moverConstruible();
            }
            construyendo.render();
        }

        public void dispose()
        {
            if(estaConstruyendo){
                estaConstruyendo = false;
                meshVerde.dispose();
                meshRojo.dispose();
            }
        }

        /// <summary>
        /// Mueve el mesh del construible con respecto a la camara
        /// </summary>
        private void moverConstruible()
        {
            // TODO: ver de mover a CharacterSheet
            /* Variables de control */
            ultimoEye = EjemploAlumno.getInstance().camera.eye;
            ultimoLookAt = EjemploAlumno.getInstance().camera.lookAt;
            /* Obtengo el angulo en el que esta mirando */
            float radians = FastMath.Atan2(
                ultimoLookAt.Z - ultimoEye.Z, 
                ultimoLookAt.X - ultimoEye.X
            );
            /* Cambio el angulo para centrarlo en la pantalla */
            radians -= FastMath.ToRad(33.3f);
            /* Lo muevo a la posicion con respecto de la camara */
            Vector3 meshPosition = new Vector3(
                ultimoEye.X + DISTANCIA_CONSTRUIBLE * FastMath.Cos(radians),
                0.0f,
                ultimoEye.Z + DISTANCIA_CONSTRUIBLE * FastMath.Sin(radians)
            );
            Cuadrante cuad = EjemploAlumno.getInstance().mapa.getCuadranteForPosition(meshPosition);
            if(cuad.getTerrain() != null){
                cuad.getTerrain().interpoledHeight(meshPosition.X, meshPosition.Z, out meshPosition.Y);
            }
            meshVerde.Position = meshPosition;
            meshVerde.Rotation = new Vector3(
                0.0f,
                -1 * radians,
                0.0f
            );
            meshRojo.Position = meshVerde.Position;
            meshRojo.Rotation = meshVerde.Rotation;
            if(CharacterSheet.getInstance().estaConstruyendo){
                CharacterSheet.getInstance().construyendo.getMesh().Position = meshVerde.Position;
                CharacterSheet.getInstance().construyendo.getMesh().Rotation = meshVerde.Rotation;
            }
        }

        public void cambioObservable()
        {
            if(estaConstruyendo){
                if (!CharacterSheet.getInstance().estaConstruyendo)
                {
                    dispose();
                }
            }
            else
            {
                if(CharacterSheet.getInstance().estaConstruyendo){
                    init();
                }
            }
        }
    }
}
