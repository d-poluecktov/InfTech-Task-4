using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading;

namespace DetailPreparationMachine
{
    public class PrepareDetailSimulation : INotifyPropertyChanged
    {
        private int _detailCount;
        private int _machineCount;
        private EventHandler _onStartedCreation;
        private EventHandler _onCreated;
        private EventHandler _onReady;
        private PrepareCommand _startSimulationCommand;

        public int DetailCount
        {
            get => _detailCount;
            set
            {
                _detailCount = value;
                OnPropertyChanged(nameof(DetailCount));
            }
        }

        public int MachineCount
        {
            get => _machineCount;
            set
            {
                if (value > 0 && value < 4)
                {
                    _machineCount = value;
                    OnPropertyChanged(nameof(MachineCount));
                }
                else
                {
                    throw new Exception("Value must be in [1,2]");
                }
            }
        }

        public PrepareCommand StartSimulationCommand
        {
            get
            {
                return _startSimulationCommand ??
                    (_startSimulationCommand = new PrepareCommand(obj =>
                    {
                        StartSimulation();
                    }));
            }
        }

        public ObservableCollection<Detail> Details { get; set; }
        public List<Machine> Machines { get; set; }
        public List<Miller> Millers { get; set; }
        public List<MainLoader> Loaders { get; set; }
        private List<Thread> _threads { get; set; }

        public PrepareDetailSimulation(EventHandler onStartedCreation, EventHandler onCreated, EventHandler onReady)
        {
            Machines = new List<Machine>();
            Millers = new List<Miller>();
            Loaders = new List<MainLoader>();
            _threads = new List<Thread>();
            Details = new ObservableCollection<Detail>();

            DetailCount = 1;
            MachineCount = 1;

            _onStartedCreation = onStartedCreation;
            _onCreated = onCreated;
            _onReady = onReady;

            //StartSimulation();
        }

        private void StartSimulation()
        {
            _fillObjects();
            foreach (var thread in _threads)
            {
                thread.Start();
            }
        }

        private void _fillObjects()
        {
            _threads.Clear();
            for (int index = 0; index < MachineCount; index++)
            {
                var machine = new Machine();
                var miller = new Miller();
                var loader = new MainLoader();
                
                for (int detIndex = 0; detIndex < DetailCount; detIndex++)
                {
                    var detail = new Detail((7+DetailCount)*index+7+detIndex);

                    detail.StartedCreation += machine.OnStartedCreation;
                    detail.StartedCreation += _onStartedCreation;

                    detail.Created += miller.OnCreated;
                    detail.Created += _onCreated;

                    detail.Ready += loader.OnReady;
                    detail.Ready += _onReady;

                    Details.Add(detail);

                    Thread thread = new Thread(detail.Prepare);
                    _threads.Add(thread);
                }
                
                Machines.Add(machine);
                Millers.Add(miller);
                Loaders.Add(loader);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
