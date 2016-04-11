using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using TgcViewer.Utils.TgcSceneLoader;
using TgcViewer.Utils.Input;
using TgcViewer;
namespace AlumnoEjemplos.Ejemplo_1
{
    /// <summary>
    /// Cámara en primera persona, con movimientos: W, A, S, D, Space, LeftControl
    /// Soporta movimiento con aceleración
    /// </summary>
    public class GameCamera : TgcCamera
    {
        //Constantes de movimiento
        public const float DEFAULT_ROTATION_SPEED = 2f;
        public const float DEFAULT_MOVEMENT_SPEED = 100f;
        public const float DEFAULT_JUMP_SPEED = 100f;
        readonly Vector3 CAMERA_VELOCITY = new Vector3(DEFAULT_MOVEMENT_SPEED, DEFAULT_JUMP_SPEED, DEFAULT_MOVEMENT_SPEED);
        readonly Vector3 CAMERA_POS = new Vector3(0.0f, 1.0f, 0.0f);
        readonly Vector3 CAMERA_ACCELERATION = new Vector3(400f, 400f, 400f);

        //Ejes para ViewMatrix
        readonly Vector3 WORLD_XAXIS = new Vector3(1.0f, 0.0f, 0.0f);
        readonly Vector3 WORLD_YAXIS = new Vector3(0.0f, 1.0f, 0.0f);
        readonly Vector3 WORLD_ZAXIS = new Vector3(0.0f, 0.0f, 1.0f);
        readonly Vector3 DEFAULT_UP_VECTOR = new Vector3(0.0f, 1.0f, 0.0f);

        float accumPitchDegrees;
        Vector3 eye;
        Vector3 xAxis;
        Vector3 yAxis;
        Vector3 zAxis;
        Vector3 viewDir;
        Vector3 lookAt;

        


        
        #region Getters y Setters
        
        bool enable;
        /// <summary>
        /// Habilita o no el uso de la camara
        /// </summary>
        public bool Enable
        {
            get { return enable; }
            set
            {
                enable = value;

                //Si se habilito la camara, cargar como la cámara actual
                if (value)
                {
                    GuiController.Instance.CurrentCamera = this;
                }
            }
        }

        Vector3 acceleration;
        /// <summary>
        /// Aceleracion de la camara en cada uno de sus ejes
        /// </summary>
        public Vector3 Acceleration
        {
            get { return acceleration; }
            set { acceleration = value; }
        }

        bool accelerationEnable;
        /// <summary>
        /// Activa o desactiva el efecto de Aceleración/Desaceleración
        /// </summary>
        public bool AccelerationEnable
        {
            get { return accelerationEnable; }
            set { accelerationEnable = value; }
        }

        Vector3 currentVelocity;
        /// <summary>
        /// Velocidad de desplazamiento actual, teniendo en cuenta la aceleracion
        /// </summary>
        public Vector3 CurrentVelocity
        {
            get { return currentVelocity; }
        }

        Vector3 velocity;
        /// <summary>
        /// Velocidad de desplazamiento de la cámara en cada uno de sus ejes
        /// </summary>
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        /// <summary>
        /// Velocidad de desplazamiento de los ejes XZ de la cámara
        /// </summary>
        public float MovementSpeed
        {
            get { return velocity.X; }
            set
            {
                velocity.X = value;
                velocity.Z = value;
            }
        }

        /// <summary>
        /// Velocidad de desplazamiento del eje Y de la cámara
        /// </summary>
        public float JumpSpeed
        {
            get { return velocity.Y; }
            set { velocity.Y = value; }
        }

       

        Matrix viewMatrix;
        /// <summary>
        /// View Matrix resultante
        /// </summary>
        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        /// <summary>
        /// Posicion actual de la camara
        /// </summary>
        public Vector3 Position
        {
            get { return eye; }
        }

        /// <summary>
        /// Punto hacia donde mira la cámara
        /// </summary>
        public Vector3 LookAt
        {
            get { return lookAt; }
        }

       

        #endregion

        /// <summary>
        /// Crea la cámara con valores iniciales.
        /// Aceleración desactivada por Default
        /// </summary>
        public GameCamera()
        {
            resetValues();

        }

