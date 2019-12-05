using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kowloon.Core
{
    public sealed class KowloonManager : IDisposable
    {
        private readonly KowloonController Controller;

        private List<IRenderer> _TestRenderers = new List<IRenderer>();
        public ReadOnlyCollection<IRenderer> TestRenderers { get; }

        public IRenderer CurrentTestRenderer => Controller.OverrideRenderer;

        public KowloonManager()
        {
            Controller = new KowloonController();

            _TestRenderers = new List<IRenderer>()
            {
                new ChaserPattern(Controller),
                new CyberPattern(Controller),
                new SolidColorCyclePattern(Controller),
                new SolidWhitePattern(Controller),
            };
            TestRenderers = _TestRenderers.AsReadOnly();
        }

        public void SetTestRenderer(IRenderer testRenderer)
        {
            if (testRenderer != null && !_TestRenderers.Contains(testRenderer))
            { throw new ArgumentException("The specified renderer does not belong to this manager."); }

            Controller.OverrideRenderer = testRenderer;
        }

        public void DisableTestRenderer()
            => SetTestRenderer(null);

        public void Dispose()
            => Controller.Dispose();
    }
}
