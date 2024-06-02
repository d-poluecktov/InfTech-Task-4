using System;

namespace DetailPreparationMachine
{
    public class Machine
    {
        public Machine() { }

        public void OnStartedCreation(object sender, EventArgs e)
        {
            var detail = sender as Detail;
            if (detail != null)
            {
                detail.Create();
            }
        }
    }
}