        /// <summary>
        /// Carga los valores default de la camara
        /// </summary>
        public void resetValues()
        {
            accumPitchDegrees = 0.0f;

            eye = new Vector3(0.0f, 0.0f, 0.0f);
            xAxis = new Vector3(1.0f, 0.0f, 0.0f);
            yAxis = new Vector3(0.0f, 1.0f, 0.0f);
            zAxis = new Vector3(0.0f, 0.0f, 1.0f);
            viewDir = new Vector3(0.0f, 0.0f, 1.0f);
            lookAt = eye + viewDir;

            accelerationEnable = false;
            acceleration = CAMERA_ACCELERATION;
            currentVelocity = new Vector3(0.0f, 0.0f, 0.0f);
            velocity = CAMERA_VELOCITY;
            viewMatrix = Matrix.Identity;
            setPosition(CAMERA_POS);

        }


        /// <summary>
        /// Configura la posicion de la cámara
        /// </summary>
        private void setCamera(Vector3 eye, Vector3 target, Vector3 up)
        {
            this.eye = eye;

            zAxis = target - eye;
            zAxis.Normalize();

            viewDir = zAxis;
            lookAt = eye + viewDir;

            xAxis = Vector3.Cross(up, zAxis);
            xAxis.Normalize();

            yAxis = Vector3.Cross(zAxis, xAxis);
            yAxis.Normalize();
            //xAxis.Normalize();

            viewMatrix = Matrix.Identity;

            viewMatrix.M11 = xAxis.X;
            viewMatrix.M21 = xAxis.Y;
            viewMatrix.M31 = xAxis.Z;
            viewMatrix.M41 = -Vector3.Dot(xAxis, eye);

            viewMatrix.M12 = yAxis.X;
            viewMatrix.M22 = yAxis.Y;
            viewMatrix.M32 = yAxis.Z;
            viewMatrix.M42 = -Vector3.Dot(yAxis, eye);

            viewMatrix.M13 = zAxis.X;
            viewMatrix.M23 = zAxis.Y;
            viewMatrix.M33 = zAxis.Z;
            viewMatrix.M43 = -Vector3.Dot(zAxis, eye);

            // Extract the pitch angle from the view matrix.
            accumPitchDegrees = Geometry.RadianToDegree((float)-Math.Asin((double)viewMatrix.M23));
        }

        /// <summary>
        /// Configura la posicion de la cámara
        /// </summary>
        public void setCamera(Vector3 pos, Vector3 lookAt)
        {
            setCamera(pos, lookAt, DEFAULT_UP_VECTOR);
        }

        /// <summary>
        /// Moves the camera by dx world units to the left or right; dy
        /// world units upwards or downwards; and dz world units forwards
        /// or backwards.
        /// </summary>
        public void move(float dx, float dy, float dz)
        {

            Vector3 auxEye = this.eye;
            Vector3 forwards;

            // Calculate the forwards direction. Can't just use the camera's local
            // z axis as doing so will cause the camera to move more slowly as the
            // camera's view approaches 90 degrees straight up and down.
            forwards = Vector3.Cross(xAxis, WORLD_YAXIS);
            forwards.Normalize();


            auxEye += xAxis * dx;
            auxEye += WORLD_YAXIS * dy;
            auxEye += forwards * dz;

            setPosition(auxEye);
        }

        /// <summary>
        /// Moves the camera by the specified amount of world units in the specified
        /// direction in world space. 
        /// </summary>
        private void move(Vector3 direction, Vector3 amount)
        {
            eye.X += direction.X * amount.X;
            eye.Y += direction.Y * amount.Y;
            eye.Z += direction.Z * amount.Z;

            reconstructViewMatrix(false);
        }

       


        public void setPosition(Vector3 pos)
        {
            eye = pos;
            reconstructViewMatrix(false);
        }

