using UnityEngine.UI;

namespace UI
{
    public class MainPanel : BasePanel
    {
        public Button Computer;
        public Button Log;
        public Button Action;
        public Button Technology;
        public Button Combat;
        public Button Reset;
        
        public override void OnEnter(object args = null)
        {
            base.OnEnter(args);
            
            
        }

        public override void OnExit()
        {
            base.OnExit();
            
        }
    }
}
