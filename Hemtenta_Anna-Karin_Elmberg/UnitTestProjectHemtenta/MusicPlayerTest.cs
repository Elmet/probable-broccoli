using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using HemtentaTdd2017;
using HemtentaTdd2017.music;

namespace UnitTestProjectHemtenta
{
    [TestFixture]
    public class MusicPlayerTest
    {
        private Mock<IMediaDatabase> mockMD;
        private MusicPlayer mp;
        private SoundMaker sm;

        [SetUp]
        public void SetUpMusicPlayerTest()
        {
            mp = new MusicPlayer();
            mockMD = new Mock<IMediaDatabase>();
            sm = new SoundMaker();
        }

        public List<ISong> makePlaylist(string search)
        {
            List<ISong> playlist = new List<ISong> {
            new Song() { Title = "Pinball map" },
            new Song() { Title = "Monochromatic stains" },
            new Song() { Title = "Sad but true" },
            new Song() { Title = "Mimic 47" },
            new Song() { Title = "Troopers" }};

            return playlist.Where(t => t.Title.Contains(search)).ToList();
        }

        [Test]
        public void MakeConnectionToDb_Success()
        {
            var fakeMediaDb = mockMD.Object;
            fakeMediaDb.OpenConnection();
            mockMD.Verify(f => f.OpenConnection(), Times.Exactly(1));
        }

        [Test]
        public void CloseConnectionToDb_Success()
        {
            var fakeMediaDb = mockMD.Object;
            fakeMediaDb.CloseConnection();
            mockMD.Verify(f => f.CloseConnection(), Times.Exactly(1));
        }

        [Test]
        public void MakeConnectionToDb_AlreadyOpen_Throws()
        {
            var fakeMediaDb = mockMD.Object;
            mockMD.SetupGet(f => f.IsConnected).Returns(true);

            mockMD.When(() => true).Setup(f => f.OpenConnection()).Throws(new DatabaseAlreadyOpenException());
        }

        [Test]
        [TestCase("stains", 1)]
        [TestCase("map", 1)]
        public void GetSongsFromDb_BySearch_Success(string searchSong, int expectedSongs)
        {            
            mockMD.Setup(l => l.FetchSongs(searchSong)).Returns(makePlaylist(searchSong));
            var fetchSongs = mockMD.Object.FetchSongs(searchSong);

            Assert.That(expectedSongs, Is.EqualTo(fetchSongs.Count));
            mockMD.Verify(x => x.FetchSongs(searchSong), Times.Once);
        }

        [Test]
        [TestCase(null)]
        public void GetSongsFromDb_InvalidInput_Throws(string search)
        {
            Assert.That(() => mp.LoadSongs(search), Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void LoadSongs_AddToPlaylist_Success()
        {
            var fakeMediaDb = mockMD.Object;
            MusicPlayer m = new MusicPlayer(fakeMediaDb);
            mockMD.Setup(x => x.IsConnected).Returns(true);
            mockMD.Setup(f => f.FetchSongs("Pinball map")).Returns(makePlaylist("Pinball map"));

            m.LoadSongs("Pinball map");
            var numSongsInQueue = m.NumSongsInQueue;

            Assert.That(1, Is.EqualTo(numSongsInQueue));
        }

        [Test]
        public void PlayCorrectSong_FromPlaylist_Success()
        {
            var fakeMediaDb = mockMD.Object;
            mp = new MusicPlayer(fakeMediaDb, sm);
            mockMD.Setup(x => x.IsConnected).Returns(true);
            mockMD.Setup(f => f.FetchSongs("Pinball map")).Returns(makePlaylist("Pinball map"));

            mp.LoadSongs("Pinball map");
            mp.Play();

            string songTitleNowPlaying = mp.NowPlaying();
            Assert.That(mp.NowPlaying().Contains("map"));
        }

        [Test]
        [TestCase("Monochromatic stains")]
        public void PlayNextSong_FromPlaylist_Success(string nextSongInList)
        {
            var fakeMediaDb = mockMD.Object;
            mp = new MusicPlayer(fakeMediaDb, sm);
            mockMD.Setup(x => x.IsConnected).Returns(true);
            mockMD.Setup(f => f.FetchSongs("Pinball map")).Returns(makePlaylist("Pinball map"));

            mp.LoadSongs("Pinball map");
            mp.Play();

            string songTitlePlayedBeforeNext = mp.NowPlaying();

            mockMD.Setup(s => s.FetchSongs(nextSongInList)).Returns(makePlaylist(nextSongInList));
            mp.LoadSongs(nextSongInList);
            mp.Play();
            mp.NextSong();
            nextSongInList = mp.NowPlaying();

            Assert.That(nextSongInList != songTitlePlayedBeforeNext);
        }

        [Test]
        public void StopCurrentSong_Success()
        {
            var fakeMediaDb = mockMD.Object;
            mp = new MusicPlayer(fakeMediaDb, sm);
            mockMD.Setup(x => x.IsConnected).Returns(true);
            mockMD.Setup(f => f.FetchSongs("Mimic 47")).Returns(makePlaylist("Mimic 47"));

            mp.LoadSongs("Mimic 47");
            mp.Play();
            var songTitleBeforeStop = mp.NowPlaying();

            mp.Stop();
            var songTitleAfterStop = mp.NowPlaying();

            Assert.AreEqual("Tystnad råder", songTitleAfterStop);
        }
    }
}