        public void rotateFirstPerson(float headingDegrees, float pitchDegrees)
        {
            accumPitchDegrees += pitchDegrees;

            if (accumPitchDegrees > 90.0f)
            {
                pitchDegrees = 90.0f - (accumPitchDegrees - pitchDegrees);
                accumPitchDegrees = 90.0f;
            }

            if (accumPitchDegrees < -90.0f)
            {
                pitchDegrees = -90.0f - (accumPitchDegrees - pitchDegrees);
                accumPitchDegrees = -90.0f;
            }

            float heading = Geometry.DegreeToRadian(headingDegrees);
            float pitch = Geometry.DegreeToRadian(pitchDegrees);

            Matrix rotMtx;
            Vector4 result;

            // Rotate camera's existing x and z axes about the world y axis.
            if (heading != 0.0f)
            {
                rotMtx = Matrix.RotationY(heading);

                result = Vector3.Transform(xAxis, rotMtx);
                xAxis = new Vector3(result.X, result.Y, result.Z);

                result = Vector3.Transform(zAxis, rotMtx);
                zAxis = new Vector3(result.X, result.Y, result.Z);
            }

            // Rotate camera's existing y and z axes about its existing x axis.
            if (pitch != 0.0f)
            {
                rotMtx = Matrix.RotationAxis(xAxis, pitch);

                result = Vector3.Transform(yAxis, rotMtx);
                yAxis = new Vector3(result.X, result.Y, result.Z);

                result = Vector3.Transform(zAxis, rotMtx);
                zAxis = new Vector3(result.X, result.Y, result.Z);
            }
            reconstructViewMatrix(true);
        }



        /// <summary>
        /// Reconstruct the view matrix.
        /// </summary>
        private void reconstructViewMatrix(bool orthogonalizeAxes)
        {
            if (orthogonalizeAxes)
            {
                // Regenerate the camera's local axes to orthogonalize them.

                zAxis.Normalize();

                yAxis = Vector3.Cross(zAxis, xAxis);
                yAxis.Normalize();

                xAxis = Vector3.Cross(yAxis, zAxis);
                xAxis.Normalize();

                viewDir = zAxis;
                lookAt = eye + viewDir;
            }

            // Reconstruct the view matrix.

            viewMatrix.M11 = xAxis.X;
            viewMatrix.M21 = xAxis.Y;
            viewMatrix.M31 = xAxis.Z;
            viewMatrix.M41 = -Vector3.Dot(xAxis, eye);

            viewMatrix.M12 = yAxis.X;
            viewMatrix.M22 = yAxis.Y;
            viewMatrix.M32 = yAxis.Z;
            viewMatrix.M42 = -Vector3.Dot(yAxis, eye);

            viewMatrix.M13 = zAxis.X;
            viewMatrix.M23 = zAxis.Y;
            viewMatrix.M33 = zAxis.Z;
            viewMatrix.M43 = -Vector3.Dot(zAxis, eye);

            viewMatrix.M14 = 0.0f;
            viewMatrix.M24 = 0.0f;
            viewMatrix.M34 = 0.0f;
            viewMatrix.M44 = 1.0f;
        }

        /// <summary>
        /// Actualiza los valores de la camara
        /// </summary>
        public void updateCamera()
        {
            //Si la camara no está habilitada, no procesar el resto del input
            if (!enable)
            {
                return;
            }

            float elapsedTimeSec = GuiController.Instance.ElapsedTime;

           
            //Obtener direccion segun entrada de teclado
//            Vector3 direction = getMovementDirection(d3dInput);
            
            

  //          updatePosition(direction, elapsedTimeSec);
        }

        /// <summary>
        /// Actualiza la ViewMatrix, si es que la camara esta activada
        /// </summary>
        public void updateViewMatrix(Microsoft.DirectX.Direct3D.Device d3dDevice)
        {
            if (!enable)
            {
                return;
            }

            d3dDevice.Transform.View = viewMatrix;
        }



        public Vector3 getPosition()
        {
            return eye;
        }

        public Vector3 getLookAt()
        {
            return lookAt;
        }

        /// <summary>
        /// String de codigo para setear la camara desde GuiController, con la posicion actual y direccion de la camara
        /// </summary>
        internal string getPositionCode()
        {
            //TODO ver de donde carajo sacar el LookAt de esta camara
            Vector3 lookAt = this.LookAt;

            return "GuiController.Instance.setCamera(new Vector3(" +
                TgcParserUtils.printFloat(eye.X) + "f, " + TgcParserUtils.printFloat(eye.Y) + "f, " + TgcParserUtils.printFloat(eye.Z) + "f), new Vector3(" +
                TgcParserUtils.printFloat(lookAt.X) + "f, " + TgcParserUtils.printFloat(lookAt.Y) + "f, " + TgcParserUtils.printFloat(lookAt.Z) + "f));";
        }



    }
}