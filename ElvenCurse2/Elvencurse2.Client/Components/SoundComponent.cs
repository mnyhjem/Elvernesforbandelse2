using System;
using System.Collections.Generic;
using System.Linq;
using Elvencurse2.Client.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Elvencurse2.Client.Components
{
    public class SoundComponent 
    {
        private readonly Game _game;
        private Dictionary<string, Song> _music;

        public SoundComponent(Game game)
        {
            _game = game;
        }

        public void Load()
        {
            _music = _game.Content.LoadListContent<Song>("Music");
        }

        public void PlayMusic(string songname)
        {
            if (!_music.ContainsKey(songname))
            {
                throw new ArgumentException("Song not found");
            }

            MediaPlayer.Play(_music[songname]);

            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            PlayRandom();
        }

        private void PlayRandom()
        {
            // afspil tilfældig..
            var rnd = new Random();

            var keys = _music.Keys.ToList();

            var name = keys[rnd.Next(0, keys.Count - 1)];
            PlayMusic(name);
        }
    }
}
