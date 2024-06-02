using System;

namespace DetailPreparationMachine
{
    public class MainLoader : ILoader
    {
        public MainLoader() { }
        public void PickUpDetail(Detail detail)
        {
            Console.WriteLine("Picking up detail " + detail.Id.ToString());
        }

        public void OnReady(object sender, EventArgs e)
        {
            var detail = sender as Detail;
            if (detail != null)
            {
                PickUpDetail(detail);
            }
        }
    }
}
