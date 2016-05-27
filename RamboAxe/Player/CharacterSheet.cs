using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlumnoEjemplos.RamboAxe.Inventario;
using Microsoft.DirectX;
namespace AlumnoEjemplos.RamboAxe.Player
{
    class CharacterSheet
    {
        int vida;
        int sed;
        int frio;
        public float velocity = 1;
        public float terminalVelocity = 5;
        public float jumpHeight = 0;
        private float nextFall = 0;
        public float playerHeight = 20;
        public void jump()
        {
            if (jumpHeight == 0)
            {
                jumpHeight = 100;
                nextFall = -10;
            }
        }
        public void crouch()
        {
            if (jumpHeight == 0)
            {
                playerHeight = 5;
            }
        }
        public void stand()
        {
            playerHeight = 20;
        }
        public void fall()
        {
            if (jumpHeight > 0)
            {
                nextFall++;
                if (nextFall > terminalVelocity)
                {
                    nextFall = terminalVelocity;
                }
                jumpHeight = jumpHeight - nextFall;
                if (jumpHeight < 0)
                {
                    jumpHeight = 0;
                }
            }

        }
        public Vector3 position = new Vector3(0, 0, 0);

        public int pesoMaximo { get; private set; }

        ModeloInventario inv;
        private static CharacterSheet singleton;
        private CharacterSheet()
        {
            pesoMaximo = 100;
            inv = new ModeloInventario();
        }

        public static CharacterSheet getInstance(){
            if(singleton==null){
                singleton = new CharacterSheet();
            }
            return singleton;
        }

        public ModeloInventario getInventario()
        {
            return inv;
        }


    }
}
