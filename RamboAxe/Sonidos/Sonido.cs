using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TgcViewer;
using TgcViewer.Utils.Sound;
namespace AlumnoEjemplos.RamboAxe.Player
{
    public class Sonido
    {
    string currentFile;
    bool loop;
    public Sonido(string audioActual)
    {
        this.loadMp3(audioActual);
    }
    public string getActualSound() { return currentFile; }
    public void setLoop(bool loopValue)
    {
        this.loop = loopValue;
    }
        public void loadMp3(string filePath)
        {
            if (currentFile == null || currentFile != filePath)
            {
                currentFile = filePath;
                //Cargar archivo
                GuiController.Instance.Mp3Player.closeFile();
                GuiController.Instance.Mp3Player.FileName = currentFile;
            }
        }

        public void playMusic()
        {
            TgcMp3Player player = GuiController.Instance.Mp3Player;
            TgcMp3Player.States currentState = player.getStatus();

            if (currentState == TgcMp3Player.States.Open)
            {
                //Reproducir MP3
                player.play(true);
            }
            if (currentState == TgcMp3Player.States.Stopped)
            {
                //Parar y reproducir MP3
                player.closeFile();
                if (loop)
                {
                   player.play(true);
                }
            }
        }
}
}