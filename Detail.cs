using System;
using System.Threading;

namespace DetailPreparationMachine
{
    public class Detail
    {
        public int Id { get; }
        public bool IsCreated { get; set; }
        public bool IsReady { get; set; }
        public event EventHandler StartedCreation;
        public event EventHandler Created;
        public event EventHandler Ready;

        public Detail(int id)
        {
            Id = id;
            IsCreated = false;
            IsReady = false;
        }

        public void Prepare()
        {
            Random rnd = new Random();
            StartedCreation?.Invoke(this, EventArgs.Empty);

            Console.WriteLine("Creating detail " + Id.ToString());
            Thread.Sleep(rnd.Next(2000, 6000));
            Created?.Invoke(this, EventArgs.Empty);

            Console.WriteLine("Processing detail " + Id.ToString());
            Thread.Sleep(rnd.Next(4000, 6000));
            Ready?.Invoke(this, EventArgs.Empty);
        }

        public void Process()
        {
            IsReady = true;
        }

        public void Create()
        {
            IsCreated = true;
        }
    }
}
