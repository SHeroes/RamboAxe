using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer.Utils.TgcSceneLoader;
using AlumnoEjemplos.RamboAxe.GameObjects;
using Microsoft.DirectX;
using TgcViewer.Utils.TgcGeometry;
using System.Drawing;

namespace AlumnoEjemplos.RamboAxe.Player
{
    public class VistaConstruyendo
    {
        private const float DISTANCIA_CONSTRUIBLE = 50.0f;
        private EjemploAlumno juego;
        //private TgcElli
        private TgcMesh construyendo;
        private TgcMesh meshVerde;
        private TgcMesh meshRojo;
        private Vector3 ultimoLookAt;
        private Vector3 ultimoEye;

        public VistaConstruyendo(EjemploAlumno juego)
        {
            this.juego = juego;
            init();
        }

        private void init()
        {
            meshVerde = MapaDelJuego.getGameMesh(4).clone("ConstruyendoCorrecto");
            meshRojo = meshVerde.clone("ConstruyendoIncorrecto");
            meshVerde.setColor(Color.LightGreen);
            meshRojo.setColor(Color.Salmon);
            construyendo = meshVerde;
        }

        public void render()
        {
            if (ultimoLookAt != juego.camera.lookAt || ultimoEye != juego.camera.eye)
            {
                moverConstruible();
            }
            construyendo.render();
        }

        public void dispose()
        {
            meshVerde.dispose();
            meshRojo.dispose();
            juego = null;
        }

        /// <summary>
        /// Mueve el mesh del construible con respecto a la camara
        /// </summary>
        private void moverConstruible()
        {
            /* Variables de control */
            ultimoEye = juego.camera.eye;
            ultimoLookAt = juego.camera.lookAt;
            /* Obtengo el angulo en el que esta mirando */
            float radians = FastMath.Atan2(
                ultimoLookAt.Z - ultimoEye.Z, 
                ultimoLookAt.X - ultimoEye.X
            );
            /* Cambio el angulo para centrarlo en la pantalla */
            radians -= FastMath.ToRad(33.3f);
            /* Lo muevo a la posicion con respecto de la camara */
            meshVerde.Position = new Vector3(
                ultimoEye.X + DISTANCIA_CONSTRUIBLE * FastMath.Cos(radians),
                0.0f,
                ultimoEye.Z + DISTANCIA_CONSTRUIBLE * FastMath.Sin(radians)
            );
            meshVerde.Rotation = new Vector3(
                0.0f,
                -1 * radians,
                0.0f
            );
            meshRojo.Position = meshVerde.Position;
            meshRojo.Rotation = meshVerde.Rotation;
        }
    }
}
