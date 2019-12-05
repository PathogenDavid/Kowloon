namespace Kowloon.Core
{
    public class DisplayMute : IRenderer
    {
        private readonly KowloonController Controller;

        internal DisplayMute(KowloonController controller)
            => Controller = controller;

        public void Render(bool isFirstFrame)
            => Controller.Leds.Clear();
    }
}
