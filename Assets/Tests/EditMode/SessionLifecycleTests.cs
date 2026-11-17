using NUnit.Framework;
using RvCulturas.Domain;

namespace RvCulturas.Tests.EditMode
{
    public class SessionLifecycleTests
    {
        [Test]
        public void PauseResumeRestartFollowValidTransitions()
        {
            var session = new ExperienceSessionState();
            session.Start(TestData.Experience());
            Assert.That(session.Pause(), Is.True);
            Assert.That(session.Resume(), Is.True);
            session.Restart();
            Assert.That(session.Lifecycle, Is.EqualTo(ExperienceLifecycle.Intro));
            Assert.That(session.CurrentStepOrdinal, Is.EqualTo(0));
        }
    }
}
