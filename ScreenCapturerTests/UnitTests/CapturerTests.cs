﻿namespace ScreenCapturerTests.UnitTests
{
    using System.Windows.Forms;

    using NUnit.Framework;

    using ScreenCapturer.Code;

    [TestFixture]
    class CapturerTests
    {
        private Capturer capturer;

        private MouseEventArgs mouseLeftDown1, mouseLeftUp1, mouseDragDown, mouseDragUp;

        [SetUp]
        public void SetUp()
        {
            capturer = new Capturer();

            // Regular click in one spot.
            mouseLeftDown1 = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);
            mouseLeftUp1 = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);

            // Click-and-drag
            mouseDragDown = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);
            mouseDragUp = new MouseEventArgs(MouseButtons.Left, 1, 100, 100, 0);
        }

        [TearDown]
        public void TearDown()
        {
            capturer = null;
        }

        [Test]
        public void TakeShotTest()
        {
            // No shots in capturer at start.
            Assert.AreEqual(0, capturer.Shots.Count);

            capturer.IsTakingShots = true;

            // Take shot and check
            capturer.TakeShot(mouseLeftDown1, mouseLeftUp1, false);
            Assert.AreEqual(1, capturer.Shots.Count);

            capturer.TakeShot(mouseLeftDown1, mouseLeftUp1, false);
            capturer.TakeShot(mouseLeftDown1, mouseLeftUp1, false);
            Assert.AreEqual(3, capturer.Shots.Count);

            capturer.TakeShot(mouseDragDown, mouseDragUp, false);
            Assert.AreEqual(4, capturer.Shots.Count);
        }

        [Test]
        public void CheckShotsBufferTest()
        {
            // No shots in capturer at start.
            Assert.AreEqual(0, capturer.Shots.Count);

            capturer.IsTakingShots = true;

            // Fill the Shots to the "buffer limit"
            for (var i = 0; i < capturer.Buffersize+1; i++)
            {
                capturer.TakeShot(mouseLeftDown1, mouseLeftUp1, false);
            }

            // Assert that limit is reached.
            Assert.AreEqual(capturer.Buffersize, capturer.Shots.Count);

            // Assert that limit is not breached.
            capturer.TakeShot(mouseLeftDown1, mouseLeftUp1, false);
            Assert.AreEqual(capturer.Buffersize, capturer.Shots.Count);
        }

        [Test]
        public void CheckIsTakingShotsWorksTest()
        {
            // No shots in capturer at start.
            Assert.AreEqual(0, capturer.Shots.Count);

            capturer.TakeShot(mouseLeftDown1, mouseLeftUp1, false);
            Assert.AreEqual(0, capturer.Shots.Count);

            capturer.IsTakingShots = true;

            capturer.TakeShot(mouseLeftDown1, mouseLeftUp1, false);
            Assert.AreEqual(1, capturer.Shots.Count);

            capturer.IsTakingShots = false;
            capturer.TakeShot(mouseLeftDown1, mouseLeftUp1, false);
            Assert.AreEqual(1, capturer.Shots.Count);
        }

        [Test]
        public void SaveFilesClearShotsTest()
        {
            // No shots in capturer at start.
            Assert.AreEqual(0, capturer.Shots.Count);

            capturer.IsTakingShots = true;
            capturer.TakeShot(mouseLeftDown1, mouseLeftUp1, false);

            // Save shot to file, then remove the zip immediately.
            capturer.SaveShots();
            System.IO.Directory.Delete(ScreenCapturer.Properties.Settings.Default.SaveFolder, true);

            Assert.AreEqual(0, capturer.Shots.Count);
        }
    }
}
