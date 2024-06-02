using System;

namespace DetailPreparationMachine
{
    public class Miller
    {
        public Miller() { }

        public void OnCreated(object sender, EventArgs e)
        {
            var detail = sender as Detail;
            if (detail != null)
            {
                detail.Process();
            }
        }
    }
}
