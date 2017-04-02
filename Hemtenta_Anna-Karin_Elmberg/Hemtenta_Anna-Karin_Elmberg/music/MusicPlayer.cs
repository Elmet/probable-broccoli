using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.music
{
    public class MusicPlayer : IMusicPlayer
    {
        private IMediaDatabase mediaDb;
        private SoundMaker sm;
        List<ISong> songList = new List<ISong>();

        public MusicPlayer()
        {
        }

        public MusicPlayer(IMediaDatabase mediaDb)
        {
            this.mediaDb = mediaDb;
        }

        public MusicPlayer(IMediaDatabase mediaDb, SoundMaker sm)
        {
            this.mediaDb = mediaDb;
            this.sm = sm;
        }

        public int NumSongsInQueue
        {
            get
            {
                return songList.Count;
            }
        }

        public void LoadSongs(string search)
        {
            if (!mediaDb.IsConnected)
            {
                throw new DatabaseClosedException();
            }

            if (search != null)
            {
                songList = mediaDb.FetchSongs(search);
            }

            mediaDb.CloseConnection();
        }

        public void NextSong()
        {
            if (NumSongsInQueue > 0)
            {
                sm.Play(songList.FirstOrDefault());
            }

            else
            {
                Stop();
            }
        }

        public string NowPlaying()
        {
            if (string.IsNullOrEmpty(sm.NowPlaying))
            {
                return "Tystnad råder";
            }

            else
            {
                return sm.NowPlaying;
            }
        }

        public void Play()
        {
            if (!mediaDb.IsConnected)
            {
                throw new DatabaseClosedException();
            }

            if (sm.NowPlaying == "Tystnad råder")
            {
                sm.Play(songList.FirstOrDefault());
            }
        }

        public void Stop()
        {
            sm.Stop();
        }
    }
}
