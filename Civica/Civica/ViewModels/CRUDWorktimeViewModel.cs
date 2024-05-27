using Civica.Interfaces;
using Civica.Models;
using GVMR;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Civica.ViewModels
{
    public class CRUDWorktimeViewModel : ObservableObject, IViewModelChild
    {
        private ExpandedResourceViewModel ervm;
        private IRepository<Worktime> worktimeRepo;

        private string _involvedName;
        public string InvolvedName
        {
            get => _involvedName;
            set
            {
                _involvedName = value;
                OnPropertyChanged(nameof(InvolvedName));
            }
        }
        private string _estimatedHours = "";
        public string EstimatedHours
        {
            get => _estimatedHours;
            set
            {
                if (double.TryParse(value, out _) || value == "")
                {
                    _estimatedHours = value;
                }
                else
                {
                    MessageBox.Show("'Est. timer' må kun være i tal.");
                }
                OnPropertyChanged(nameof(EstimatedHours));
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public void Init(ObservableObject o)
        {
            ervm = (o as ExpandedResourceViewModel);

            worktimeRepo = ervm.mvm.GetWorktimeRepo();
        }

        public void CreateWorktime(int userId, int resourceId, string estimatedHours, string involvedName, string description, DateTime dateTime)
        {
            Worktime w = new Worktime(userId, resourceId, int.TryParse(EstimatedHours, out int r) ? r : 0, involvedName, description, dateTime);

            worktimeRepo.Add(w);

            ervm.Worktimes.Clear();

            ervm.UpdateList();
            EstimatedHours = "";
            InvolvedName = "";
            Description = "";
        }

        public void UpdateWorktime(WorktimeViewModel wvm)
        {
            Worktime w = worktimeRepo.GetById(x => x.Id == wvm.GetId());

            w.SpentHours = int.Parse(wvm.SpentHours);
            w.EstimatedHours = int.Parse(wvm.EstimatedHours);
            w.Description = wvm.Description;
            w.InvolvedName = wvm.InvolvedName;

            worktimeRepo.Update(w);

            ervm.Worktimes.Clear();

            ervm.UpdateList();

            ervm.SelectedWorktime = wvm;
        }

        public void DeleteWorktime(WorktimeViewModel wvm)
        {
            Worktime w = worktimeRepo.GetById(x => x.Id == wvm.GetId());

            worktimeRepo.Delete(w);

            ervm.Worktimes.Clear();

            ervm.UpdateList();
        }
    }
}